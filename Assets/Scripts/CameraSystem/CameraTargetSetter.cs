using System;
using Cinemachine;
using Golf;
using UnityEngine;

namespace CameraSystem
{
	public class CameraTargetSetter : MonoBehaviour
	{
		public ActiveGolfConfiguration Caddy;
		public bool SetFollow = true;
		public bool SetLookAt = true;

		public void Start()
		{
			CinemachineVirtualCamera _camera;
			_camera = GetComponent<CinemachineVirtualCamera>();
			if (SetLookAt)
			{
				_camera.LookAt = Caddy.CurrentPlayer.transform;
			}

			if (SetFollow)
			{
				_camera.Follow = Caddy.CurrentPlayer.transform;
			}
		}
	}
}