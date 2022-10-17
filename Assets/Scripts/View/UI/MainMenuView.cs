using Core;
using Entities;
using Events;
using Events.UI;
using Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View.UI
{
	
	public class MainMenuView : MonoBehaviour, DependencyManager.IDependencyRequired
	{

		[SerializeField]
		private Button PlayButton;

		[SerializeField]
		private TMP_Text Score;

		[SerializeField]
		private GameObject ScoreGroup;

		private PlayButtonEvent _playButtonEvent;
		private GameStateEvent _gameStateEvent;


		private void Awake()
		{
			PlayButton.onClick.AddListener(OnPlayClick);
		}

		private void OnPlayClick()
		{
			_playButtonEvent.TriggerPlayButton();
		}

		private void OnDestroy()
		{
			PlayButton.onClick.RemoveListener(OnPlayClick);
		}

		public void SetupDependencies(DependencyManager manager)
		{
			_playButtonEvent = manager.Get<PlayButtonEvent>();
		}

		public void ShowScore(int score)
		{
			ScoreGroup.SetActive(true);
			Score.text = score.ToString();
		}
	}
}