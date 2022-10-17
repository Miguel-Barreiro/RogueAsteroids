using System;
using Entities;
using UnityEngine;

namespace Events.Collision
{
	public class CollisionEvent<T>
	{
		public event Action<T, Asteroid, Collision2D> OnCollisionWithAsteroid;
		public event Action<T, Bullet, Collision2D> OnCollisionWithBullet;
		public event Action<T, Ship, Collision2D> OnCollisionWithShip;

		public void TriggerCollision(T origin, Asteroid target, Collision2D collision) { OnCollisionWithAsteroid?.Invoke(origin, target, collision); }
		public void TriggerCollision(T origin, Bullet target, Collision2D collision) { OnCollisionWithBullet?.Invoke(origin, target, collision); }
		public void TriggerCollision(T origin, Ship target, Collision2D collision) { OnCollisionWithShip?.Invoke(origin, target, collision); }
	}
}