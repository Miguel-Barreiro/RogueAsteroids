using System;
using System.Collections.Generic;
using Entities;
using Events;
using Events.UI;
using Systems.Game;
using Systems.UI;
using UnityEngine;
using View;
using Object = System.Object;

namespace Systems 
{

	// we would use something like zenject to manage our dependencies
	public sealed class DependencyManager : MonoBehaviour
	{

		[SerializeField]
		private MainController MainController;
		
		[SerializeField]
		private UIConfig UIConfig;
		
		[SerializeField]
		private Canvas UICanvas;

		private readonly Dictionary<Type, Object> _objectsByType = new Dictionary<Type, Object>();
		private static DependencyManager _instance;

		public interface IDependencyRequired
		{
			void SetupDependencies(DependencyManager manager);
		}
		
		private void SetupBindings(){
			_objectsByType.Add(typeof(InputSystem.InputSystem), new InputSystem.InputSystem());
			_objectsByType.Add(typeof(ShipMovementSystem), new ShipMovementSystem());
			_objectsByType.Add(typeof(MainMenuSystem), new MainMenuSystem());
			_objectsByType.Add(typeof(PhysicalBodiesSystem), new PhysicalBodiesSystem());
			_objectsByType.Add(typeof(GameStateSystem), new GameStateSystem());
			
			_objectsByType.Add(typeof(PlayButtonEvent), new PlayButtonEvent());
			
			_objectsByType.Add(typeof(GameStateEvent), new GameStateEvent());

			_objectsByType.Add(typeof(EntityCycleEvent<Asteroid>), new EntityCycleEvent<Asteroid>());
			_objectsByType.Add(typeof(EntityCycleEvent<Ship>), new EntityCycleEvent<Ship>());
			_objectsByType.Add(typeof(EntityCycleEvent<Bullet>), new EntityCycleEvent<Bullet>());
			_objectsByType.Add(typeof(EntityCycleEvent<Entities.Game>), new EntityCycleEvent<Entities.Game>());
			
			_objectsByType.Add(typeof(EntityFactory<Asteroid>), new EntityFactory<Asteroid>());
			_objectsByType.Add(typeof(EntityFactory<Ship>), new EntityFactory<Ship>());
			_objectsByType.Add(typeof(EntityFactory<Bullet>), new EntityFactory<Bullet>());
			_objectsByType.Add(typeof(EntityFactory<Entities.Game>), new EntityFactory<Entities.Game>());
			
			_objectsByType.Add(typeof(MainController), MainController);
			
			_objectsByType.Add(typeof(UIConfig), UIConfig);
			_objectsByType.Add(typeof(Canvas), UICanvas);
			
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

		public GameObject Instanciate(GameObject prefab, Transform parent)
		{
			GameObject result = GameObject.Instantiate(prefab, parent);

			IDependencyRequired[] dependants = result.GetComponents<IDependencyRequired>();
			foreach (IDependencyRequired dependencyRequired in dependants)
			{
				dependencyRequired.SetupDependencies(this);
			}
			
			dependants = result.GetComponentsInChildren<IDependencyRequired>();
			foreach (IDependencyRequired dependencyRequired in dependants)
			{
				dependencyRequired.SetupDependencies(this);
			}

			return result;
		}
	}
}