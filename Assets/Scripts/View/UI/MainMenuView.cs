using Events.UI;
using Systems;
using UnityEngine;
using UnityEngine.UI;

namespace View.UI
{
	
	public class MainMenuView : MonoBehaviour, DependencyManager.IDependencyRequired
	{

		[SerializeField]
		private Button PlayButton;

		private PlayButtonEvent _playButtonEvent;


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
	}
}