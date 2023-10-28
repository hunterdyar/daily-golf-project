using System;
using UnityEngine;

namespace MapTileset
{
	[CreateAssetMenu(fileName = "Map Tileset", menuName = "Golf/Map 3D Tileset", order = 0)]
	public class MapTileset : ScriptableObject
	{
		[SerializeField]
		private MapTileSelection[] _tilePatterns;
	}
}