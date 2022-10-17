using Configuration;
using Core;
using Entities;
using Events;
using UnityEngine;
using View;

namespace Systems.Game
{
	public class AsteroidSpawnerSystem: System, IExecuteSystem, ISetupSystem, DependencyManager.IDependencyRequired
	{
		private PrefabFactory _prefabFactory;
		private GameStateEvent _gameStateEvent;
		private EntityFactory<Asteroid> _asteroidFactory;
		private LevelConfiguration _levelConfiguration;
		private EntityCycleEvent<Ship> _shipCycleEvent;
		private Ship _ship;
		private EntityCycleEvent<Asteroid> _asteroidCycleEvent;

		private float _timeSinceNextAsteroidSpawn = 0;

		public void Execute(float elapsedTime)
		{
			_timeSinceNextAsteroidSpawn += elapsedTime;
			if (_timeSinceNextAsteroidSpawn > 10)
			{
				_timeSinceNextAsteroidSpawn -= 10;
				SpawnNewAsteroid();
			}
		}

		public void Setup()
		{
			_shipCycleEvent.OnCreated += OnNewShip;
			_asteroidCycleEvent.OnDestroyed += OnAsteroidDestroy;
			_gameStateEvent.OnGameStart += OnGameStart;
		}

		private void OnGameStart()
		{
			for (int i = 0; i < 10; i++) {
				SpawnNewAsteroid(i-2);
			}
			for (int i = 0; i < 5; i++) {
				SpawnNewAsteroid(i+10f);
			}
			for (int i = 0; i < 30; i++) {
				SpawnNewAsteroid(i*2+10f);
			}
		}

		private void OnAsteroidDestroy(Asteroid asteroid)
		{
			SpawnNewAsteroid();
		}

		private void SpawnNewAsteroid(float extraDistance = 0)
		{
			Transform shipTransform = _ship.View.GameObject.Value.transform;

			_asteroidFactory.CreateNew(newAsteroid =>
			{
				GameObject newAsteroidGameObject = _prefabFactory.CreateNew(_levelConfiguration.NormalAsteroidPrefab, null);

				int randomSize = Random.Range(100, 250);
				float randomScale = randomSize * 0.01f;
				newAsteroidGameObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

				newAsteroid.Life.Value = randomSize; 
				newAsteroid.View.GameObject.Value = newAsteroidGameObject;
				newAsteroid.PhysicalBody.BodyView.Value = newAsteroidGameObject.GetComponent<PhysicsBodyView>();
				
				Vector3 deltaDirection = Random.rotation * Vector2.up;
				deltaDirection.z = 0;
				deltaDirection.Normalize();
				
				Vector3 startingPosition = shipTransform.position + deltaDirection * (5 + extraDistance);

				Quaternion randomRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);

				newAsteroidGameObject.transform.SetPositionAndRotation(startingPosition, randomRotation);

				float randomVelocityMagnitude = Random.Range(0, 100) * 0.05f;
				Vector2 startingVelocity = (shipTransform.position - startingPosition).normalized * randomVelocityMagnitude;

				newAsteroid.PhysicalBody.BodyView.Value.RigidBody.velocity = startingVelocity;
			});
		}


		private void OnNewShip(Ship newShip){
			_ship = newShip;
		}

		public void SetupDependencies(DependencyManager manager)
		{
			_prefabFactory = manager.Get<PrefabFactory>();
			_gameStateEvent = manager.Get<GameStateEvent>();
			_asteroidFactory = manager.Get<EntityFactory<Asteroid>>();
			_levelConfiguration = manager.Get<LevelConfiguration>();

			_shipCycleEvent = manager.Get<EntityCycleEvent<Ship>>();
			_asteroidCycleEvent = manager.Get<EntityCycleEvent<Asteroid>>();
		}
	}
}