using Core;
using Entities;
using Events.Collision;
using View;

namespace Systems.Game.Collisions
{
	public class BulletCollisionSystem : System, ISetupSystem,DependencyManager.IDependencyRequired 
	{
		private PrefabFactory _prefabFactory;
		private EntityFactory<Bullet> _bulletFactory;
		private CollisionEvent<Bullet> _collisionEvent;

		public void Setup()
		{
			_collisionEvent.OnCollisionWithAsteroid += OnCollisionWithAsteroid;
			_collisionEvent.OnCollisionWithShip += (bullet, ship) => { DestroyBullet(bullet); };
			// _collisionEvent.OnCollisionWithBullet += (bullet, otherBullet) => { DestroyBullet(bullet); };
		}


		private void OnCollisionWithAsteroid(Bullet bullet, Asteroid asteroid)
		{
			DestroyBullet(bullet);
		}
		
		private void DestroyBullet(Bullet bullet)
		{
			_bulletFactory.DestroyEntity(bullet, () =>
			{
				_prefabFactory.Destroy(bullet.View.GameObject.Value);
			});
		}

		public void SetupDependencies(DependencyManager manager)
		{
			_prefabFactory = manager.Get<PrefabFactory>();
			_bulletFactory = manager.Get<EntityFactory<Bullet>>();
			_collisionEvent = manager.Get<CollisionEvent<Bullet>>();
		}
	}
}