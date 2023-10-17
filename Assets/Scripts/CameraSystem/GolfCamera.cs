using Cinemachine;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace CameraSystem
{
	//Base class for the golf cameras.
	//they are states in a state machine that is the camera system.
	public class GolfCamera : MonoBehaviour
	{
		protected CinemachineVirtualCameraBase _virtualCamera;
		protected readonly int activeCameraAmount = 40;
		public int CameraPriority = 0;//we could vcam priorities, but I want a truth value we can reset to after they get modified at runtime.
		protected CameraSystem _cameraSystem;
		
		public virtual void Init(CameraSystem system)
		{
			_cameraSystem = system;
		}
		public void SetActiveCam(bool active)
		{
			if (_virtualCamera == null)
			{
				//lazy init... it's not that lazy.
				_virtualCamera = GetComponentInChildren<CinemachineVirtualCameraBase>();
			}
			if (active)
			{
				_virtualCamera.Priority = activeCameraAmount + CameraPriority;
			}
			else
			{
				_virtualCamera.Priority = CameraPriority;
			}
		}
	}
}