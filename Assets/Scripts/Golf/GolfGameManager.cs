using System;
using MapGen;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Golf
{
	public class GolfGameManager : MonoBehaviour
	{
		[SerializeField] ActiveGolfConfiguration _caddy;
		private void OnEnable()
		{
			MapGenerator.OnGenerationComplete += StartNewGolfGame;
		}

		private void OnDisable()
		{
			MapGenerator.OnGenerationComplete -= StartNewGolfGame;
		}

		private void StartNewGolfGame(MapGenerator obj)
		{
			_caddy.StartNewGame();
		}
		
	}
}