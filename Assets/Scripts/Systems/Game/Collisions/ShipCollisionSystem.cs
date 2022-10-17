using Core;
using Entities;
using Events.Collision;
using UnityEngine;

namespace Systems.Game.Collisions
{
	public class ShipCollisionSystem : System, ISetupSystem, DependencyManager.IDependencyRequired
	{
		private CollisionEvent<Ship> _collisionEvent;

		public void Setup()
		{
			_collisionEvent.OnCollisionWithAsteroid += OnCollisionWithAsteroid;
		}


		private void OnCollisionWithAsteroid(Ship ship, Asteroid asteroid, Collision2D collision)
		{
			ship.Lifes.Value--;
		}
		
		public void SetupDependencies(DependencyManager manager)
		{
			_collisionEvent = manager.Get<CollisionEvent<Ship>>();
		}
	}
}