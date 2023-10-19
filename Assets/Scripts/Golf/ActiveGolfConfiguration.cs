using System;
using System.Collections.Generic;
using CameraSystem;
using UnityEngine;
using Utilities.ReadOnlyAttribute;

namespace Golf
{
	[CreateAssetMenu(fileName = "Caddy", menuName = "Golf/Caddy", order = 0)]
	public class ActiveGolfConfiguration : ScriptableObject
	{
		[SerializeField] private Scorecard _scorecard;

		public GolfMovement CurrentPlayer => _currentPlayer;
		private GolfMovement _currentPlayer;

		public Stroke CurrentStroke => _currentStroke;

		[SerializeField] [ReadOnly]
		private Stroke _currentStroke;
		
		public Action<Club> OnSelectedClubChanged;

		[SerializeField] private List<Club> clubs;
		
		//we don't actually serialize this to edit it, but this lets us preview in the inspector for debugging.
		[SerializeField,ReadOnly]
		private int selectedClubIndex = 0;
		public Club SelectedClub => clubs[selectedClubIndex];

		public void SetCurrentPlayer(GolfMovement player)
		{
			_currentPlayer = player;
		}

		public void StartNewStrokeAndAim(Rigidbody ball)
		{
			
			//take previous stroke and add it to the score card.
			if (_currentStroke != null)
			{
				_scorecard.ScoreStroke(_currentStroke);
			}
			
			_currentStroke = new Stroke(ball, SelectedClub);
			_currentStroke.Status = StrokeStatus.Aiming;
			//OnNewStroke?.Invoke();
		}
		public void CycleClub(int delta = 1)
		{
			selectedClubIndex+=delta;
			if (selectedClubIndex >= clubs.Count)
			{
				selectedClubIndex = 0;
			}

			if (selectedClubIndex < 0)
			{
				selectedClubIndex = clubs.Count - 1;
			}
			
			//alert anyone who cares that the selected club has changed.
			OnSelectedClubChanged?.Invoke(SelectedClub);
		}
	}
}