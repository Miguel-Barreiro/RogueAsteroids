using Configuration;
using Core;
using Entities;
using Events;
using UnityEngine;
using View;

namespace Systems.Game
{
	public class PlayerSpawnSystem : System, ISetupSystem, DependencyManager.IDependencyRequired
	{
		private GameStateEvent _gameStateEvent;
		private EntityFactory<Ship> _shipFactory;
		private Ship _ship;
		private ShipConfig _shipConfig;
		private GameObject _shipGameObject;
		private PrefabFactory _prefabFactory;

		public void SetupDependencies(DependencyManager manager)
		{
			_gameStateEvent = manager.Get<GameStateEvent>();
			_shipFactory = manager.Get<EntityFactory<Ship>>();
			_shipConfig = manager.Get<ShipConfig>();
			_prefabFactory = manager.Get<PrefabFactory>();
		}

		public void Setup()
		{
			_gameStateEvent.OnGameStart += OnGameStart;
			_gameStateEvent.OnGameEnd += OnGameEnd;
		}

		private void OnGameEnd()
		{
			if (_ship != null)
				_shipFactory.DestroyEntity(_ship);
		}

		private void OnGameStart()
		{
			GameObject shipGameObject = _prefabFactory.CreateNew(_shipConfig.ShipPrefab, null);
			_ship = _shipFactory.CreateNew(newShip =>
			{
				newShip.Lifes.Value = _shipConfig.StartingLifes;
				newShip.View.GameObject.Value = shipGameObject;
				newShip.PhysicalBody.BodyView.Value = shipGameObject.GetComponent<PhysicsBodyView>();
				newShip.PhysicalBody.BreakSpeed = _shipConfig.BreakingSpeed * 10f;
				newShip.PhysicalBody.TurnSpeed = _shipConfig.TurningSpeed * 10f;
			});
		}
	}
}