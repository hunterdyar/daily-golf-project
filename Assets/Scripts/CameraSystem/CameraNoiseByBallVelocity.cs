using System;
using Cinemachine;
using Golf;
using UnityEngine;

namespace CameraSystem
{
	public class CameraNoiseByBallVelocity : MonoBehaviour
	{
		private CinemachineVirtualCamera _camera;
		[SerializeField] private ActiveGolfConfiguration _caddy;
		private CinemachineBasicMultiChannelPerlin _perlin;
		
		
		private void Awake()
		{
			_camera = GetComponent<CinemachineVirtualCamera>();
			_perlin = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		}
		

		private void Update()
		{
			_perlin.m_AmplitudeGain = Mathf.Clamp01(_caddy.CurrentPlayer.Velocity.sqrMagnitude/3f -0.1f)*1.5f;
		}
	}
}