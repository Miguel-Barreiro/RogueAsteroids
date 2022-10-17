using System;
using Core;
using Entities;
using Events.Collision;
using View;

namespace Systems.Game.Collisions
{
	public class AsteroidExplosionSystem: System, ISetupSystem, DependencyManager.IDependencyRequired
	{
		private CollisionEvent<Asteroid> _collisionEvent;
		private EntityFactory<Asteroid> _asteroidFactory;
		private PrefabFactory _prefabFactory;

		private void OnCollisionWithBullet(Asteroid asteroid, Bullet bullet)
		{
			asteroid.Life.Value = Math.Max(0, asteroid.Life.Value-1);
			if (asteroid.Life.Value <= 0)
			{
				_asteroidFactory.DestroyEntity(asteroid, () =>
				{
					_prefabFactory.Destroy(asteroid.View.GameObject.Value);
				});
			}
		}

		public void Setup()
		{
			_collisionEvent.OnCollisionWithBullet += OnCollisionWithBullet;
		}
		
		public void SetupDependencies(DependencyManager manager)
		{
			_collisionEvent = manager.Get<CollisionEvent<Asteroid>>();
			_asteroidFactory = manager.Get<EntityFactory<Asteroid>>();
			_prefabFactory = manager.Get<PrefabFactory>();
		}
	}
}