using Configuration;
using Core;
using Entities;
using Events;
using UnityEngine;
using View;

namespace Systems.Game
{
	public class GameBackgroundSystem : Core.System, ISetupSystem, IExecuteSystem, DependencyManager.IDependencyRequired
	{
		private GameStateEvent _gameStateEvent;
		private LevelConfiguration _levelConfiguration;
		private EntityCycleEvent<Ship> _shipCycleEvent;
		private Ship _ship;
		private Camera _camera;
		private PrefabFactory _prefabFactory;
		private GameObject _gameBackground;

		public void Execute(float elapsedTime)
		{
			if (_ship != null && _gameBackground != null)
			{
				SpriteRenderer background = _gameBackground.GetComponent<SpriteRenderer>();
				Transform shipTransform = _ship.View.GameObject.Value.transform;

				Vector3 position = shipTransform.position;
				Bounds spriteBounds = background.sprite.bounds;
				
				float newX = position.x % spriteBounds.size.x;
				float newY = position.y % spriteBounds.size.y;
				Vector3 newPosition = new Vector3( -newX , -newY,  20 );
				background.transform.localPosition = newPosition;
			}
		}

		public void Setup()
		{
			_shipCycleEvent.OnCreated += OnNewShip;
			_shipCycleEvent.OnDestroyed += OnShipDestroyed;
			_gameStateEvent.OnGameStart += OnGameStart;
			_gameStateEvent.OnGameEnd += OnGameEnd;
		}


		private void OnGameEnd()
		{
			if (_gameBackground != null)
				_prefabFactory.Destroy(_gameBackground);
			
			_gameBackground = null;
		}

		private void OnGameStart()
		{
			_gameBackground = _prefabFactory.CreateNew(_levelConfiguration.BackgroundPrefab, _camera.transform);
			_gameBackground.transform.localPosition = Vector3.zero;
			
		}

		private void OnShipDestroyed(Ship ship)
		{
			if (_ship == ship)
			{
				_ship = null;
			}
		}
		private void OnNewShip(Ship newShip)
		{
			_ship = newShip;
		}

		public void SetupDependencies(DependencyManager manager)
		{
			_gameStateEvent = manager.Get<GameStateEvent>();
			_levelConfiguration = manager.Get<LevelConfiguration>();
			_shipCycleEvent = manager.Get<EntityCycleEvent<Ship>>();
			_prefabFactory = manager.Get<PrefabFactory>();
			_camera = manager.Get<Camera>();
		}
	}
}