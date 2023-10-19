using System;
using Cinemachine;
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
		//[ReadOnly, SerializeField] private GolfCamera[] _cameras;

		[SerializeField] private CinemachineVirtualCameraBase _aimCamera;

		[SerializeField] private CinemachineVirtualCameraBase _inFlight;

		private CinemachineMixingCamera _mixingCam;

		public virtual void Start()
		{
			_mixingCam = GetComponent<CinemachineMixingCamera>();
		}

		void Update()
		{
			//hacky listener pattern until we start subscribing to invents.
			
			//set aim 
			if (_caddy.CurrentStroke.Status == StrokeStatus.Aiming)
			{
				_mixingCam.SetWeight(_aimCamera,100);
				
			}
			else
			{
				_mixingCam.SetWeight(_aimCamera, 0);
			}
			
			//set flight
			if (_caddy.CurrentStroke.Status == StrokeStatus.InMotion)
			{
				_mixingCam.SetWeight(_inFlight, 1);
			}
			else
			{
				_mixingCam.SetWeight(_inFlight, 0);
			}
		}
	}
}