using System;
using System.Collections.Generic;
using Core;
using Entities;
using Events;
using UnityEngine;
using View;

namespace Systems.Game
{
	public sealed class CollisionHandlingSystem:System, ISetupSystem, DependencyManager.IDependencyRequired
	{
		private EntityCycleEvent<Ship> _shipCycleEvent;
		private EntityCycleEvent<Asteroid> _asteroidCycleEvent;

		private readonly Dictionary<PhysicsBodyView, Asteroid> _asteroidsByBody = new Dictionary<PhysicsBodyView, Asteroid>();
		private readonly Dictionary<PhysicsBodyView, Ship> _shipsByBody = new Dictionary<PhysicsBodyView, Ship>();

		
		private void OnAsteroidCollision(Collision2D collision, PhysicsBodyView bodyView)
		{
			Asteroid asteroid = _asteroidsByBody[bodyView];
			asteroid.Life.Value = Math.Max(0, asteroid.Life.Value-1);
		}


		private void OnShipCollision( Collision2D collision, PhysicsBodyView bodyView)
		{
			Ship ship = _shipsByBody[bodyView];
			ship.Lifes.Value = Math.Max(0, ship.Lifes.Value-1);
		}
		
		public void Setup()
		{
			_asteroidCycleEvent.OnCreated += OnAsteroidCreated;
			_asteroidCycleEvent.OnDestroyed += OnAsteroidDestroyed;

			_shipCycleEvent.OnCreated += OnShipCreated;
			_shipCycleEvent.OnDestroyed += OnShipDestroyed;
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
			
		}
	}
}