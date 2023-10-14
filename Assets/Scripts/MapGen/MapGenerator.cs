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
		public float scale = 1;
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

			Vector2Int playerPosition = new Vector2Int(Random.Range(0,tex.width), Random.Range(0, tex.height));
			while (tex.GetPixel(playerPosition.x, playerPosition.y).grayscale < 0.1f)
			{ 
				playerPosition = new Vector2Int(Random.Range(0, tex.width), Random.Range(0, tex.height));
			}

			var player = GameObject.FindObjectOfType<GolfMovement>();
			 var pos = new Vector3(playerPosition.x, (steps + 1) * scale, playerPosition.y);
			 //took me a while to figure out why setting transform directly didn't work... dang rigidbodies...
			 player.GetComponent<Rigidbody>().position = transform.TransformPoint(pos);
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
			if (g > 0.1f)
			{
				return GroundPrefab;
			}

			return null;
		}
	}
}