using Core;
using Entities;
using Events;
using UnityEngine;

namespace Systems.Game
{
	public class CameraUpdateSystem : Core.System, IExecuteSystem, ISetupSystem, DependencyManager.IDependencyRequired
	{
		private Camera _camera;
		private EntityCycleEvent<Ship> _shipCycleEvent;
		private Ship _ship;
		private GameStateEvent _gameStateEvent;

		private readonly Vector3 CAMERA_OFFSET = new Vector3(0, 0, -20);

		public void Execute(float elapsedTime)
		{
			if (_ship != null)
			{
				_camera.transform.position = _ship.View.GameObject.Value.transform.position + CAMERA_OFFSET ;
			}
		}

		public void SetupDependencies(DependencyManager manager)
		{
			_gameStateEvent = manager.Get<GameStateEvent>();
			_camera = manager.Get<Camera>();
			_shipCycleEvent = manager.Get<EntityCycleEvent<Ship>>();
		}

		public void Setup()
		{
			_shipCycleEvent.OnCreated += newShip => { _ship = newShip; };
			_shipCycleEvent.OnDestroyed += ship => { if (_ship == ship) _ship = null; };
			_gameStateEvent.OnGameStart += OnGameStart;
		}

		private void OnGameStart()
		{
			
		}
	}
}