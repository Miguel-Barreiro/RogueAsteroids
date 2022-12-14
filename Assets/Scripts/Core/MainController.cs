using System.Collections.Generic;
using Entities;
using Events;
using Systems;
using Systems.Game;
using Systems.Game.Collisions;
using Systems.Input;
using Systems.UI;
using UnityEngine;

namespace Core
{
	public class MainController : MonoBehaviour, DependencyManager.IDependencyRequired
	{
		[SerializeField]
		private DependencyManager DependencyManager;


		private readonly List<System> _systems = new List<System>();
		private readonly List<IExecuteSystem> _executeSystems = new List<IExecuteSystem>();
		private GameStateEvent _gameStateEvent;


		public void SetupDependencies(DependencyManager manager)
		{
			// lets set the systems we want to be enabled
			
			Add(manager.Get<InputSystem>());
			Add(manager.Get<PhysicalBodiesSystem>());
			Add(manager.Get<PlayerSpawnSystem>());
			Add(manager.Get<AsteroidSpawnerSystem>());
			Add(manager.Get<CollisionHandlingSystem>());
			Add(manager.Get<ShootSystem>());
			
			Add(manager.Get<EntityBoundariesSystem>());
			
			Add(manager.Get<AsteroidExplosionSystem>());
			Add(manager.Get<BulletCollisionSystem>());
			Add(manager.Get<ShipCollisionSystem>());
			
			// UI
			Add(manager.Get<GameUISystem>());
			Add(manager.Get<MainMenuSystem>());
			Add(manager.Get<CameraUpdateSystem>());

			Add(manager.Get<GameStateSystem>());
			
			Add(manager.Get<GameBackgroundSystem>());
			
			_gameStateEvent = manager.Get<GameStateEvent>();
		}

		private void Awake()
		{
			DependencyManager.Setup();
			foreach (System system in _systems)
			{
				if (system is ISetupSystem setupSystem)
					setupSystem.Setup();
			}
			
			_gameStateEvent.TriggerOnGameLoad();
		}

		private void Update()
		{
			foreach (IExecuteSystem executeSystem in _executeSystems)
				executeSystem.Execute(Time.deltaTime);
		}

		private void LateUpdate()
		{
			// we need to destroy the entities after we are done with the execute and other physics callbacks
			EntityFactory<Ship>.TriggerEntitiesDestroyed();
			EntityFactory<Asteroid>.TriggerEntitiesDestroyed();
			EntityFactory<Bullet>.TriggerEntitiesDestroyed();
			EntityFactory<Game>.TriggerEntitiesDestroyed();
		}

		public void DisableSystem(System system)
		{
			if (system is IExecuteSystem executeSystem)
				_executeSystems.Remove(executeSystem);
			
			system.Disable();
		}
		public void EnableSystem(System system)
		{
			if (system is IExecuteSystem executeSystem)
				_executeSystems.Add(executeSystem);
			
			system.Enable();
		}

		private void Add(System system)
		{
			_systems.Add(system);
			if (system is IExecuteSystem executeSystem)
				_executeSystems.Add(executeSystem);
		}
	}
}