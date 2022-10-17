using Core;
using Entities;
using Events;
using UnityEngine;

namespace Systems.Game
{
	public class EntityBoundariesSystem : Core.System, ISetupSystem, IExecuteSystem, DependencyManager.IDependencyRequired
	{
		private EntityFactory<Asteroid> _asteroidFactory;
		private EntityFactory<Bullet> _bulletFactory;
		private EntityCycleEvent<Ship> _shipCycleEvent;
		private Ship _ship;

		private const float MAX_BULLET_DISTANCE = 300;
		private const float MAX_ASTEROID_DISTANCE = 300;

		public void Execute(float elapsedTime)
		{
			if (_ship != null)
			{
				Vector3 shipPosition = _ship.View.GameObject.Value.transform.position;
				foreach (Asteroid asteroid in _asteroidFactory.GetAll())
				{
					Vector3 asteroidPosition = asteroid.View.GameObject.Value.transform.position;
					float roughDistance = Vector3.SqrMagnitude( asteroidPosition - shipPosition);
					if (roughDistance > MAX_ASTEROID_DISTANCE)
						_asteroidFactory.DestroyEntity(asteroid);
				}
				foreach (Bullet bullet in _bulletFactory.GetAll())
				{
					Vector3 bulletPosition = bullet.View.GameObject.Value.transform.position;
					float roughDistance = Vector3.SqrMagnitude( bulletPosition - shipPosition);
					if (roughDistance > MAX_BULLET_DISTANCE)
						_bulletFactory.DestroyEntity(bullet);
				}
			}
		}

		public void Setup()
		{
			_shipCycleEvent.OnCreated += ship => { _ship = ship; };
			_shipCycleEvent.OnDestroyed += ship => { if (_ship == ship) _ship = null; };
		}

		public void SetupDependencies(DependencyManager manager)
		{
			_asteroidFactory = manager.Get<EntityFactory<Asteroid>>();
			_bulletFactory = manager.Get<EntityFactory<Bullet>>();
			_shipCycleEvent = manager.Get<EntityCycleEvent<Ship>>();
		}

	}
}