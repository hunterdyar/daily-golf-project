using System;
using Cinemachine;
using Golf;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Serialization;
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

		[SerializeField] private CinemachineVirtualCamera _aimCamera;

		[SerializeField] private CinemachineVirtualCamera _inFlight;

		private CinemachineMixingCamera _mixingCam;
		[SerializeField] private float _aimFlightBlendTime = 0.25f;
		[SerializeField] private AnimationCurve _aimFlightBlendCurve;
		private float _aimFlightBlend;

		public virtual void Start()
		{
			_mixingCam = GetComponent<CinemachineMixingCamera>();
		}

		void Update()
		{
			//hacky listener pattern until we start subscribing to invents.
			WeightsTick();
		}

		private void WeightsTick()
		{
			int sign = 0;
			if (_caddy.CurrentStroke.Status == StrokeStatus.Aiming)
			{
				sign = -1;
			}
			else if (_caddy.CurrentStroke.Status == StrokeStatus.InMotion)
			{
				sign = 1;
			}

			_aimFlightBlend = Mathf.Clamp01(_aimFlightBlend + sign * Time.deltaTime / _aimFlightBlendTime);
			float t = _aimFlightBlendCurve.Evaluate(_aimFlightBlend);
			_mixingCam.SetWeight(_aimCamera, Mathf.Lerp(50, 0, t));
			_mixingCam.SetWeight(_inFlight, Mathf.Lerp(0, 50, t));

		}
	}
}