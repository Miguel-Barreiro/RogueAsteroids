using System;
using Configuration;
using Core;
using Entities;
using Events;
using Events.UI;
using UnityEngine;

namespace Systems.Input
{
    public sealed class InputSystem: Core.System, ISetupSystem, IExecuteSystem, DependencyManager.IDependencyRequired
    {
        private readonly PlayerControls _controls = new PlayerControls();
        private GameStateEvent _gameStateEvent;
        private Camera _mainCamera;
        private EntityCycleEvent<Ship> _shipCycleEvent;
        private Ship _ship;
        private ShipConfig _shipConfig;
        private ShootEvent _shootEvent;
        
        private float _timeBetweenShots = 0;

        private const float TIME_BETWEEN_SHOTS = 0.2f;

        public void Setup()
        {
            
            _gameStateEvent.OnGameStart += OnGameStart;
            _gameStateEvent.OnGameEnd += OnGameEnd;
            
            _shipCycleEvent.OnCreated += ship => { _ship = ship; };
            _shipCycleEvent.OnDestroyed += ship => { if (_ship == ship) _ship = null; };
        }
        
        private void OnGameEnd()
        {
            _controls.Player.Disable();
        }

        private void OnGameStart()
        {
            _controls.Player.Enable();
        }

        public void Execute(float elapsedTime)
        {
            if (_ship != null)
            {
                Vector2 direction = _mainCamera.ScreenToViewportPoint(_controls.Player.Look.ReadValue<Vector2>());
                // we need to shift the direction to the -1 to 1 range 
                direction = direction - Vector2.one / 2;
                direction = direction * 2;
                direction.Normalize();
                _ship.PhysicalBody.Direction = direction;

                if (_controls.Player.Move.IsPressed() )
                    _ship.PhysicalBody.Velocity = _ship.View.GameObject.Value.transform.right * _shipConfig.Velocity;
                else
                    _ship.PhysicalBody.Velocity = Vector2.zero;

                _timeBetweenShots = Math.Min(_timeBetweenShots + elapsedTime, TIME_BETWEEN_SHOTS);
                if (_controls.Player.Shoot.IsPressed() && _timeBetweenShots >= TIME_BETWEEN_SHOTS)
                {
                    _timeBetweenShots -= TIME_BETWEEN_SHOTS;
                    _shootEvent.TriggerShoot(_ship);
                }
            }
        }

        public void SetupDependencies(DependencyManager manager)
        {
            _gameStateEvent = manager.Get<GameStateEvent>();
            _mainCamera = manager.Get<Camera>();
            _shipCycleEvent = manager.Get<EntityCycleEvent<Ship>>();
            _shipConfig = manager.Get<ShipConfig>();
            _shootEvent = manager.Get<ShootEvent>();
        }
    }
    
}
