using System;
using UnityEngine;

namespace Golf
{
	public class GolfHitPreviewLine : MonoBehaviour
	{
		private GolfMovement _golfMovement;
		private LineRenderer _lineRenderer;
		private void Awake()
		{
			_lineRenderer = GetComponent<LineRenderer>();
			_golfMovement = GetComponentInParent<GolfMovement>();
		}

		private void Start()
		{
			if (_golfMovement == null)
			{
				Debug.LogWarning("Golf Hit Preview Line must be on or child of GolfMovement.",gameObject);
				gameObject.SetActive(false);
			}
		}

		private void Update()
		{
			_lineRenderer.enabled = _golfMovement.IsAiming;

			if (_golfMovement.IsAiming)
			{
				_lineRenderer.SetPosition(0, _golfMovement.CurrentStroke.startPosition);

				//temp
				_lineRenderer.SetPosition(1,
					_golfMovement.CurrentStroke.startPosition + _golfMovement.CurrentStroke.RealAimDir);

			}
		}
	}
}