using UnityEngine;

namespace Glaidiator
{
	public class CharacterPresenter: MonoBehaviour
	{
		private Transform _transform;
		private Model.Character _character;
		
		private Camera _camera;
		private PlayerActions _playerActions;

		
		public Vector3 InputMove => GetCameraRelativeMovement();

		[HideInInspector] public bool inputAttackLight;
		[HideInInspector] public bool inputAttackHeavy;
		[HideInInspector] public bool inputAttackRanged;
		[HideInInspector] public bool inputBlock;
		[HideInInspector] public bool inputDodge;
		[HideInInspector] private Vector2 _inputMove;

		private void Awake()
		{
			if (!_camera) _camera = Camera.main;
			_transform = transform;
			_playerActions = new PlayerActions();
			_character = new Model.Character(_transform);
		}
		
		private void OnEnable()
		{
			_playerActions.Gameplay.Enable();
			
			// Register observer methods
			_character.Movement.onPositionChanged += OnPositionChanged;
			_character.Movement.onRotationChanged += OnRotationChanged;
		}

		private void OnDisable()
		{
			_playerActions.Gameplay.Disable();
			_character.Movement.onPositionChanged -= OnPositionChanged;
			_character.Movement.onRotationChanged -= OnRotationChanged;
		}

		
		private void Update()
		{
			Inputs();
			if (InputMove != Vector3.zero)
			{
				_character.Move(InputMove, Time.deltaTime);
			}
		
		}
		
		
		private void Inputs()
		{
			inputAttackLight = _playerActions.Gameplay.AttackLight.WasPressedThisFrame();
			inputAttackHeavy = _playerActions.Gameplay.AttackHeavy.WasPressedThisFrame();
			inputAttackRanged = _playerActions.Gameplay.AttackRanged.WasPressedThisFrame();
			inputBlock = _playerActions.Gameplay.Block.IsPressed();
			inputDodge = _playerActions.Gameplay.Dodge.WasPressedThisFrame();
			_inputMove = _playerActions.Gameplay.Move.ReadValue<Vector2>();
			GetCameraRelativeMovement();
		}
		
		public Vector3 GetCameraRelativeMovement()
		{
			Vector3 forward = _camera.transform.forward;
			// Forward vector relative to the camera along the x-z plane.
			forward.y = 0;
			forward = forward.normalized;
			// Right vector relative to the camera always orthogonal to the forward vector.
			Vector3 right = new Vector3(forward.z, 0, -forward.x);
			return _inputMove.x * right + _inputMove.y * forward;
		}
		
		// Observer methods
		private void OnPositionChanged()
		{
			transform.position = _character.Movement.Position;
		}
		
		private void OnRotationChanged()
		{
			transform.rotation = _character.Movement.Rotation;
		}
	}
}