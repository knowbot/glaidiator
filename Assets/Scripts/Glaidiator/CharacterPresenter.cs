using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Glaidiator
{
	public class CharacterPresenter: MonoBehaviour
	{
		// Model
		private Model.Character _character;
		
		// Inputs
		private Camera _camera;
		private PlayerActions _playerActions;
		private Input _inputs;
		public TextMeshProUGUI displayState;
		
		// View
		[HideInInspector] private Transform _transform;
		[HideInInspector] public Animator animator;
		private static readonly int Moving = Animator.StringToHash("Moving");
		private static readonly int VelocityX = Animator.StringToHash("Velocity X");
		private static readonly int VelocityZ = Animator.StringToHash("Velocity Z");
		private static readonly int Action = Animator.StringToHash("Action");
		private static readonly int Trigger = Animator.StringToHash("Trigger");
		private static readonly int TriggerNumber = Animator.StringToHash("TriggerNumber");


		private void Awake()
		{
			if (!_camera) _camera = Camera.main;
			_transform = transform;
			_playerActions = new PlayerActions();
			_character = new Model.Character(_transform);
			animator = GetComponentInChildren<Animator>();
			animator.applyRootMotion = false;
			displayState = GetComponentInChildren<TextMeshProUGUI>();
		}
		
		private void OnEnable()
		{
			_playerActions.Gameplay.Enable();
			// Register observer methods
			_character.onMove += OnMove;
			_character.onStop += OnStop;
			_character.onAttack += OnAttack;
		}

		private void OnDisable()
		{
			_playerActions.Gameplay.Disable();
			// Deregister observer methods
			_character.onMove -= OnMove;
			_character.onStop -= OnStop;
			_character.onAttack -= OnAttack;
		}

		
		private void Update()
		{
			// Process inputs and pass them onto the model
			Inputs();
			_character.SetInputs(_inputs);
			// Advance the model
			_character.Tick(Time.deltaTime);
			displayState.text = _character.CurrentState.ToString();
		}
		
		
		private void Inputs()
		{
			_inputs.attackLight = _playerActions.Gameplay.AttackLight.WasPressedThisFrame();
			_inputs.attackHeavy  = _playerActions.Gameplay.AttackHeavy.WasPressedThisFrame();
			_inputs.attackRanged  = _playerActions.Gameplay.AttackRanged.WasPressedThisFrame();
			_inputs.block  = _playerActions.Gameplay.Block.IsPressed();
			_inputs.dodge  = _playerActions.Gameplay.Dodge.WasPressedThisFrame();
			_inputs.move = GetCameraRelativeMovement(_playerActions.Gameplay.Move.ReadValue<Vector2>());
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
		
		// Observer methods

		private void OnMove()
		{
			_transform.position = _character.Movement.Position;
			_transform.rotation = _character.Movement.Rotation;
			animator.SetBool(Moving, true);
			animator.SetFloat(VelocityX, _transform.InverseTransformDirection(_character.Movement.CurrVelocity).x);
			animator.SetFloat(VelocityZ, _transform.InverseTransformDirection(_character.Movement.CurrVelocity).z);
		}

		private void OnStop()
		{
			animator.SetBool(Moving, false);
			animator.SetFloat(VelocityZ, 0);
		}

		public Model.Character GetCharacter()
		{
			return _character;
		}
		
		private void OnAttack()
		{
			animator.SetInteger(Action, 1);
			animator.SetTrigger(Trigger);
			animator.SetInteger(TriggerNumber, _character.ActiveAction!.ID);
		}
	}
}