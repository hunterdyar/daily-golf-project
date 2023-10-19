using System.Collections.Generic;
using UnityEngine;
using Utilities.ReadOnlyAttribute;

namespace Golf
{
	[CreateAssetMenu(fileName = "Scorecard", menuName = "Golf/Scorecard", order = 0)]
	public class Scorecard : ScriptableObject
	{
		public int StrokeCount => _strokes.Count;

	[SerializeField, ReadOnly]
		private List<Stroke> _strokes = new List<Stroke>();

		public void ResetScorecard()
		{
			_strokes.Clear();
		}

		public void ScoreStroke(Stroke stroke)
		{
			if (stroke.Status == StrokeStatus.NotTaken)
			{
				Debug.LogWarning("Shouldn't add stroke that is not taken.");
				return;
			}

			if (stroke.Status == StrokeStatus.Aiming)
			{
				Debug.LogWarning("Shouldn't add stroke that is still aiming?");
				return;
			}
			_strokes.Add(stroke);
		}

		public float GetTotalDistance()
		{
			float distance = 0;
			for (int i = 0; i < _strokes.Count; i++)
			{
				distance += _strokes[i].TravelDistance();
			}

			return distance;
		}

	}
}