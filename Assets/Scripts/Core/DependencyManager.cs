using System;
using System.Collections.Generic;
using Configuration;
using Entities;
using Events;
using Events.Collision;
using Events.UI;
using Systems;
using Systems.Game;
using Systems.Game.Collisions;
using Systems.Input;
using Systems.UI;
using UnityEngine;
using View;
using Object = System.Object;

namespace Core 
{

	// we would use something like zenject to manage our dependencies
	public sealed class DependencyManager : MonoBehaviour
	{

		[SerializeField]
		private MainController MainController;
		
		[SerializeField]
		private UIConfig UIConfig;

		[SerializeField]
		private ShipConfig ShipConfig;

		[SerializeField]
		private Canvas UICanvas;

		[SerializeField]
		private Camera MainCamera;
		
		// we only have one level but with a level system we could have different configurations for each one
		[SerializeField]
		private LevelConfiguration LevelConfiguration;

		private readonly Dictionary<Type, Object> _objectsByType = new Dictionary<Type, Object>();
		private static DependencyManager _instance;

		public interface IDependencyRequired
		{
			void SetupDependencies(DependencyManager manager);
		}
		
		private void SetupBindings(){
			
			AddSystems();
			AddEvents();
			AddFactories();

			_objectsByType.Add(typeof(MainController), MainController);

			_objectsByType.Add(typeof(UIConfig), UIConfig);
			_objectsByType.Add(typeof(ShipConfig), ShipConfig);
			
			_objectsByType.Add(typeof(LevelConfiguration), LevelConfiguration);
			
			_objectsByType.Add(typeof(Canvas), UICanvas);
			_objectsByType.Add(typeof(Camera), MainCamera);

		}

		private void AddSystems()
		{
			_objectsByType.Add(typeof(InputSystem), new InputSystem());
			_objectsByType.Add(typeof(ShootSystem), new ShootSystem());

			_objectsByType.Add(typeof(PhysicalBodiesSystem), new PhysicalBodiesSystem());
			_objectsByType.Add(typeof(GameStateSystem), new GameStateSystem());
			_objectsByType.Add(typeof(PlayerSpawnSystem), new PlayerSpawnSystem());
			_objectsByType.Add(typeof(AsteroidSpawnerSystem), new AsteroidSpawnerSystem());

			_objectsByType.Add(typeof(CollisionHandlingSystem), new CollisionHandlingSystem());
			_objectsByType.Add(typeof(AsteroidExplosionSystem), new AsteroidExplosionSystem());
			_objectsByType.Add(typeof(BulletCollisionSystem), new BulletCollisionSystem());
			_objectsByType.Add(typeof(ShipCollisionSystem), new ShipCollisionSystem());

			_objectsByType.Add(typeof(EntityBoundariesSystem), new EntityBoundariesSystem());
			
			_objectsByType.Add(typeof(GameBackgroundSystem), new GameBackgroundSystem());

			// UI
			_objectsByType.Add(typeof(MainMenuSystem), new MainMenuSystem());
			_objectsByType.Add(typeof(GameUISystem), new GameUISystem());
			_objectsByType.Add(typeof(CameraUpdateSystem), new CameraUpdateSystem());
			
		}

		private void AddFactories()
		{
			_objectsByType.Add(typeof(EntityFactory<Asteroid>), new EntityFactory<Asteroid>());
			_objectsByType.Add(typeof(EntityFactory<Ship>), new EntityFactory<Ship>());
			_objectsByType.Add(typeof(EntityFactory<Bullet>), new EntityFactory<Bullet>());
			_objectsByType.Add(typeof(EntityFactory<Game>), new EntityFactory<Game>());

			_objectsByType.Add(typeof(PrefabFactory), new PrefabFactory());
		}

		private void AddEvents()
		{
			_objectsByType.Add(typeof(PlayButtonEvent), new PlayButtonEvent());
			_objectsByType.Add(typeof(ShootEvent), new ShootEvent());
			
			_objectsByType.Add(typeof(GameStateEvent), new GameStateEvent());

			_objectsByType.Add(typeof(EntityCycleEvent<Asteroid>), new EntityCycleEvent<Asteroid>());
			_objectsByType.Add(typeof(EntityCycleEvent<Ship>), new EntityCycleEvent<Ship>());
			_objectsByType.Add(typeof(EntityCycleEvent<Bullet>), new EntityCycleEvent<Bullet>());
			_objectsByType.Add(typeof(EntityCycleEvent<Game>), new EntityCycleEvent<Game>());
			
			_objectsByType.Add(typeof(CollisionEvent<Bullet>), new CollisionEvent<Bullet>());
			_objectsByType.Add(typeof(CollisionEvent<Ship>), new CollisionEvent<Ship>());
			_objectsByType.Add(typeof(CollisionEvent<Asteroid>), new CollisionEvent<Asteroid>());

			_objectsByType.Add(typeof(ShipDamagedEvent), new ShipDamagedEvent());
			
		}

		private void SetupDependencies()
		{
			foreach (KeyValuePair<Type, object> pair in _objectsByType)
			{
				if (pair.Value is IDependencyRequired dependencyRequired)
					dependencyRequired.SetupDependencies(this);
			}
		}


		public T Get<T>() where T : class {
			Type type = typeof(T);
			if (_objectsByType.ContainsKey(type)) {
				return _objectsByType[type] as T;
			} else {
				Debug.LogError($"injector for type {type.Name} wasnt setup");
				return null;
			}
		}
		
		public void Setup()
		{
			SetupBindings();
			SetupDependencies();
		}

		public static DependencyManager Instance()
		{
			return _instance; 
		}

		private void Awake()
		{
			_instance = this;
		}

		public void HandleDependencies(GameObject newObject)
		{

			IDependencyRequired[] dependants = newObject.GetComponents<IDependencyRequired>();
			foreach (IDependencyRequired dependencyRequired in dependants)
			{
				dependencyRequired.SetupDependencies(this);
			}
			
			dependants = newObject.GetComponentsInChildren<IDependencyRequired>();
			foreach (IDependencyRequired dependencyRequired in dependants)
			{
				dependencyRequired.SetupDependencies(this);
			}
		}
	}
}