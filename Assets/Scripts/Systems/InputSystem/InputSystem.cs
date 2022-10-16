using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems.InputSystem
{
    public sealed class InputSystem: System, ISetupSystem
    {
        private readonly PlayerControls _controls = new PlayerControls();
        
        public void Setup()
        {
            _controls.Player.Look.performed += OnLook; 
            _controls.Player.Enable();
        }

        private void OnLook(InputAction.CallbackContext obj)
        {
            Debug.Log("ON look " + obj.phase + obj.ReadValue<Vector2>());
        }
    }
    
}
