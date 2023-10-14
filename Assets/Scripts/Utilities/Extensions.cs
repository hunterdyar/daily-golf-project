using UnityEngine;
using UnityEngine.UIElements;

namespace Utilities
{
	public static class Extensions
	{
		public static void CopyPropertiesFrom(this Rigidbody rb, Rigidbody rigidbody)
		{
			if (rigidbody == null)
			{
				Debug.LogWarning("Can't copy properties from null rigidbody.",rb);
				return;
			}
			rb.mass = rigidbody.mass;
			rb.drag = rigidbody.drag;
			rb.angularDrag = rigidbody.angularDrag;
			rb.constraints = rigidbody.constraints;
			rb.centerOfMass = rigidbody.centerOfMass;
			rb.excludeLayers = rigidbody.excludeLayers;
			rb.freezeRotation = rigidbody.freezeRotation;
			rb.useGravity = rigidbody.useGravity;
			rb.interpolation = rigidbody.interpolation;
			rb.isKinematic = rigidbody.isKinematic;
			rb.solverIterations = rigidbody.solverIterations;
			rb.solverVelocityIterations = rigidbody.solverVelocityIterations;
			rb.sleepThreshold = rigidbody.sleepThreshold;
			rb.detectCollisions = rigidbody.detectCollisions;
			rb.collisionDetectionMode = rigidbody.collisionDetectionMode;
		}
	}
}