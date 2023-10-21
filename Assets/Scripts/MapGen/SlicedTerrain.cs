using UnityEngine;

namespace MapGen
{
	[CreateAssetMenu(fileName = "Terrain", menuName = "Golf/Sliced Terrain Definition", order = 0)]
	public class SlicedTerrain : ScriptableObject
	{
		[SerializeField] private GameObject[,] _map;

		
		//what's the input here...
		public GameObject GetTile()
		{
			return null;
		}
	}
}