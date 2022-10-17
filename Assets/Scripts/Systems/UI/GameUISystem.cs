using Configuration;
using Core;
using Entities;
using Events;
using Events.Collision;
using UnityEngine;
using View;
using View.UI;
using NotImplementedException = System.NotImplementedException;

namespace Systems.UI
{
	
	// in case we had a bigger game we could divide this system into multiple ones each controlling a part of the game UI
	public class GameUISystem : Core.System, ISetupSystem, DependencyManager.IDependencyRequired
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
		private ShipDamagedEvent _shipDamagedEvent;
		private GameObject _warningDamagedVFXGameObject;

		public void Setup()
		{
			_gameStateEvent.OnGameStart += OnGameStart;
			_gameStateEvent.OnGameEnd += OnGameEnd;
			
			_gameCycleEvent.OnCreated += OnNewGame;
			_shipCycleEvent.OnCreated += ship =>
			{
				_ship = ship;
				ship.Lifes.SubscribeToChanged(OnShipLifeChange);
			};
			_shipCycleEvent.OnDestroyed += ship =>
			{
				ship.Lifes.UnsubscribeFromChanged(OnShipLifeChange);
				if (_ship == ship) _ship = null;
			};
		}

		private void OnNewGame(Entities.Game newGame)
		{
			if (_game != null)
				_game.Score.UnsubscribeFromChanged(OnScoreChange);	
			_game = newGame;
		}

		private void OnShipLifeChange(int newValue)
		{
			if(_gameUI != null)
				_gameUI.Lifes.text = newValue.ToString();
		}

		private void OnScoreChange(int newValue)
		{
			if(_gameUI != null)
				_gameUI.Score.text = newValue.ToString();
		}

		private void OnGameEnd()
		{
			_prefabFactory.Destroy(_gameUIGameObject);
			_game.Score.UnsubscribeFromChanged(OnScoreChange);
			_shipDamagedEvent.OnDamaged -= OnShipDamaged;

			if (_warningDamagedVFXGameObject != null)
			{
				EndAnimationController endAnimationController = _warningDamagedVFXGameObject.GetComponent<EndAnimationController>();
				endAnimationController.OnAnimationEnd -= OnWarningLoop;
				_prefabFactory.Destroy(_warningDamagedVFXGameObject);
			}

		}

		private void OnGameStart()
		{
			_gameUIGameObject = _prefabFactory.CreateNew(_uiConfig.GameUIPrefab, _canvas.transform);
			_gameUI = _gameUIGameObject.GetComponent<GameUI>();
			
			_gameUI.Score.text = _game.Score.Value.ToString();
			_game.Score.SubscribeToChanged(OnScoreChange);

			_gameUI.Lifes.text = _ship.Lifes.Value.ToString();
			_ship.Lifes.SubscribeToChanged(OnShipLifeChange);

			_shipDamagedEvent.OnDamaged += OnShipDamaged;
		}

		private void OnShipDamaged(int deltaLife)
		{
			if (_warningDamagedVFXGameObject == null)
			{
				_warningDamagedVFXGameObject = _prefabFactory.CreateNew(_uiConfig.DamageWarningVFXPrefab, _canvas.transform);
				EndAnimationController endAnimationController = _warningDamagedVFXGameObject.GetComponent<EndAnimationController>();
				endAnimationController.OnAnimationEnd += OnWarningLoop;
			}
		}

		private void OnWarningLoop(AnimationType animationtype)
		{
			EndAnimationController endAnimationController = _warningDamagedVFXGameObject.GetComponent<EndAnimationController>();
			endAnimationController.OnAnimationEnd -= OnWarningLoop;
			_prefabFactory.Destroy(_warningDamagedVFXGameObject);
			_warningDamagedVFXGameObject = null;
		}

		public void SetupDependencies(DependencyManager manager)
		{
			_prefabFactory = manager.Get<PrefabFactory>();
			_uiConfig = manager.Get<UIConfig>();
			_canvas = manager.Get<Canvas>();
			_gameStateEvent = manager.Get<GameStateEvent>();
			_gameCycleEvent = manager.Get<EntityCycleEvent<Entities.Game>>();
			_shipCycleEvent = manager.Get<EntityCycleEvent<Ship>>();
			_shipDamagedEvent = manager.Get<ShipDamagedEvent>();
		}
	}
}