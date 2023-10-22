using UnityEngine;
using UnityEngine.Rendering;

namespace MapGen
{
	[CreateAssetMenu(fileName = "Terrain", menuName = "Golf/Sliced Terrain Definition", order = 0)]
	public class SlicedTerrain : ScriptableObject
	{
		[SerializeField] private GameObject[,,] _map = new GameObject[3, 3, 3];


		//what's the input here...
		public GameObject GetTile()
		{
			return null;
		}

		public void SetObject(int layer, int row, int col, GameObject obj)
		{
			_map[layer, row, col] = obj;
		}

		public GameObject GetPrefab(int layer, int row, int col)
		{
			return _map[layer, row, col];
		}

		public void InstantiateRect(Vector3 position, Vector3Int size, Transform transform)
		{
			
		}
	}
}