using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterInputController: MonoBehaviour
{
	private PlayerActions _playerActions;
	private Camera _camera;
	[HideInInspector] public bool inputAttackLight;
	[HideInInspector] public bool inputAttackHeavy;
	[HideInInspector] public bool inputAttackRanged;
	[HideInInspector] public bool inputBlock;
	[HideInInspector] public bool inputDodge;
	[HideInInspector] public Vector2 inputMove = Vector2.zero;

	public Vector3 GetMove => CameraRelativeInput();

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
	}

	/// <summary>
	/// Input abstraction for easier asset updates using outside control schemes.
	/// </summary>
	private void Inputs()
	{
		inputAttackLight = _playerActions.Gameplay.AttackLight.WasPressedThisFrame();
		inputAttackHeavy = _playerActions.Gameplay.AttackHeavy.WasPressedThisFrame();
		inputAttackRanged = _playerActions.Gameplay.AttackRanged.WasPressedThisFrame();
		inputBlock = _playerActions.Gameplay.Block.IsPressed();
		inputDodge = _playerActions.Gameplay.Dodge.WasPressedThisFrame();
		inputMove = _playerActions.Gameplay.Move.ReadValue<Vector2>();
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

