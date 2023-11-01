using System;
using System.Collections.Generic;
using Golf;
using UnityEditor;
using UnityEngine;
using Utilities.ReadOnlyAttribute;
using Random = UnityEngine.Random;

namespace MapGen
{
	public class MapGenerator : MonoBehaviour
	{
		public static Action<MapGenerator> OnGenerationComplete;


		public MapTileset.MapTileset _mapTileset;
		public Generator _generator;
		public GameObject GroundPrefab;
		public GameObject TeePrefab;

		private const float BlackThreshold = 0.01f;
		public float scale = 1;
		public int steps = 3;
		public bool GenerateOnStart = true;

		[SerializeField] private MeshColliderCookingOptions cookingOptions;
		[ReadOnly] public List<Collider> EnvironmentColliders = new List<Collider>();

		private Texture2D _genTex;

		private void Start()
		{
			if (GenerateOnStart)
			{
				_generator.Generate();
			}
		}

		private void OnEnable()
		{
			_generator.OnGenerate += OnGenerate;
		}

		private void OnDisable()
		{
			_generator.OnGenerate -= OnGenerate;
		}

		private void OnGenerate(Texture2D tex)
		{
			_genTex = tex;
			//Delete and reset from last generation, so we can spam and test without exiting play mode.
			EnvironmentColliders.Clear();
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}

			//store any mesh we will take from the prefab, remove from the prefab, and then add to this combineinstance thing.
			List<CombineInstance> meshes = new List<CombineInstance>();

			//loop through input texture (the map was generated as a Texture2D, AKA an image)
			//use world space positions for variable names. the texture is on the XZ plane.
			for (int x = 0; x < tex.width; x++)
			{
				for (int z = 0; z < tex.height; z++)
				{
					for (int y = 0; y < steps; y++)
					{
						//Skip empty areas.
						if (!IsSpaceHere(new Vector3Int(x, y, z)))
						{
							continue;
						}
						
						var spawn = _mapTileset.GetPrefabAndOrientation(NeighborTest, new Vector3Int(x, y, z));
						if (spawn.Item1 != null)
						{
							//instantiate the cube
							var o = Instantiate(spawn.Item1, new Vector3(x*scale,y*scale,z*scale),spawn.Item2, transform);
							//o.transform.position = new Vector3(x * scale, GetHeight((tex.GetPixel(x, y).grayscale)), y * scale);

							//for mesh combining, this we set to be a mesh.
							CombineInstance ci = new CombineInstance();

							var c = o.GetComponentInChildren<Collider>();
							//if the object we are cloning has a MeshCollider, let's grab it's mesh.
							if (c != null && c is MeshCollider instanceMeshCollider)
							{
								ci.mesh = instanceMeshCollider.sharedMesh; //we will be merging all of the meshes.
								instanceMeshCollider.enabled = false;
							}
							else
							{
								if (c != null)
								{
									Debug.LogWarning(
										"Environment Geometry should use MeshColliders or No colliders (Meshes). Terrain will be one joined mesh, and must come from these. Using regular mesh for environment collider.",
										o);
								}

								//if the object we are cloning does NOT have a mesh collider, we can't use the BoxCollider or SphereCollider, since they don't use meshes under the hood, but optimized equations
								//so we will use the regular mesh from the Mesh Renderer instead.
								ci.mesh = o.GetComponent<MeshFilter>().sharedMesh;
							}

							//one way or the other, we set the .mesh property of the CombineInstance to the mesh of our thing, and then add it to a list (below)
							ci.transform = o.transform.localToWorldMatrix; //set's the position of the combine.
							meshes.Add(ci);
						}
					}
				}
			}

			//Now, make a NEW mesh out of the list of combined ones.
			Mesh mesh = new Mesh();
			mesh.CombineMeshes(meshes.ToArray()); //CombineMeshes doing the real work.

			Physics.BakeMesh(mesh.GetInstanceID(), false,
				cookingOptions); //the cookingOptions are set to have Weld Colocated Vertices and EnableMeshCleaning.
			//a bunch of cubes will have LOTS Of overlapping vertices, they were spawned that way. So we want to let unity clean it all up for us - which it can do with this BakeMesh function.

			//with the mesh, we make a new game object and give it a mesh.
			//curiously, we don't need a mesh filter, because this mesh is already in RAM and processed, so we can skip that component.
			GameObject combined = new GameObject();
			var mc = combined.AddComponent<MeshCollider>();
			mc.sharedMesh = mesh;
			combined.name = "World Collider";

			//EnvironmentCOlliders is used by the physics trajectory prediction system, so it knows what objects to clone into it's fake scene.
			EnvironmentColliders.Add(mc);


			//Update Player
			Vector2Int playerPosition = _generator.teePositions[0];
			var player =
				GameObject
					.FindObjectOfType<
						GolfMovement>(); //This can't be the best way to do this, but I don't know what will trigger MapGeneration in the future.
			//caddy doesn't get currentPlayer until Awake, which is probably when player should happen.
			//todo: I think that this map generation should also just spawn in the player prefab itself. 
			//Make this change after camera works, since it will need to get a reference to the player at runtime too.

			var pos = new Vector3(playerPosition.x, (steps + 1) * scale, playerPosition.y);
			//took me a while to figure out why setting transform directly didn't work... had to edit position via rigidobody. Odd race condition issue. dang rigidbodies...
			player.GetComponent<Rigidbody>().position = transform.TransformPoint(pos);

			//spawn in the tee prefabs. One for each position.
			//i starts at 1 not 0, because we use the first position for the player object.
			for (int i = 1; i < _generator.teePositions.Count; i++)
			{
				var o = Instantiate(TeePrefab, transform);
				o.transform.position = new Vector3(_generator.teePositions[i].x * scale,
					GetHeight((tex.GetPixel(_generator.teePositions[i].x, _generator.teePositions[i].y).grayscale)) *
					scale, _generator.teePositions[i].y * scale);
			}

			//
			OnGenerationComplete?.Invoke(this);
		}

		//Generation right now is smooth, from 0 to 1. we block-ify it here.
		private float GetHeight(float input)
		{
			input = Mathf.Clamp01(input);
			input = Mathf.Round(input * steps) / steps;
			input = input * scale;
			return input;
		}


		//todo: this will need to get the image and coordinates, because it will need to sample around it, to figure out if it's a corner or water, and get the right prefab from a tile set.
		//the tile set also needs to be defined in some way. Sounds like a new system.

		/// <summary>
		/// Given a tile in the texture2D, returns the appropriate prefab.
		/// </summary>
		private GameObject ColorToPrefab(Color color)
		{
			float g = color.grayscale;
			if (g > BlackThreshold)
			{
				return GroundPrefab;
			}

			return null;
		}
		

		public bool NeighborTest(Vector3Int pos, Vector3Int dir)
		{
			var n = pos + dir;
			return IsSpaceHere(n);
		}

		public bool IsSpaceHere(Vector3Int pos)
		{
			//bounds
			if (pos.x < 0 || pos.y < 0 || pos.z < 0 || pos.x >= _genTex.width || pos.z >= _genTex.height || pos.y > steps)
			{
				return false;
			}

			float h = _genTex.GetPixel(pos.x, pos.z).grayscale;
			if (h <= BlackThreshold)
			{
				return false;
			}

			//we divide the world into steps. if we are above the h, it's air. No tunnels or overhangs. 
			return h >= ((pos.y) / (float)steps);
		}
	}
}