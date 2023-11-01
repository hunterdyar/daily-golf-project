using System;
using UnityEngine;

namespace MapTileset
{
	[CreateAssetMenu(fileName = "Map Tileset", menuName = "Golf/Map 3D Tileset", order = 0)]
	public class MapTileset : ScriptableObject
	{
		
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
			
			return (null,Quaternion.identity);
		}
	}
}