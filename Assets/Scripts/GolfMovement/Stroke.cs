using UnityEngine;
using UnityEngine.Serialization;

namespace Golf
{
	[System.Serializable]
	public class Stroke
	{
		//todo: Capitalize
		public Club club;
		public StrokeStatus Status;
		public Vector3 startPosition;
		public Vector3 endPosition;
		public Vector3 AimDir;
		public float inputPower;
		public float hitTimer;
		private float timeNotMoving;
		public Vector3 RealAimDir => GetRealAimDir();

		public Rigidbody BallRB => _ball;
		private Rigidbody _ball;
		public Stroke(Rigidbody ball, Club club)
		{
			_ball = ball;
			this.club = club;
			Status = StrokeStatus.NotTaken;
			this.startPosition = ball.position;
			AimDir = ball.transform.forward;
			inputPower = 0;
		}

		public void Tick(float delta)
		{
			hitTimer += delta;
			if (_ball.velocity.magnitude < 0.005f)
			{
				timeNotMoving += delta;
			}
			else
			{
				timeNotMoving = 0;
			}
		}
		private Vector3 GetRealAimDir()
		{
			return Vector3.RotateTowards(AimDir, Vector3.up, club.angle * Mathf.Deg2Rad, Mathf.Infinity).normalized;
		}

		public Vector3 GetForce()
		{
			Vector3 vel = GetRealAimDir() * (inputPower * club.power);
			return vel;
		}

		public void Complete()
		{
			endPosition = _ball.position;
			Status = StrokeStatus.Taken;
		}

		public void Failure()
		{
			endPosition = startPosition;//To tally the total distance, this stroke accomplished 0.
			//record trap type?
			Status = StrokeStatus.Failure;
		}

		/// <summary>
		/// Has the ball stopped moving, and remained still for "long enough".
		/// </summary>
		public bool IsStrokeComplete()
		{
			//todo: manage magic variables. Or at least readonly static.
			return hitTimer > 0.75f && _ball.velocity.sqrMagnitude < 0.02f && timeNotMoving > 0.15f;
		}

		/// <summary>
		/// Moves the ball to the start position, and clears it's velocity. Called after the ball goes into a trap.
		/// </summary>
		public void ResetStrokeToStart()
		{
			_ball.position = startPosition;
			_ball.velocity = Vector3.zero;
			_ball.angularVelocity = Vector3.zero;
		}

		public float TravelDistance()
		{
			return Vector3.Distance(startPosition, endPosition);
		}
	}
}