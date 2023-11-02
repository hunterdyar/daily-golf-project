using System;
using UnityEngine;

//todo rename
namespace MapTileset
{
	[CreateAssetMenu(fileName = "Map Tileset", menuName = "Golf/Map 3D Tileset", order = 0)]
	public class MapTileset : ScriptableObject
	{
		//todo: custom editor so this doesn't allow non-prefabs.
		[SerializeField] private GameObject _defaultPrefab;
		
		[SerializeField]
		private MapTileSelection[] _tilePatterns;
		
		public (GameObject, Quaternion) GetPrefabAndOrientation(MapTileSelection.NeighborTestDelegate NeighborTest, Vector3Int pos)
		{
			foreach (var pattern in _tilePatterns)
			{
				if (pattern.Matches(NeighborTest, pos, out var match))
				{
					return match;
				}
			}
			
			return (_defaultPrefab,Quaternion.identity);
		}
	}
}