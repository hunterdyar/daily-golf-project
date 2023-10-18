using System;
using UnityEditor;
using UnityEditor.Rendering;
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
		public Action<int>CycleClub;
		public Action OnAnyPropertyUpdated;
		public Vector2 Look => _look;
		[SerializeField]
		private Vector2 _look;

		public float Aim => _aim;
		[SerializeField]
		private float _aim;

		public float PowerDelta => _powerDelta;
		[SerializeField]
		private float _powerDelta;

		[SerializeField] private float _aimSpeed;

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
			aim = Vector3.ProjectOnPlane(aim, Vector3.up).normalized;
			aim = Quaternion.AngleAxis(Aim * Time.deltaTime * _aimSpeed, up) * aim;
		}

		public void OnAim(InputAction.CallbackContext context)
		{
			float aim = context.ReadValue<float>();
			if (_aim != aim)
			{
				_aim = aim;
				OnAnyPropertyUpdated?.Invoke();
			}
		}

		public void OnPower(InputAction.CallbackContext context)
		{
			_powerDelta = context.ReadValue<float>();
		}

		public void OnSwing(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				DoSwing();
			}
		}

		public void OnLook(InputAction.CallbackContext context)
		{
			_look = context.ReadValue<Vector2>();
		}

		public void OnCycleClubRight(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				DoCycleClubRight();
			}
		}

		public void OnCycleClubLeft(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				DoCycleClubLeft();
			}
		}

		//wrapper so we can call this for fake-input events in the inspector, tutorials, etc.
		public void DoSwing()
		{
			Swing?.Invoke();
		}

		public void DoCycleClubRight()
		{
			CycleClub?.Invoke(1);
		}

		public void DoCycleClubLeft()
		{
			CycleClub?.Invoke(-1);
		}
}
}