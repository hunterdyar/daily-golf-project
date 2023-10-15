using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using Utilities.Kernels;
using Random = UnityEngine.Random;

namespace MapGen
{
	[CreateAssetMenu(fileName = "Map Generator", menuName = "Golf/Map Generator", order = 0)]
	public class Generator : ScriptableObject
	{
		public Action<Texture2D> OnGenerate;
		public Vector2Int Size => size;
		public int numberTees = 5;
		[SerializeField] private Vector2Int size = new Vector2Int(10, 10);
		[SerializeField] private float perlinScale;
		[SerializeField] private AnimationCurve heightCurve;
		[Range(0, 1)] public float waterLevel;
		public Texture2D MapImage => _texture2D;
		private Texture2D _texture2D;

		//just fell silly to do this one a hundred times. No noticable difference when i profiled it.
		private readonly float sqrtTwo = Mathf.Sqrt(2);

		public List<Vector2Int> teePositions;

		[Header("Generation Settings")] public int initialCellularSteps = 10;
		[ContextMenu("Generate")]

		public void Generate()
		{
			//seeds?
			
			SaveTexture2D();//creates if it is null. Creates a new one at the correct size if size has changed.

			SetAllToRandomBlackOrWhite();
			for (int i = 0; i < initialCellularSteps; i++)
			{
				ProcessFourFive();
			}
			
			//todo: blur our islands using a convolution.
			ApplyKernel(KernelUtility.BoxKernal);
			//Kernel breaks our edges, which is fine. We'll just generate more than we need and 'delete' the outer ring.
			//If the kernal is > 3x3 then I will need to rewrite this to take border thickness.
			SetBordersToColor(Color.black);
			

			PerlinScale(Random.Range(0,1000f));
			CenterShapeGreyscale(0.1f);

			_texture2D.Apply();

			//apply water level...
			ApplyWaterLevelGreyscale(waterLevel,Color.black);
			
			CalculateTeePositions();
			
			SaveTexture2D();
			
			
			OnGenerate?.Invoke(_texture2D);
		}

		private void ApplyWaterLevelGreyscale(float waterLevel,Color color)
		{
			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					if (_texture2D.GetPixel(x, y).grayscale < waterLevel)
					{
						_texture2D.SetPixel(x,y, color);
					}
				}
			}
		}

		private void CalculateTeePositions()
		{
			teePositions = new List<Vector2Int>();
			if (numberTees <= 0)
			{
				return;
			}

			int triesBeforeRadiusShrink = size.x*100;
			float radius = Mathf.Sqrt(size.x * size.y);
			int totalTries = 0;
			int giveUp = 1000;
			while (teePositions.Count < numberTees && totalTries < giveUp)
			{
				int sampleTries = 0;
				while (sampleTries < triesBeforeRadiusShrink)
				{
					var testingPos = GetRandomLandPoint();
					bool validTest = true;
					foreach (var position in teePositions)
					{
						if (Vector2Int.Distance(position, testingPos) < radius)
						{
							//we also have to check that the tee isn't on an isoldated island.
							//I think it will be faster to just generate and validate, and regenerate after, instead of doing the sample in the check.
							//having a list of islands would be nice anyway to do flood fills on.
							validTest = false;
							break;
						}
					}

					if (validTest)
					{
						teePositions.Add(testingPos);
						if (teePositions.Count == numberTees)
						{
							//possible to add multiple tees on one attempt-before-radius shrink, so check and bail.
							return;
						}
					}

					sampleTries++;
				}

				totalTries++;
				radius = radius - 1;
				radius = Mathf.Max(radius, 0);//clamp
			}
		}

		private Vector2Int GetRandomLandPoint(int maxTries = 1000)
		{
			int tries = 0;
			while (tries < maxTries)
			{
				Vector2Int p = new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));
				if (_texture2D.GetPixel(p.x, p.y).grayscale > waterLevel)
				{
					return p;
				}
				tries++;
			}

			Debug.LogWarning("Couldn't get random land point.");
			return Vector2Int.zero;
		}

		private void SetBordersToColor(Color color)
		{
			for (int x = 0; x < size.x; x++)
			{
				_texture2D.SetPixel(x,0, color);
				_texture2D.SetPixel(x, size.y-1, color);
			}

			for (int y = 0; y < size.y; y++)
			{
				_texture2D.SetPixel(0, y, color);
				_texture2D.SetPixel(size.x-1, y, color);
			}
		}

		private void ApplyKernel(float[,] kernel)
		{
			Texture2D postTex = new Texture2D(size.x, size.y);

			int offset = kernel.GetLength(0) / 2;
			int kw = kernel.GetLength(0);
			int kh = kernel.GetLength(1);

			float[] color = new float[3];
			
			for (int x = offset; x < size.x - offset; x++)
			{
				for (int y = offset; y < size.y - offset; y++)
				{
					color[0] = 0;
					color[1] = 0;
					color[2] = 0;

					for (int a = 0; a < kw; a++)
					{
						for (int b = 0; b < kw; b++)
						{
							var xn = x + a - offset;
							var yn = y + b - offset;
							Color source = _texture2D.GetPixel(xn, yn);
							color[0] += source.r * kernel[a,b];
							color[1] += source.g * kernel[a,b];
							color[2] += source.b * kernel[a,b];
						}
					}

					postTex.SetPixel(x,y,new Color(color[0],color[1],color[2]));
				}
			}
				
			postTex.Apply();
			_texture2D.SetPixels(postTex.GetPixels());

		}

		private void PerlinScale(float seed)
		{
			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					var c = _texture2D.GetPixel(x, y);

					
					//d = 1 - (1 - nx * nx) * (1 - ny * ny);
					
					var height = Mathf.PerlinNoise(x / perlinScale*size.x+seed, y / perlinScale*size.x+seed);
					height = heightCurve.Evaluate(height);
					//_texture2D.SetPixel(x,y,new Color(c.r* height,c.g* height,c.b* height,1));
					_texture2D.SetPixel(x, y, new Color((c.r + height) / 2, (c.g + height) / 2, (c.b + height) / 2));

					//_texture2D.SetPixel(x, y, new Color(height,  height, height, 1));

				}
			}
		}

		private void CenterShapeGreyscale(float t)
		{
			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					//Center Bump
					//can't believe i got stuck on an issue where i didn't know it was doing integer division.
					float s = 2.5f;//size of island. 2 extends to map edges, larger values compact to center.
					var nx = (s * (x / (float)(size.x))) - s/2f;
					var ny = (s * (y / (float)(size.y))) - s/2f;
					float pow = 2;
					float d = 1-Mathf.Min(1, (Mathf.Pow(nx,pow) + Mathf.Pow(ny, pow)) / sqrtTwo)-0.2f;
					//var height = (_texture2D.GetPixel(x,y).grayscale + (1 - d)) / 2;
					var height = Mathf.Lerp(_texture2D.GetPixel(x, y).grayscale, d, t);

					_texture2D.SetPixel(x, y,new Color(height, height,height));

				}
			}

		}
		public void SaveTexture2D()
		{
			//if the variable got unassigned the asset still exists, so reset it.
			_texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GetAssetPath(this));
			
			if (_texture2D == null)
			{
				_texture2D = new Texture2D(size.x, size.y);
				_texture2D.filterMode = FilterMode.Point;
				// _texture2D
				_texture2D.name = this.name + "_tex";
#if UNITY_EDITOR
				AssetDatabase.AddObjectToAsset(_texture2D, AssetDatabase.GetAssetPath(this));
				AssetDatabase.SaveAssets();//can't do this during import. onenable happens during import.
#endif
			}

			//if size changed, update.
			if (_texture2D.width != size.x || _texture2D.height != size.y)
			{
				_texture2D.Reinitialize(size.x, size.y);
				_texture2D.filterMode = FilterMode.Point;
#if UNITY_EDITOR
				AssetDatabase.SaveAssets();
#endif
			}
			
