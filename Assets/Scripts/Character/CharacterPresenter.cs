using UnityEngine;

namespace Character
{
	public class CharacterPresenter: MonoBehaviour

	{
	private PlayerActions _playerActions;
	private Camera _camera;
	[HideInInspector] public bool inputAttackLight;
	[HideInInspector] public bool inputAttackHeavy;
	[HideInInspector] public bool inputAttackRanged;
	[HideInInspector] public bool inputBlock;
	[HideInInspector] public bool inputDodge;
	private Vector2 _inputMove;
	public Vector3 InputMove => CameraRelativeInput();

	private void Awake()
	{
		_playerActions = new PlayerActions();
		if (!_camera) _camera = Camera.main;
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

	private void Inputs()
	{
		inputAttackLight = _playerActions.Gameplay.AttackLight.WasPressedThisFrame();
		inputAttackHeavy = _playerActions.Gameplay.AttackHeavy.WasPressedThisFrame();
		inputAttackRanged = _playerActions.Gameplay.AttackRanged.WasPressedThisFrame();
		inputBlock = _playerActions.Gameplay.Block.IsPressed();
		inputDodge = _playerActions.Gameplay.Dodge.WasPressedThisFrame();
		_inputMove = _playerActions.Gameplay.Move.ReadValue<Vector2>();
	}

	private Vector3 CameraRelativeInput()
	{
		// Forward vector relative to the camera along the x-z plane.
		Vector3 forward = _camera.transform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;

		// Right vector relative to the camera always orthogonal to the forward vector.
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		Vector3 relativeVelocity = _inputMove.x * right + _inputMove.y * forward;

		return relativeVelocity;
	}
	}
}