using System;
using Golf;
using UnityEngine;

namespace CameraSystem
{
	public class AimCamera : GolfCamera
	{
		private GolfMovement Player => Caddy.CurrentPlayer;
		public InputReader _InputReader;

		
		public ActiveGolfConfiguration Caddy;


		public override void Init(CameraSystem system)
		{
			base.Init(system);
			_virtualCamera.LookAt = Player.transform;
		}
		
		private void LateUpdate()
		{
			//move to player.
			transform.position = Player.transform.position;
			
			transform.Rotate(Vector3.up,_InputReader.Look.x);
		}
	}
}