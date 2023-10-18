using UnityEngine;

namespace CameraSystem
{
	public class PlayerCamera : GolfCamera
	{
		public override void Init(CameraSystem system)
		{
			base.Init(system);
			_virtualCamera.LookAt = Player.transform;
		}

		protected virtual void LateUpdate()
		{
			//move to player.
			transform.position = Player.transform.position;
		}
	}
}