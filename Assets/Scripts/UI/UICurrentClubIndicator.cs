using System;
using Golf;
using TMPro;
using UnityEngine;

namespace UI
{
	public class UICurrentClubIndicator : MonoBehaviour
	{
		[SerializeField]
		private ActiveGolfConfiguration _caddy;

		private TMP_Text _text;//just text for now

		private void Awake()
		{
			_text = GetComponent<TMP_Text>();
		}

		private void Start()
		{
			//refresh on start
			OnSelectedClubChanged(_caddy.SelectedClub);
		}

		private void OnEnable()
		{
			_caddy.OnSelectedClubChanged += OnSelectedClubChanged;
		}
		private void OnDisable()
		{
			_caddy.OnSelectedClubChanged -= OnSelectedClubChanged;
		}

		private void OnSelectedClubChanged(Club club)
		{
			//text is temp, this will be an icon in the future.
			_text.text = "Club: " + club.displayName;
		}
	}
}