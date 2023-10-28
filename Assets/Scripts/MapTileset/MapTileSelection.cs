using System;
using UnityEngine;
using UnityEngine.Playables;

namespace MapTileset
{
	[Serializable]
	public class MapTileSelection
	{
		public TileFaceType Face;
		public GameObject Prefab;

		public TileNeighbor Above;//positive y
		public TileNeighbor Below;
		
		public TileNeighbor Right;
		public TileNeighbor Left;
		public TileNeighbor Forward;
		public TileNeighbor Back;
		
		public TileNeighbor ForwardRightCorner;
		public TileNeighbor ForwardLeftCorner;
		public TileNeighbor BackRightCorner;
		public TileNeighbor BackLeftCorner;
	}
}