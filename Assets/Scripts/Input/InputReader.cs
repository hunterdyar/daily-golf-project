using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities.ReadOnlyAttribute;

namespace Golf
{
	[CreateAssetMenu(fileName = "Input Reader", menuName = "Golf/Input/Input Reader", order = 0)]
	public class InputReader : ScriptableObject, GolfControls.IGameplayActions
	{
		private GolfControls _golfControls;

		public Action Swing;
		public Action CycleClubRight;
		public Action CycleClubLeft;
		
		
		public Vector2 Look => _look;
		[Header("Debug")]
		[SerializeField, ReadOnly] private Vector2 _look;

		public float Aim => _aim;
		[SerializeField, ReadOnly] private float _aim;

		public float PowerDelta => _powerDelta;
		[SerializeField, ReadOnly] private float _powerDelta;

		[Header("Input Settings")] [SerializeField]
		private float _aimSpeed;
		private void OnEnable()
		{
			_golfControls = new GolfControls();
			_golfControls.Gameplay.Enable();
			_golfControls.Gameplay.SetCallbacks(this);
		}

		//overload to pass default rotatearound axis up in. 
		public void AdjustAimVectorTick(ref Vector3 aim)
		{
			AdjustAimVectorTick(ref aim, Vector3.up);
		}

		/// <summary>
		/// Rotates a given vector by the aim input. Uses Time.deltaTime so should be called from Update.
		/// </summary>
		public void AdjustAimVectorTick(ref Vector3 aim, Vector3 up)
		{
			//rotating vectors is hard. Trig? bleh. Lets use quaternions, which are convenient for rotations.
			//(quaternion * vector) will return a vector that has been rotated by that quaternion. neat!
			// first we make a quaternion of just the little change in aim (delta)
			// then we use that above trick to rotate the vector. it's a ref property so we are modifying the original.
			aim = Quaternion.AngleAxis(Aim * Time.deltaTime * _aimSpeed, up) * aim;
		}

		public void OnAim(InputAction.CallbackContext context)
		{
			_aim = context.ReadValue<float>();
		}

		public void OnPower(InputAction.CallbackContext context)
		{
			_powerDelta = context.ReadValue<float>();
		}

		public void OnSwing(InputAction.CallbackContext context)
		{
			Swing?.Invoke();
		}

		public void OnLook(InputAction.CallbackContext context)
		{
			_look = context.ReadValue<Vector2>();
		}

		public void OnCycleClubRight(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				CycleClubRight?.Invoke();
			}
		}

		public void OnCycleClubLeft(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				CycleClubLeft?.Invoke();
			}
		}
	}
}