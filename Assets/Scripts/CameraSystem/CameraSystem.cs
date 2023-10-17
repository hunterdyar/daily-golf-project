using System;
using UnityEngine;
using Utilities.ReadOnlyAttribute;

namespace CameraSystem
{
	public class CameraSystem : MonoBehaviour
	{
		[ReadOnly, SerializeField] private GolfCamera[] _cameras;
		
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
				camera.SetActiveCam(false);
				if (camera.CameraPriority > priority)
				{
					priority = camera.CameraPriority;
					highestPriorityCam = camera;
				}
			}
			highestPriorityCam.SetActiveCam(true);
		}

		void Update()
		{
			//listen to the input/game states and 
		}
	}
}