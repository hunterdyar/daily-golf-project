using Cinemachine;
using Golf;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace CameraSystem
{
	public class CameraOrbitInputProvider : MonoBehaviour, Cinemachine.AxisState.IInputAxisProvider
	{
		[SerializeField] private InputReader _input;
		
		public float GetAxisValue(int axis)
		{
			return _input.Look.x;
		}
	}
}