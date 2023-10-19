using System;
using Cinemachine;
using Golf;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace CameraSystem
{
	//Base class for the golf cameras.
	//they are states in a state machine that is the camera system.
	public class GolfCamera : MonoBehaviour
	{
		protected GolfMovement Player => _cameraSystem.Caddy.CurrentPlayer;
		protected CinemachineVirtualCameraBase _virtualCamera;
		protected readonly int activeCameraAmount = 40;
		public int CameraPriority = 0;//we could vcam priorities, but I want a truth value we can reset to after they get modified at runtime.
		protected CameraSystem _cameraSystem;
		public bool lookAtPlayer;
		public bool followPlayer;
		
		public virtual void Init(CameraSystem system)
		{
			_cameraSystem = system;
			_virtualCamera = GetComponentInChildren<CinemachineVirtualCameraBase>();
			if (lookAtPlayer)
			{
				_virtualCamera.LookAt = Player.transform;
			}

		}
		public void SetActiveCam(bool active)
		{
			if (active)
			{
				_virtualCamera.Priority = activeCameraAmount + CameraPriority;
			}
			else
			{
				_virtualCamera.Priority = CameraPriority;
			}
		}

		protected virtual void LateUpdate()
		{
			if (followPlayer)
			{
				transform.position = Player.transform.position;
			}
		}
	}
}