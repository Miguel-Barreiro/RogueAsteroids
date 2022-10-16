using System;
using System.Collections.Generic;
using Events;
using Systems.Game;
using Systems.UI;
using UnityEngine;

namespace Systems
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
			Add(manager.Get<InputSystem.InputSystem>());
			Add(manager.Get<PhysicalBodiesSystem>());
			Add(manager.Get<MainMenuSystem>());
			
			Add(manager.Get<GameStateSystem>());
			
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