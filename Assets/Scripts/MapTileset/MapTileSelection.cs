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
			Vector3Int.forward,
			Vector3Int.right,
		    Vector3Int.left,
			Vector3Int.back,
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

		Dictionary<Vector3Int, bool> _resultCache = new Dictionary<Vector3Int, bool>();

		public bool Matches(NeighborTestDelegate neighborTest, Vector3Int pos, out (GameObject, Quaternion) match)
		{
			//clear the dictionary
			_resultCache.Clear();
			
			var rotations = GetRotationsForFaceSetting(Face);

			match.Item1 = Prefab;
			match.Item2 = Quaternion.identity;
			
			//in many cases there is only one rotation; identity.
			foreach (var o in rotations)
			{
				bool valid = true;
				
				foreach(Vector3Int direction in TestDirections)
				{
					var dir = RotatedVector3Int(direction, o);
					var face = GetFaceForDir(direction);

					if (face == TileNeighbor.Any)
					{
						//still valid no matter what...
						continue;
					}
					
					//if dir is in the cache, gets set.
					if (!_resultCache.TryGetValue(dir, out bool hasNeighbor))
					{
						hasNeighbor = neighborTest(pos, dir);
						_resultCache.Add(dir, hasNeighbor);
					}

					hasNeighbor = neighborTest(pos, dir);

					//get our results.
					if (face == TileNeighbor.HasNeighbor)
					{
						if (!hasNeighbor)
						{
							//invalid test.
							valid = false;
							break;
						}
					}else if (face == TileNeighbor.NoNeighbor)
					{
						if (hasNeighbor)
						{
							//invalid test
							valid = false;
							break;
						}
					}
				}//end directions loop

				if (valid)
				{
					//match.Item1 = Prefab;
					match.Item2 = o;
					return true;
				}
			}//loop next rotation

			return false;
		}

		public TileNeighbor GetFaceForDir(Vector3Int dir)
		{
			//todo: cache a dictionary...
			//we know that these properties don't change, but we shouldn't assume that. Our cache should reset for each lookup. 
			//it would be safe to assume they don't change during a single lookup, or generation.
			//lets just do this for now, and turn it into a Cubearray or dictionary later.
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

			Debug.LogWarning($"Bad Orientation: {dir}");
			return TileNeighbor.Any;
		}

		private Vector3Int RotatedVector3Int(Vector3Int dir, Quaternion rot)
		{
			if (rot == Quaternion.identity)
			{
				return dir;
			}
			
			//there is FOR SURE a faster way to do this, if we knew what the rotation was as a ... not quaternion/euler. Like an enum.
			
			var newDir = rot * dir;
			return new Vector3Int(Mathf.RoundToInt(newDir.x), Mathf.RoundToInt(newDir.y), Mathf.RoundToInt(newDir.z));
		}
		public static Quaternion[] GetRotationsForFaceSetting(TileFaceType face)
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