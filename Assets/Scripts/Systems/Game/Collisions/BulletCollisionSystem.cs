using Core;
using Entities;
using Events.Collision;
using UnityEngine;
using View;

namespace Systems.Game.Collisions
{
	public class BulletCollisionSystem : System, ISetupSystem,DependencyManager.IDependencyRequired 
	{
		private EntityFactory<Bullet> _bulletFactory;
		private CollisionEvent<Bullet> _collisionEvent;

		public void Setup()
		{
			_collisionEvent.OnCollisionWithAsteroid += OnCollisionWithAsteroid;
			_collisionEvent.OnCollisionWithShip += (bullet, ship, collision) => { DestroyBullet(bullet); };
		}


		private void OnCollisionWithAsteroid(Bullet bullet, Asteroid asteroid, Collision2D collision)
		{
			DestroyBullet(bullet);
		}
		
		private void DestroyBullet(Bullet bullet)
		{
			_bulletFactory.DestroyEntity(bullet);
		}

		public void SetupDependencies(DependencyManager manager)
		{
			_bulletFactory = manager.Get<EntityFactory<Bullet>>();
			_collisionEvent = manager.Get<CollisionEvent<Bullet>>();
		}
	}
}