#if UNITY_EDITOR
			EditorUtility.SetDirty(_texture2D);
			AssetDatabase.SaveAssetIfDirty(AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(_texture2D)));
#endif
			
		}

		public void SetAllToRandomBlackOrWhite()
		{
			for (int i = 0; i < size.x; i++)
			{
				for (int j = 0; j < size.y; j++)
				{
					_texture2D.SetPixel(i, j, Random.value > 0.5f ? Color.white : Color.black);
				}
			}
		}

		int CountColor(Color col)
		{
			return _texture2D.GetPixels().Count(c => c == col);
		}

		int CountColorNeighbors(int x, int y, Color color)
		{
			int c = 0;
			for (int i = x - 1; i<= x + 1;i++)
			{
				for (int j = y- 1; j <= y + 1; j++)
				{
					if (i >= 0 && i < size.x && j >= 0 && i < size.y && !(i == x && j == y))
					{
						c += _texture2D.GetPixel(i, j) == color ? 1 : 0;
					}
				}
			}

			return c;
		}

		//http://www.roguebasin.com/index.php?title=Cellular_Automata_Method_for_Generating_Random_Cave-Like_Levels
		private void ProcessFourFive()
		{
			Texture2D newTexture = new Texture2D(size.x, size.y);
			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					int n = CountColorNeighbors(x,y,Color.white);

					//w is number of wall neighbors
					int w = 8 - n; //this is true while we only have walls and floors.
					var c = _texture2D.GetPixel(x, y);
					//: a tile becomes a wall if it was a wall and 4 or more of its eight neighbors were walls,
					if (c == Color.black && w >= 4)
					{
						newTexture.SetPixel(x,y,Color.black);
					}
					else if (c == Color.white && w >= 5)
					{
						//if it was not a wall and 5 or more neighbors were.
						newTexture.SetPixel(x, y, Color.black);
					}
					else
					{
						newTexture.SetPixel(x, y, Color.white);
					}
				}
			}

			newTexture.Apply();
			//pull from copy
			_texture2D.SetPixels(newTexture.GetPixels());
		}
	}
}