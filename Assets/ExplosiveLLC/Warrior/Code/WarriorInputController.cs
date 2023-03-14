using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace WarriorAnims
{
	public class WarriorInputController:MonoBehaviour
	{
		private PlayerActions _playerActions;
		private Camera _camera;
		[HideInInspector] public bool inputAttack;
		[HideInInspector] public bool inputAttackMove;
		[HideInInspector] public bool inputAttackRanged;
		[HideInInspector] public bool inputAttackSpecial;
		[HideInInspector] public bool inputBlock;
		[HideInInspector] public bool inputDeath;
		[HideInInspector] public bool inputLightHit;
		[HideInInspector] public bool inputDodge;
		[HideInInspector] public Vector2 inputMove = Vector2.zero;

		public Vector3 MoveInput => CameraRelativeInput();

		private void Awake()
		{
			_playerActions = new PlayerActions();
			if(!_camera) _camera = Camera.main;
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
			Inputs();
			Toggles();
		}

		/// <summary>
		/// Input abstraction for easier asset updates using outside control schemes.
		/// </summary>
		private void Inputs()
		{
			inputAttack = _playerActions.Gameplay.AttackLight.WasPressedThisFrame();
			inputAttackRanged = _playerActions.Gameplay.AttackRanged.WasPressedThisFrame();
			inputAttackSpecial = _playerActions.Gameplay.AttackHeavy.WasPressedThisFrame();
			inputBlock = _playerActions.Gameplay.Block.IsPressed();
			inputDeath = Input.GetButtonDown("Death");
			inputLightHit = Input.GetButtonDown("LightHit");
			inputMove = _playerActions.Gameplay.Move.ReadValue<Vector2>();
			inputDodge = _playerActions.Gameplay.Dodge.WasPressedThisFrame();
		}

		private void Toggles()
		{
			// Slow time toggle.
			if (Input.GetKeyDown(KeyCode.T)) {
				if (Time.timeScale != 1) { Time.timeScale = 1; }
				else { Time.timeScale = 0.125f; }
			}
			// Pause toggle.
			if (Input.GetKeyDown(KeyCode.P)) {
				if (Time.timeScale != 1) { Time.timeScale = 1; }
				else { Time.timeScale = 0f; }
			}
		}

		public bool hasBlockInput
		{
			get { return inputBlock;  }
		}

		/// <summary>
		/// Movement based off camera facing.
		/// </summary>
		private Vector3 CameraRelativeInput()
		{
			// Forward vector relative to the camera along the x-z plane.
			Vector3 forward = _camera.transform.TransformDirection(Vector3.forward);
			forward.y = 0;
			forward = forward.normalized;

			// Right vector relative to the camera always orthogonal to the forward vector.
			Vector3 right = new Vector3(forward.z, 0, -forward.x);
			Vector3 relativeVelocity = inputMove.x * right + inputMove.y * forward;

			return relativeVelocity;
		}
	}
}
