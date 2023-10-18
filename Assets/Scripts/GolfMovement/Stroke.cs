using UnityEngine;

namespace Golf
{
	[System.Serializable]
	public class Stroke
	{
		public Club club;
		public StrokeStatus Status;
		public Vector3 startPosition;
		public Vector3 endPosition;
		public Vector3 aimDir;
		public float inputPower;
		public float hitTimer;
		public Vector3 RealAimDir => GetRealAimDir();
		//preview data.
		//state

		public Rigidbody BallRB => _ball;
		private Rigidbody _ball;
		public Stroke(Rigidbody ball, Club club)
		{
			_ball = ball;
			this.club = club;
			Status = StrokeStatus.NotTaken;
			this.startPosition = ball.position;
			aimDir = ball.transform.forward;
			inputPower = 0;
		}

		public void Tick(float delta)
		{
			hitTimer += delta;
		}
		private Vector3 GetRealAimDir()
		{
			return Vector3.RotateTowards(aimDir, Vector3.up, club.angle * Mathf.Deg2Rad, Mathf.Infinity).normalized;
		}

		public Vector3 GetForce()
		{
			Vector3 vel = GetRealAimDir() * (inputPower * club.power);
			return vel;
			//return (vel / Time.fixedDeltaTime) * _ball.mass;
			//return (GetRealAimDir() * inputPower / rigidbody.mass) / Time.fixedDeltaTime;

			// v = (f / m) * dt;
			// v / dt = f / m;
			// (v/dt)*m
		}

		public void Complete()
		{
			endPosition = _ball.position;
			Status = StrokeStatus.Taken;
		}

		public void Failure()
		{
			endPosition = startPosition;//i guess.
			//record trap type.
			Status = StrokeStatus.Failure;
		}
	}
}