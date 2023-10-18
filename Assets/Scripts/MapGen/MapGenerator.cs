using System;
using System.Collections.Generic;
using Golf;
using UnityEngine;
using Utilities.ReadOnlyAttribute;
using Random = UnityEngine.Random;

namespace MapGen
{
	public class MapGenerator : MonoBehaviour
	{
		public static Action<MapGenerator> OnGenerationComplete;
		
		public Generator _generator;
		public GameObject GroundPrefab;
		public GameObject TeePrefab;

		public float scale = 1;
		public AnimationCurve perlinHeightCurve;
		public int steps = 3;
		public bool GenerateOnStart = true;

		[SerializeField] private MeshColliderCookingOptions cookingOptions;
		[ReadOnly]
		public List<Collider> EnvironmentColliders = new List<Collider>();
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
			EnvironmentColliders.Clear();
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}

			List<CombineInstance> meshes = new List<CombineInstance>();
			for (int x = 0; x < tex.width; x++)
			{
				for (int y = 0; y < tex.height; y++)
				{
					var prefab = ColorToPrefab(tex.GetPixel(x, y));
					if (prefab != null)
					{
						var o = Instantiate(prefab, transform);
						o.transform.position = new Vector3(x * scale, GetHeight((tex.GetPixel(x, y).grayscale)), y * scale);
						CombineInstance ci = new CombineInstance();

						var c = o.GetComponent<Collider>();
						if (c != null && c is MeshCollider instanceMeshCollider)
						{
							ci.mesh = instanceMeshCollider.sharedMesh;//we will be merging all of the meshes.
							instanceMeshCollider.enabled = false;
						}
						else
						{
							if (c != null)
							{
								Debug.LogWarning("Environment Geometry should use MeshColliders or No colliders (Meshes). Terrain will be one joined mesh, and must come from these. Using regular mesh for environment collider.",o);
							}
							ci.mesh = o.GetComponent<MeshFilter>().sharedMesh;
						}

						
						
						ci.transform = o.transform.localToWorldMatrix;
						meshes.Add(ci);
					}
				}
			}
			
			//
			Mesh mesh = new Mesh();
			mesh.CombineMeshes(meshes.ToArray());
			GameObject combined = new GameObject();
			Physics.BakeMesh(mesh.GetInstanceID(),false,cookingOptions);
			//
			var mc = combined.AddComponent<MeshCollider>();
			mc.sharedMesh = mesh;
			combined.name = "World Collider";
			EnvironmentColliders.Add(mc);
			
			
			//Update Player

			Vector2Int playerPosition = _generator.teePositions[0];
			var player = GameObject.FindObjectOfType<GolfMovement>();
			 var pos = new Vector3(playerPosition.x, (steps + 1) * scale, playerPosition.y);
			 //took me a while to figure out why setting transform directly didn't work... dang rigidbodies...
			 player.GetComponent<Rigidbody>().position = transform.TransformPoint(pos);

			 for (int i = 1; i < _generator.teePositions.Count; i++)
			 {
				 var o = Instantiate(TeePrefab, transform);
				 o.transform.position = new Vector3(_generator.teePositions[i].x * scale, GetHeight((tex.GetPixel(_generator.teePositions[i].x, _generator.teePositions[i].y).grayscale))+scale, _generator.teePositions[i].y * scale);
			 }

			 OnGenerationComplete?.Invoke(this);
		}

		private float GetHeight(float input)
		{
			input = Mathf.Round(input * steps) / steps;
			input = input * scale;
			return input;
		}

		private GameObject ColorToPrefab(Color color)
		{
			float g = color.grayscale;
			if (g > 0.01f)
			{
				return GroundPrefab;
			}

			return null;
		}
	}
}