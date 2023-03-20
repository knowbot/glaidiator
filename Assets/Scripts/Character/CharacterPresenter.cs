using System;
using Character.Model;
using UnityEngine;

namespace Character.Presenter
{
	public class CharacterPresenter: MonoBehaviour
	{
		private Transform _transform;
		private Model.CharacterModel _model;
		
		private Camera _camera;
		private PlayerActions _playerActions;
		[HideInInspector]public Vector2 inputMove;
		[HideInInspector] public bool inputAttackLight;
		[HideInInspector] public bool inputAttackHeavy;
		[HideInInspector] public bool inputAttackRanged;
		[HideInInspector] public bool inputBlock;
		[HideInInspector] public bool inputDodge;
        
		private void Awake()
		{
			if (!_camera) _camera = Camera.main;
			_transform = transform;
			_playerActions = new PlayerActions();
		}
		
		private void OnEnable()
		{
			_playerActions.Gameplay.Enable();
			
			// Register observer methods
			_model.Transform.onPositionChanged += OnPositionChanged;
			_model.Transform.onRotationChanged += OnRotationChanged;
		}

		private void OnDisable()
		{
			_playerActions.Gameplay.Disable();
		}

		
		private void Update()
		{
			Inputs();
			Vector3 move = GetCameraRelativeMovement();
			if (move != Vector3.zero)
			{
				_model.Move(move, Time.deltaTime);
			}
		
		}
		
		
		private void Inputs()
		{
			inputAttackLight = _playerActions.Gameplay.AttackLight.WasPressedThisFrame();
			inputAttackHeavy = _playerActions.Gameplay.AttackHeavy.WasPressedThisFrame();
			inputAttackRanged = _playerActions.Gameplay.AttackRanged.WasPressedThisFrame();
			inputBlock = _playerActions.Gameplay.Block.IsPressed();
			inputDodge = _playerActions.Gameplay.Dodge.WasPressedThisFrame();
			inputMove = _playerActions.Gameplay.Move.ReadValue<Vector2>();
		}
		
		public Vector3 GetCameraRelativeMovement()
		{
			Vector3 forward = _camera.transform.forward;
			// Forward vector relative to the camera along the x-z plane.
			forward.y = 0;
			forward = forward.normalized;

			// Right vector relative to the camera always orthogonal to the forward vector.
			Vector3 right = new Vector3(forward.z, 0, -forward.x);
			Vector3 relativeVelocity = inputMove.x * right + inputMove.y * forward;

			return relativeVelocity;
		}
		
		// Observer methods
		private void OnPositionChanged()
		{
			transform.position = _model.Transform.Position;
		}
		
		private void OnRotationChanged()
		{
			transform.rotation = _model.Transform.Rotation;
		}
	}
}