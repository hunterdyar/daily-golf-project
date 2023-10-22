using System;
using UnityEngine;

namespace MapGen
{
	[CreateAssetMenu(fileName = "Terrain", menuName = "Golf/Sliced Terrain Definition", order = 0)]
	public class SlicedTerrain : ScriptableObject
	{
		[SerializeField] private GameObject[,,] _map = new GameObject[3, 3, 3];

		public delegate bool SampleTest(int x, int y, int nx, int ny);
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

		public GameObject PrefabFromXZNeighbors(SampleTest neighborTest, int x, int y, TerrainHeightLayer layer)
		{
			bool right = neighborTest.Invoke(x,y,1,0);
			bool left = neighborTest.Invoke(x, y, -1, 0);
			bool up = neighborTest.Invoke(x, y, 0, 1);
			bool down = neighborTest.Invoke(x, y, 0, 1);

			//write bool functions for HasNeighborInDir()
			//Then we sample hasNeighborInDoor and do BitWise and's.
			//for each one, we do a bitwise OR, for our neighnorDirKey, which we cast to an enum for debugging.
			//then we grab the right map from a const lookup between positions and these enums.
			return null;
		}
	}
}