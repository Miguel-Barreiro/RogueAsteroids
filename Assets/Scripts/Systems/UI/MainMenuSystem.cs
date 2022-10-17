using Configuration;
using Core;
using Events;
using Events.UI;
using UnityEngine;
using View;
using View.UI;

namespace Systems.UI
{
	public class MainMenuSystem : Core.System, ISetupSystem,  DependencyManager.IDependencyRequired
	{
		private GameStateEvent _gameStateEvent;
		private Canvas _uiCanvas;
		private UIConfig _uiConfig;
		private GameObject _mainMenuGameObject;
		private PlayButtonEvent _playButtonEvent;
		private PrefabFactory _prefabFactory;
		private EntityCycleEvent<Entities.Game> _gameCycleEvent;
		private Entities.Game _game;

		public void SetupDependencies(DependencyManager manager)
		{
			_gameStateEvent = manager.Get<GameStateEvent>();
			_uiCanvas = manager.Get<Canvas>();
			_uiConfig = manager.Get<UIConfig>();
			_playButtonEvent = manager.Get<PlayButtonEvent>();
			_prefabFactory = manager.Get<PrefabFactory>();
			_gameCycleEvent = manager.Get<EntityCycleEvent<Entities.Game>>();
		}

		public void Setup()
		{
			_gameCycleEvent.OnCreated += newGame => { _game = newGame; };
			_gameCycleEvent.OnDestroyed += game => { _game = null; };
			_gameStateEvent.OnGameLoad += OnGameLoad;
			_playButtonEvent.OnPlayButton += OnPlayButton;
			_gameStateEvent.OnGameStart += OnGameStart;
			_gameStateEvent.OnGameEnd += OnGameEnd;
		}

		private void OnGameEnd()
		{
			_mainMenuGameObject.SetActive(true);
			MainMenuView mainMenuView = _mainMenuGameObject.GetComponent<MainMenuView>();
			if (mainMenuView != null && _game != null)
				mainMenuView.ShowScore(_game.Score.Value);
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