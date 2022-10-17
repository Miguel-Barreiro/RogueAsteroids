using System.Collections.Generic;
using Core;
using Entities;
using Events;
using Events.Collision;
using UnityEngine;
using View;

namespace Systems.Game.Collisions
{
	public sealed class CollisionHandlingSystem:System, ISetupSystem, DependencyManager.IDependencyRequired
	{
		private EntityCycleEvent<Ship> _shipCycleEvent;
		private EntityCycleEvent<Asteroid> _asteroidCycleEvent;
		private EntityCycleEvent<Bullet> _bulletCycleEvent;

		private readonly Dictionary<PhysicsBodyView, Asteroid> _asteroidsByBody = new Dictionary<PhysicsBodyView, Asteroid>();
		private readonly Dictionary<PhysicsBodyView, Ship> _shipsByBody = new Dictionary<PhysicsBodyView, Ship>();
		private readonly Dictionary<PhysicsBodyView, Bullet> _bulletsByBody = new Dictionary<PhysicsBodyView, Bullet>();
		private EntityFactory<Bullet> _bulletFactory;
		private PrefabFactory _prefabFactory;
		private CollisionEvent<Asteroid> _asteroidCollisionEvent;
		private CollisionEvent<Bullet> _bulletCollisionEvent;
		private CollisionEvent<Ship> _shipCollisionEvent;


		private void OnAsteroidCollision(Collision2D collision, PhysicsBodyView bodyView)
		{
			Asteroid originAsteroid = _asteroidsByBody[bodyView];

			PhysicsBodyView targetBodyView = collision.gameObject.GetComponent<PhysicsBodyView>();
			if (targetBodyView != null)
			{
				if (_shipsByBody.TryGetValue(targetBodyView, out Ship ship))
				{
					_asteroidCollisionEvent.TriggerCollision(originAsteroid, ship, collision);
					return;
				}
				if (_bulletsByBody.TryGetValue(targetBodyView, out Bullet bullet))
				{
					_asteroidCollisionEvent.TriggerCollision(originAsteroid, bullet, collision);
					return;
				}
				if (_asteroidsByBody.TryGetValue(targetBodyView, out Asteroid asteroid))
				{
					_asteroidCollisionEvent.TriggerCollision(originAsteroid, asteroid, collision);
					return;
				}
			}
		}

		
		private void OnShipCollision( Collision2D collision, PhysicsBodyView bodyView)
		{
			Ship originShip = _shipsByBody[bodyView];

			PhysicsBodyView targetBodyView = collision.gameObject.GetComponent<PhysicsBodyView>();
			if (targetBodyView != null)
			{
				if (_shipsByBody.TryGetValue(targetBodyView, out Ship ship))
				{
					_shipCollisionEvent.TriggerCollision(originShip, ship, collision);
					return;
				}
				if (_bulletsByBody.TryGetValue(targetBodyView, out Bullet bullet))
				{
					_shipCollisionEvent.TriggerCollision(originShip, bullet, collision);
					return;
				}
				if (_asteroidsByBody.TryGetValue(targetBodyView, out Asteroid asteroid))
				{
					_shipCollisionEvent.TriggerCollision(originShip, asteroid, collision);
					return;
				}
			}

			// ship.Lifes.Value = Math.Max(0, ship.Lifes.Value-1);
		}

		private void OnBulletCollision(Collision2D collision, PhysicsBodyView bodyView)
		{
			Bullet originBullet = _bulletsByBody[bodyView];
			
			PhysicsBodyView targetBodyView = collision.gameObject.GetComponent<PhysicsBodyView>();
			if (targetBodyView != null)
			{
				if (_shipsByBody.TryGetValue(targetBodyView, out Ship ship))
				{
					_bulletCollisionEvent.TriggerCollision(originBullet, ship, collision);
					return;
				}
				if (_bulletsByBody.TryGetValue(targetBodyView, out Bullet bullet))
				{
					_bulletCollisionEvent.TriggerCollision(originBullet, bullet, collision);
					return;
				}
				if (_asteroidsByBody.TryGetValue(targetBodyView, out Asteroid asteroid))
				{
					_bulletCollisionEvent.TriggerCollision(originBullet, asteroid, collision);
					return;
				}
			}
			
			// _bulletFactory.DestroyEntity(bullet);
			// _prefabFactory.Destroy(bullet.View.GameObject.Value );
		}
		
		
		public void Setup()
		{
			_asteroidCycleEvent.OnCreated += OnAsteroidCreated;
			_asteroidCycleEvent.OnDestroyed += OnAsteroidDestroyed;

			_shipCycleEvent.OnCreated += OnShipCreated;
			_shipCycleEvent.OnDestroyed += OnShipDestroyed;

			_bulletCycleEvent.OnCreated += OnBulletCreated;
			_bulletCycleEvent.OnDestroyed += OnBulletDestroyed;
			
		}

		private void OnBulletDestroyed(Bullet bullet)
		{
			_bulletsByBody.Remove(bullet.PhysicalBody.BodyView.Value);
			bullet.PhysicalBody.BodyView.Value.OnCollision -= OnBulletCollision;
		}


		private void OnBulletCreated(Bullet newBullet)
		{
			_bulletsByBody.Add(newBullet.PhysicalBody.BodyView.Value, newBullet);
			newBullet.PhysicalBody.BodyView.Value.OnCollision += OnBulletCollision;
		}

		private void OnAsteroidDestroyed(Asteroid asteroid)
		{
			_asteroidsByBody.Remove(asteroid.PhysicalBody.BodyView.Value);
			asteroid.PhysicalBody.BodyView.Value.OnCollision -= OnAsteroidCollision;
		}

		private void OnAsteroidCreated(Asteroid newAsteroid)
		{
			_asteroidsByBody.Add(newAsteroid.PhysicalBody.BodyView.Value, newAsteroid);
			newAsteroid.PhysicalBody.BodyView.Value.OnCollision += OnAsteroidCollision;
		}

		private void OnShipDestroyed(Ship ship)
		{
			_shipsByBody.Remove(ship.PhysicalBody.BodyView.Value);
			ship.PhysicalBody.BodyView.Value.OnCollision -= OnShipCollision;
		}

		private void OnShipCreated(Ship newShip)
		{
			_shipsByBody.Add(newShip.PhysicalBody.BodyView.Value, newShip);
			newShip.PhysicalBody.BodyView.Value.OnCollision += OnShipCollision;

		}



		public void SetupDependencies(DependencyManager manager)
		{
			_shipCycleEvent = manager.Get<EntityCycleEvent<Ship>>();
			_asteroidCycleEvent = manager.Get<EntityCycleEvent<Asteroid>>();
			_bulletCycleEvent = manager.Get<EntityCycleEvent<Bullet>>();
			_bulletFactory = manager.Get<EntityFactory<Bullet>>();
			_prefabFactory = manager.Get<PrefabFactory>();
			_asteroidCollisionEvent = manager.Get<CollisionEvent<Asteroid>>();
			_shipCollisionEvent = manager.Get<CollisionEvent<Ship>>();
			_bulletCollisionEvent = manager.Get<CollisionEvent<Bullet>>();
		}
	}
}