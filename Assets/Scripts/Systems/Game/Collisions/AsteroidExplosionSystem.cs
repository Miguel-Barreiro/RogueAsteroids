using System;
using Configuration;
using Core;
using Entities;
using Events;
using Events.Collision;
using UnityEngine;
using View;
using Random = UnityEngine.Random;

namespace Systems.Game.Collisions
{
	public class AsteroidExplosionSystem: Core.System, ISetupSystem, DependencyManager.IDependencyRequired
	{
		private CollisionEvent<Asteroid> _collisionEvent;
		private EntityFactory<Asteroid> _asteroidFactory;
		private PrefabFactory _prefabFactory;
		private LevelConfiguration _levelConfiguration;
		private EntityCycleEvent<Entities.Game> _gameCycleEvent;
		private Entities.Game _game;

		private void OnCollisionWithBullet(Asteroid asteroid, Bullet bullet, Collision2D collision)
		{
			asteroid.Life.Value = Math.Max(0, asteroid.Life.Value-1);
			if (asteroid.Life.Value <= 0)
			{
				_game.Score.Value += asteroid.Size;
				
				Vector2 physicalBodyVelocity = asteroid.PhysicalBody.BodyView.Value.RigidBody.velocity;
				int newAsteroidSize = Mathf.FloorToInt(asteroid.Size / 3f);
				
				_asteroidFactory.DestroyEntity(asteroid);

				if (newAsteroidSize > 1)
					SpawnSmallAsteroids(collision, physicalBodyVelocity, newAsteroidSize);
				
			}
		}

		private void SpawnSmallAsteroids(Collision2D collision, Vector2 bodyVelocity, int asteroidSize)
		{
			
			for (int i = -1; i < 2; i++)
			{
				float offsetMultiplier = i * 0.25f;
				_asteroidFactory.CreateNew(newAsteroid =>
				{
					GameObject newAsteroidGameObject = _prefabFactory.CreateNew(_levelConfiguration.NormalAsteroidPrefab, null);

					float randomScale = asteroidSize * 0.1f;
					newAsteroidGameObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

					newAsteroid.Size = asteroidSize;
					newAsteroid.Life.Value = asteroidSize; 
					newAsteroid.View.GameObject.Value = newAsteroidGameObject;
					newAsteroid.PhysicalBody.BodyView.Value = newAsteroidGameObject.GetComponent<PhysicsBodyView>();
					newAsteroid.PhysicalBody.BreakSpeed = 0;
					
					Vector3 deltaDirection = Random.rotation * Vector2.up;
					deltaDirection.z = 0;
					deltaDirection.Normalize();

					Vector2 offset = Random.rotation * Vector2.one * offsetMultiplier;
					Vector3 startingPosition = collision.transform.position + new Vector3(offset.x, offset.y, 0);

					Quaternion randomRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);

					newAsteroidGameObject.transform.SetPositionAndRotation(startingPosition, randomRotation);
					
					Vector2 startingVelocity = bodyVelocity;
					float randomVelocityMagnitude = Random.Range(1, 100) * 0.01f;
					newAsteroid.PhysicalBody.BodyView.Value.RigidBody.velocity = startingVelocity + offset.normalized * randomVelocityMagnitude;
				});
			}
		}

		public void Setup()
		{
			_collisionEvent.OnCollisionWithBullet += OnCollisionWithBullet;
			_gameCycleEvent.OnCreated += game => { _game = game; };
		}
		
		public void SetupDependencies(DependencyManager manager)
		{
			_collisionEvent = manager.Get<CollisionEvent<Asteroid>>();
			_asteroidFactory = manager.Get<EntityFactory<Asteroid>>();
			_prefabFactory = manager.Get<PrefabFactory>();
			_levelConfiguration = manager.Get<LevelConfiguration>();
			_gameCycleEvent = manager.Get<EntityCycleEvent<Entities.Game>>();
		}
	}
}