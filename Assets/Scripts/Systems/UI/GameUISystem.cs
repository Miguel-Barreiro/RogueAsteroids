using Configuration;
using Entities;
using Events;
using Systems.Core;
using UnityEngine;
using View;
using View.UI;
using NotImplementedException = System.NotImplementedException;

namespace Systems.UI
{
	
	// in case we had a bigger game we could divide this system into multiple ones each controlling a part of the game UI
	public class GameUISystem : System, ISetupSystem, DependencyManager.IDependencyRequired
	{
		private PrefabFactory _prefabFactory;
		private UIConfig _uiConfig;
		private Canvas _canvas;
		private GameStateEvent _gameStateEvent;
		private GameObject _gameUIGameObject;
		private GameUI _gameUI;
		private EntityCycleEvent<Entities.Game> _gameCycleEvent;
		private Entities.Game _game;
		private EntityCycleEvent<Ship> _shipCycleEvent;
		private Ship _ship;

		public void Setup()
		{
			_gameStateEvent.OnGameStart += OnGameStart;
			_gameStateEvent.OnGameEnd += OnGameEnd;
			
			_gameCycleEvent.OnCreated += OnNewGame;
			_shipCycleEvent.OnCreated += OnNewShip;
		}

		private void OnNewShip(Ship newShip)
		{
			if (_ship != null)
				_ship.Lifes.UnsubscribeFromChanged(OnLifesChange);	
			_ship = newShip;
		}


		private void OnNewGame(Entities.Game newGame)
		{
			if (_game != null)
				_game.Score.UnsubscribeFromChanged(OnScoreChange);	
			_game = newGame;
		}

		private void OnLifesChange(int newValue)
		{
			_gameUI.Lifes.text = newValue.ToString();
		}

		private void OnScoreChange(int newValue)
		{
			_gameUI.Score.text = newValue.ToString();
		}

		private void OnGameEnd()
		{
			_prefabFactory.Destroy(_gameUIGameObject);
			_game.Score.UnsubscribeFromChanged(OnScoreChange);
		}

		private void OnGameStart()
		{
			_gameUIGameObject = _prefabFactory.CreateNew(_uiConfig.GameUIPrefab, _canvas.transform);
			_gameUI = _gameUIGameObject.GetComponent<GameUI>();
			
			_gameUI.Score.text = _game.Score.Value.ToString();
			_game.Score.SubscribeToChanged(OnScoreChange);

			_gameUI.Lifes.text = _ship.Lifes.Value.ToString();
			_ship.Lifes.SubscribeToChanged(OnLifesChange);
		}

		public void SetupDependencies(DependencyManager manager)
		{
			_prefabFactory = manager.Get<PrefabFactory>();
			_uiConfig = manager.Get<UIConfig>();
			_canvas = manager.Get<Canvas>();
			_gameStateEvent = manager.Get<GameStateEvent>();
			_gameCycleEvent = manager.Get<EntityCycleEvent<Entities.Game>>();
			_shipCycleEvent = manager.Get<EntityCycleEvent<Ship>>();
		}
	}
}