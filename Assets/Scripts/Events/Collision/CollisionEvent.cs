using System;
using Entities;

namespace Events.Collision
{
	public class CollisionEvent<T>
	{
		public event Action<T, Asteroid> OnCollisionWithAsteroid;
		public event Action<T, Bullet> OnCollisionWithBullet;
		public event Action<T, Ship> OnCollisionWithShip;

		public void TriggerCollision(T origin, Asteroid target) { OnCollisionWithAsteroid?.Invoke(origin, target); }
		public void TriggerCollision(T origin, Bullet target) { OnCollisionWithBullet?.Invoke(origin, target); }
		public void TriggerCollision(T origin, Ship target) { OnCollisionWithShip?.Invoke(origin, target); }
	}
}