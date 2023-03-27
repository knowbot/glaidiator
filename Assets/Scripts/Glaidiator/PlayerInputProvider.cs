using System;
using Glaidiator.Model;
using UnityEngine;

namespace Glaidiator
{
    public class PlayerInputProvider : AInputProvider
    {
        private Camera _camera;
        private PlayerActions _playerActions;

        private void Awake()
        {
            if (!_camera) _camera = Camera.main;
            _playerActions = new PlayerActions();
        }

        private void OnEnable()
        {
            _playerActions.Gameplay.Enable();
        }

        private void OnDisable()
        {
            _playerActions.Gameplay.Disable();
        }

        private void Update()
        {
            Inputs.attackLight = _playerActions.Gameplay.AttackLight.WasPressedThisFrame();
            Inputs.attackHeavy  = _playerActions.Gameplay.AttackHeavy.WasPressedThisFrame();
            Inputs.attackRanged  = _playerActions.Gameplay.AttackRanged.WasPressedThisFrame();
            Inputs.block  = _playerActions.Gameplay.Block.IsPressed();
            Inputs.dodge  = _playerActions.Gameplay.Dodge.WasPressedThisFrame();
            Inputs.move = GetCameraRelativeMovement(_playerActions.Gameplay.Move.ReadValue<Vector2>());
        }
        
        private Vector3 GetCameraRelativeMovement(Vector2 movement)
        {
            Vector3 forward = _camera.transform.forward;
            // Forward vector relative to the camera along the x-z plane.
            forward.y = 0;
            forward = forward.normalized;
            // Right vector relative to the camera always orthogonal to the forward vector.
            Vector3 right = new Vector3(forward.z, 0, -forward.x);
            return movement.x * right + movement.y * forward;
        }

        public override Input GetInputs()
        {
            return Inputs;
        }
    }
}