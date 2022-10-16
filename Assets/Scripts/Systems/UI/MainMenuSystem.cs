using Configuration;
using Core;
using Events;
using Events.UI;
using UnityEngine;
using View;

namespace Systems.UI
{
	public class MainMenuSystem : System, ISetupSystem,  DependencyManager.IDependencyRequired
	{
		private GameStateEvent _gameStateEvent;
		private Canvas _uiCanvas;
		private UIConfig _uiConfig;
		private GameObject _mainMenuGameObject;
		private PlayButtonEvent _playButtonEvent;
		private PrefabFactory _prefabFactory;

		public void SetupDependencies(DependencyManager manager)
		{
			_gameStateEvent = manager.Get<GameStateEvent>();
			_uiCanvas = manager.Get<Canvas>();
			_uiConfig = manager.Get<UIConfig>();
			_playButtonEvent = manager.Get<PlayButtonEvent>();
			_prefabFactory = manager.Get<PrefabFactory>();
		}

		public void Setup()
		{
			_gameStateEvent.OnGameLoad += OnGameLoad;
			_playButtonEvent.OnPlayButton += OnPlayButton;
			_gameStateEvent.OnGameStart += OnGameStart;
		}

		private void OnGameStart()
		{
			_mainMenuGameObject.SetActive(false);
		}

		private void OnPlayButton()
		{
			
		}

		private void OnGameLoad()
		{
			if (_mainMenuGameObject == null)
				_mainMenuGameObject = _prefabFactory.CreateNew(_uiConfig.MainMenuPrefab, _uiCanvas.transform); 
			
			_mainMenuGameObject.SetActive(true);
		}
	}
}