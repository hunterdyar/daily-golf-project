using System;
using Golf;
using UnityEngine;
using Utilities.ReadOnlyAttribute;

namespace CameraSystem
{
	public class CameraSystem : MonoBehaviour
	{

		public ActiveGolfConfiguration Caddy => _caddy;
		[Header("Active Config")] [SerializeField]
		private ActiveGolfConfiguration _caddy;
		[Header("Camera Config")]
		[ReadOnly, SerializeField] private GolfCamera[] _cameras;

		[SerializeField] private GolfCamera _aimCamera;
		[SerializeField] private GolfCamera _inFlightCamera;

		private GolfCamera ActiveCamera;
		public virtual void Start()
		{
			FindAndInitializeCameras();
		}

		public void FindAndInitializeCameras()
		{
			_cameras = transform.GetComponentsInChildren<GolfCamera>();
			//disable all cameras. Enable the one with the highest default priority.
			GolfCamera highestPriorityCam = null;
			int priority = Int32.MinValue;
			foreach (var camera in _cameras)
			{
				camera.Init(this);
				camera.SetActiveCam(false);

				if (camera is AimCamera aimCamera)
				{
					_aimCamera = aimCamera;
				}
				
				if (camera.CameraPriority > priority)
				{
					priority = camera.CameraPriority;
					highestPriorityCam = camera;
				}
			}
			highestPriorityCam.SetActiveCam(true);
			ActiveCamera = highestPriorityCam;
		}

		private void SetActiveCamera(GolfCamera camera)
		{
			if (ActiveCamera == camera)
			{
				return;
			}

			if (ActiveCamera != null)
			{
				ActiveCamera.SetActiveCam(false);
			}

			ActiveCamera = camera;
			camera.SetActiveCam(true);
		}

		void Update()
		{
			//hacky listener pattern until we start subscribing to invents.
			if (_caddy.CurrentStroke.Status == StrokeStatus.Aiming)
			{
				if (ActiveCamera != _aimCamera)
				{
					SetActiveCamera(_aimCamera);
				}
			}else if (_caddy.CurrentStroke.Status == StrokeStatus.InMotion)
			{
				if (ActiveCamera != _inFlightCamera)
				{
					SetActiveCamera(_inFlightCamera);
				}
			}
		}
	}
}