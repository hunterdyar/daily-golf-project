using System;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MapTileset
{
	[Serializable]
	public class MapTileSelection
	{
		private static readonly Quaternion[] NoRotations = new[] { Quaternion.identity };
		private static readonly Quaternion[] Rotations90 = new[] { Quaternion.identity, Quaternion.Euler(0,90,0),
			Quaternion.Euler(0, 180, 0), Quaternion.Euler(0, 270, 0)
		};
		private static readonly Quaternion[] Rotations180 = new[]
		{
			Quaternion.identity,
			Quaternion.Euler(0, 180, 0),
		};

		private static readonly Vector3Int ForwardRight = new Vector3Int(1, 0, 1);
		private static readonly Vector3Int ForwardLeft = new Vector3Int(-1, 0, 1);
		private static readonly Vector3Int BackRight = new Vector3Int(1, 0, -1);
		private static readonly Vector3Int BackLeft = new Vector3Int(-1, 0, -1);

		private static readonly Vector3Int[] TestDirections = new[]
		{
			Vector3Int.up,
			Vector3Int.down,
			Vector3Int.back,
			Vector3Int.forward,
			Vector3Int.left,
			Vector3Int.right,
			ForwardRight,
			ForwardLeft,
			BackLeft,
			BackRight
		};
		
		public delegate bool NeighborTestDelegate(Vector3Int pos, Vector3Int dir);
		
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
		
		public bool Matches(NeighborTestDelegate neighborTest, Vector3Int pos, out (GameObject, Quaternion) match)
		{
			Dictionary<Vector3Int, bool> resultCache = new Dictionary<Vector3Int, bool>();
			var rotations = GetRotationsForFace(Face);

			match.Item1 = Prefab;
			match.Item2 = Quaternion.identity;
			
			foreach (var o in rotations)
			{
				bool valid = true;
				//var dir = RotatedVector3Int(new Vector3Int(0, 1, 0),o);
				foreach(Vector3Int dir in TestDirections)
				{
					var face = GetFaceForDir(dir);

					if (face == TileNeighbor.Any)
					{
						continue;
					}
					
					bool a = false;

					//if dir is in the cache, a gets set.
					if (!resultCache.TryGetValue(dir, out a))
					{
						a = neighborTest(pos, dir);
						resultCache.Add(dir, a);
					}

					//get our results.
					if (face == TileNeighbor.HasNeighbor)
					{
						if (!a)
						{
							//invalid test.
							valid = false;
							break;
						}
						else
						{
							continue;
						}
					}else if (face == TileNeighbor.NoNeighbor)
					{
						if (a)
						{
							//invalid test
							valid = false;
							break;
						}
						else
						{
							continue;
						}
					}
				}

				if (valid)
				{
					//match.Item1 = Prefab;
					match.Item2 = o;
					return true;
				}
			}

			return false;
			
		}

		public TileNeighbor GetFaceForDir(Vector3Int dir)
		{
			//todo: cache a dictionary...
			//we know that these properties don't change, but we shouldn't assume that. Our cache should reset for each lookup.
			//so lets just do this for now, and turn it into a Vector3Cube later.
			if (dir == Vector3Int.up)
			{
				return Above;
			}else if (dir == Vector3Int.right)
			{
				return Right;
			}else if (dir == Vector3Int.back)
			{
				return Back;
			}
			else if (dir == Vector3Int.left)
			{
				return Left;
			}
			else if (dir == Vector3Int.down)
			{
				return Below;
			}
			else if (dir == Vector3Int.forward)
			{
				return Forward;
			}
			else if (dir == BackLeft)
			{
				return BackLeftCorner;
			}
			else if (dir == BackRight)
			{
				return BackRightCorner;
			}
			else if (dir == ForwardRight)
			{
				return ForwardRightCorner;
			}
			else if (dir == ForwardLeft)
			{
				return ForwardLeftCorner;
			}

			Debug.LogWarning("Bad Orientation");
			return TileNeighbor.Any;
		}

		private Vector3Int RotatedVector3Int(Vector3Int dir, Quaternion rot)
		{
			var newDir = rot * dir;
			return new Vector3Int((int)newDir.x, (int)newDir.y, (int)newDir.z);
		}
		public static Quaternion[] GetRotationsForFace(TileFaceType face)
		{
			switch (face)
			{
				case TileFaceType.Rotate90:
					return Rotations90;
				case TileFaceType.Rotate180:
					return Rotations180;
				case TileFaceType.NoRotate:
				default:
					return NoRotations;
			} 
		}
	}
}