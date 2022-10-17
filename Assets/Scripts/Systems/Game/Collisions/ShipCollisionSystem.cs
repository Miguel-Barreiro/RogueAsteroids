using Core;
using Entities;
using Events.Collision;
using UnityEngine;

namespace Systems.Game.Collisions
{
	public class ShipCollisionSystem : Core.System, ISetupSystem, DependencyManager.IDependencyRequired
	{
		private CollisionEvent<Ship> _collisionEvent;
		private ShipDamagedEvent _shipDamagedEvent;

		public void Setup()
		{
			_collisionEvent.OnCollisionWithAsteroid += OnCollisionWithAsteroid;
		}


		private void OnCollisionWithAsteroid(Ship ship, Asteroid asteroid, Collision2D collision)
		{
			_shipDamagedEvent.TriggerOnDamaged(-1);
			ship.Lifes.Value--;
		}
		
		public void SetupDependencies(DependencyManager manager)
		{
			_collisionEvent = manager.Get<CollisionEvent<Ship>>();
			_shipDamagedEvent = manager.Get<ShipDamagedEvent>();
		}
	}
}