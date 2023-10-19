using System;
using Golf;
using UnityEngine;

namespace CameraSystem
{
	public class AimCamera : GolfCamera
	{
		public InputReader _InputReader;
		public Transform LookCam => _virtualCamera.transform;
		
		
		protected override void LateUpdate()
		{
			base.LateUpdate();
			
			transform.Rotate(Vector3.up,_InputReader.Look.x);
			LookCam.Translate(Vector3.up * _InputReader.Look.y*Time.deltaTime);
		}
	}
}