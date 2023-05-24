using UnityEngine;
using Input = Glaidiator.Model.Input;

namespace Glaidiator.Presenter
{
    public class PlayerInputProvider : MonoBehaviour, IInputProvider
    {
        private Camera _camera;
        private PlayerActions _playerActions;
        private Input _inputs;
        public Input Inputs
        {
            get => _inputs;
            private set => _inputs = value;
        }

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
            _inputs.attackLight = _playerActions.Gameplay.AttackLight.WasPressedThisFrame();
            _inputs.attackHeavy  = _playerActions.Gameplay.AttackHeavy.WasPressedThisFrame();
            _inputs.attackRanged  = _playerActions.Gameplay.AttackRanged.WasPressedThisFrame();
            _inputs.block  = _playerActions.Gameplay.Block.WasPressedThisFrame();
            _inputs.dodge  = _playerActions.Gameplay.Dodge.WasPressedThisFrame();
            _inputs.move = GetCameraRelativeMovement(_playerActions.Gameplay.Move.ReadValue<Vector2>());
            _inputs.facing = GetMouseFacingDirection(_playerActions.Gameplay.Face.ReadValue<Vector2>());
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

        private Vector3 GetMouseFacingDirection(Vector2 mouse)
        {
            return _camera.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, _camera.farClipPlane * .5f));
        }

        public Input GetInputs()
        {
            return Inputs;
        }
    }
}