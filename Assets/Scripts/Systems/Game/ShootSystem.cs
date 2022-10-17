using Configuration;
using Core;
using Entities;
using Events.UI;
using UnityEngine;
using View;

namespace Systems.Game
{
	public class ShootSystem : System, ISetupSystem, DependencyManager.IDependencyRequired
	{
		private ShootEvent _shootEvent;
		private PrefabFactory _prefabFactory;
		private EntityFactory<Bullet> _bulletFactory;
		private ShipConfig _shipConfig;

		public void Setup()
		{
			_shootEvent.OnShoot += ShootNewBullet;
		}

		private void ShootNewBullet(Ship ship)
		{
			_bulletFactory.CreateNew(bullet =>
			{
				GameObject newBulletGameObject = _prefabFactory.CreateNew(_shipConfig.BulletPrefab, null);
				bullet.PhysicalBody.BodyView.Value = newBulletGameObject.GetComponent<PhysicsBodyView>();
				bullet.View.GameObject.Value = newBulletGameObject;

				bullet.PhysicalBody.BreakSpeed = 0;
				Vector3 direction = ship.View.GameObject.Value.transform.right;
				bullet.PhysicalBody.Velocity = direction * 10f;
				Transform transform = bullet.View.GameObject.Value.transform;
				transform.position = ship.View.GameObject.Value.transform.position + direction * 0.5f;
				transform.right = direction;
			});

		}

		public void SetupDependencies(DependencyManager manager)
		{
			_shootEvent = manager.Get<ShootEvent>();
			_prefabFactory = manager.Get<PrefabFactory>();
			_bulletFactory = manager.Get<EntityFactory<Bullet>>();
			_shipConfig = manager.Get<ShipConfig>();
		}
	}
}