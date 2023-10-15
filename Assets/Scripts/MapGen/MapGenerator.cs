using System;
using Golf;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MapGen
{
	public class MapGenerator : MonoBehaviour
	{
		public Generator _generator;
		public GameObject GroundPrefab;
		public GameObject TeePrefab;

		public float scale = 1;
		public AnimationCurve perlinHeightCurve;
		public int steps = 3;
		public bool GenerateOnStart = true;

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
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}
			
			for (int x = 0; x < tex.width; x++)
			{
				for (int y = 0; y < tex.height; y++)
				{
					var prefab = ColorToPrefab(tex.GetPixel(x, y));
					if (prefab != null)
					{
						var o = Instantiate(prefab, transform);
						o.transform.position = new Vector3(x * scale, GetHeight((tex.GetPixel(x, y).grayscale)), y * scale);
					}
				}
			}

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