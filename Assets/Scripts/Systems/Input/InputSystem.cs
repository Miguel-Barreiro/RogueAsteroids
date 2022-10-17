using Configuration;
using Core;
using Entities;
using Events;
using UnityEngine;
using UnityEngine.InputSystem;
using NotImplementedException = System.NotImplementedException;

namespace Systems.Input
{
    public sealed class InputSystem: System, ISetupSystem, IExecuteSystem, DependencyManager.IDependencyRequired
    {
        private readonly PlayerControls _controls = new PlayerControls();
        private GameStateEvent _gameStateEvent;
        private Camera _mainCamera;
        private EntityCycleEvent<Ship> _shipCycleEvent;
        private Ship _ship;
        private ShipConfig _shipConfig;

        public void Setup()
        {
            
            _gameStateEvent.OnGameStart += OnGameStart;
            _gameStateEvent.OnGameEnd += OnGameEnd;
            
            _shipCycleEvent.OnCreated += OnNewShip;
            
        }

        private void OnNewShip(Ship newShip)
        {
            _ship = newShip;
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
            }
        }

        public void SetupDependencies(DependencyManager manager)
        {
            _gameStateEvent = manager.Get<GameStateEvent>();
            _mainCamera = manager.Get<Camera>();
            _shipCycleEvent = manager.Get<EntityCycleEvent<Ship>>();
            _shipConfig = manager.Get<ShipConfig>();
        }
    }
    
}
