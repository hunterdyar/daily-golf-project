using System;
using Cinemachine;
using Golf;
using TMPro;
using UnityEngine;

namespace CameraSystem
{
	public class AimCameraControl : MonoBehaviour, AxisState.IInputAxisProvider
	{
		//[SerializeField] InputReader _inputReader;
		[SerializeField] private ActiveGolfConfiguration _caddy;
		private CinemachinePOV _pov;
		private CinemachineCameraOffset _cameraOffset;
		
		private void Awake()
		{ 
			var virtualCamera = GetComponent<CinemachineVirtualCamera>();
			_pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
			_cameraOffset = virtualCamera.GetComponent<CinemachineCameraOffset>();
		}

		private void Start()
		{
			_pov.VirtualCamera.Follow = _caddy.CurrentPlayer.transform;
			_pov.VirtualCamera.LookAt = _caddy.CurrentPlayer.transform;
		}

		protected void LateUpdate()
		{
			// if (_caddy.CurrentPlayer.transform.position != null)
			// {
			// 	transform.position = _caddy.CurrentPlayer.transform.position;
			// }
			// transform.Rotate(Vector3.up,_inputReader.Look.x); 

			var aim = _caddy.CurrentStroke.aimDir;
			_pov.m_HorizontalAxis.Value = Quaternion.LookRotation(aim).eulerAngles.y;

			var height = _cameraOffset.m_Offset.z;
			//height?
			// _caddy.CurrentStroke.inputPower;
			
			_cameraOffset.m_Offset = new Vector3(_cameraOffset.m_Offset.x, _cameraOffset.m_Offset.y, _cameraOffset.m_Offset.z);
		}

		//Axis index ranges from 0...2 for X, Y, and Z.

		public float GetAxisValue(int axis)
		{
			if (enabled)
			{
				// if (axis < 2)
				// {
				// 	return Vector3.Angle(Vector3.forward, _caddy.CurrentStroke.aimDir);
				// }
				// else
				// {
				// 	return 0;
				// }
			}

			return 0;
		}
	}
}