using System;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MapGen
{
	[CreateAssetMenu(fileName = "Map Generator", menuName = "Golf/Map Generator", order = 0)]
	public class Generator : ScriptableObject
	{
		public Action<Texture2D> OnGenerate;
		public Vector2Int Size => size;
		[SerializeField] private Vector2Int size = new Vector2Int(10, 10);
		[SerializeField] private float perlinScale;
		[SerializeField] private AnimationCurve heightCurve;
		public Texture2D MapImage => _texture2D;
		private Texture2D _texture2D;

		//just fell silly to do this one a hundred times. No noticable difference when i profiled it.
		private float sqrtTwo;

		[Header("Generation Settings")] public int initialCellularSteps = 10;
		[ContextMenu("Generate")]

		public void Generate()
		{
			//caching and seeds
			sqrtTwo = Mathf.Sqrt(2);
			
			
			SaveTexture2D();//creates if it is null. Creates a new one at the correct size if size has changed.

			SetAllToRandomBlackOrWhite();
			for (int i = 0; i < initialCellularSteps; i++)
			{
				ProcessFourFive();
			}
			
			//todo: blur our islands using a convolution.
			
			PerlinScale(Random.Range(0,1000f));
			_texture2D.Apply();
			SaveTexture2D();
			
			OnGenerate?.Invoke(_texture2D);
		}

		private void PerlinScale(float seed)
		{
			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					var c = _texture2D.GetPixel(x, y);

					//Center Bump
					//can't believe i got stuck on an issue where i didn't know it was doing integer division.
					var nx = (2 * (x / (float)(size.x))) - 1;
					var ny = (2 * (y / (float)(size.y))) - 1;
					float d = Mathf.Min(1, (nx * nx + ny * ny) / sqrtTwo);
					//d = 1 - (1 - nx * nx) * (1 - ny * ny);
					
					var height = Mathf.PerlinNoise(x / perlinScale*size.x+seed, y / perlinScale*size.x+seed);
					height = (height + (1 - d)) / 2;
					height = heightCurve.Evaluate(height);
					//_texture2D.SetPixel(x,y,new Color(c.r* height,c.g* height,c.b* height,1));
					_texture2D.SetPixel(x, y, new Color((c.r + height) / 2, (c.g + height) / 2, (c.b + height) / 2));

					//_texture2D.SetPixel(x, y, new Color(height,  height, height, 1));

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