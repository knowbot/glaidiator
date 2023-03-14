using UnityEngine;


public class CharacterMovementController:SuperStateMachine
{
	[Header("Components")]
	private CharacterController characterController;

	[Header("Movement")]
	public float runSpeed = 6f;
	private readonly float rotationSpeed = 100f;
	[HideInInspector] public Vector3 currentVelocity;

	[Header("Debug")]
	public CharacterState characterState;
	public bool debugMessages = true;

	[HideInInspector] public Vector3 lookDirection { get; private set; }

	private void Start()
	{
		characterController = GetComponent<CharacterController>();
		currentState = CharacterState.Idle;
	}

	#region Updates

	/*void Update () {
	 * Update is normally run once on every frame update. We won't be using it in this case, since the SuperCharacterController
	 * component sends a callback Update called SuperUpdate. SuperUpdate is recieved by the SuperStateMachine, and then fires further
	 * callbacks depending on the state
	}*/

	// Put any code in here you want to run BEFORE the state's update function. This is run regardless of what state you're in.
	protected override void EarlyGlobalSuperUpdate()
	{
	}

	// Put any code in here you want to run AFTER the state's update function.  This is run regardless of what state you're in.
	protected override void LateGlobalSuperUpdate()
	{
		// Move the player by our velocity every frame.
		transform.position += currentVelocity * characterController.superCharacterController.deltaTime;

		// If alive and is moving, set animator.
		if (!characterController.isDead && characterController.canMove && !characterController.isBlocking) {
			if (currentVelocity.magnitude > 0 && characterController.HasMoveInput()) {
				characterController.isMoving = true;
				characterController.SetAnimatorBool("Moving", true);
				characterController.SetAnimatorFloat("Velocity Z", currentVelocity.magnitude);
			}
			else {
				characterController.isMoving = false;
				characterController.SetAnimatorBool("Moving", false);
				characterController.SetAnimatorFloat("Velocity Z", 0);
			}
		}
		
		// Just visual, no need to simulate this
		if (characterController.HasMoveInput() && characterController.canMove && !characterController.isBlocking)
		{
			RotateTowardsMovementDir();
		}
			

		// Update animator with local movement values.
		characterController.SetAnimatorFloat("Velocity X", transform.InverseTransformDirection(currentVelocity).x);
		characterController.SetAnimatorFloat("Velocity Z", transform.InverseTransformDirection(currentVelocity).z);

		characterState = (CharacterState)currentState;
	}

	#endregion
	

	#region States

	// Below are the state functions. Each one is called based on the name of the state, so when currentState = Idle, we call Idle_EnterState.
	private void Idle_EnterState()
	{
		if (debugMessages) { Debug.Log("Idle_EnterState"); }
		characterController.superCharacterController.EnableSlopeLimit();
		characterController.superCharacterController.EnableClamping();
	}

	// Run every frame we are in the idle state.
	private void Idle_SuperUpdate()
	{
		if (debugMessages) { Debug.Log("Idle_SuperUpdate"); }
		// Moving.
		if (characterController.HasMoveInput() && characterController.canMove && !characterController.isBlocking) {
			currentState = CharacterState.Move;
			return;
		}
		currentVelocity = Vector3.zero;
	}

	// Run once when exit the Idle state.
	private void Idle_ExitState()
	{
		if (debugMessages) { Debug.Log("Idle_ExitState"); }
	}

	// Run once when exit the Idle state.
	private void Idle_MoveState()
	{ characterController.SetAnimatorBool("Moving", true); }

	private void Move_EnterState()
	{
		if (debugMessages) {Debug.Log("Move_EnterState"); }
	}

	private void Move_SuperUpdate()
	{
		if (debugMessages) { Debug.Log("Move_SuperUpdate"); }
		// Set speed determined by movement type.
		if (characterController.HasMoveInput() && characterController.canMove) {
			// Run.
			currentVelocity = characterController.inputMove * runSpeed;
		}
		// Not moving, Idle.
		else { currentState = CharacterState.Idle; }
	}

	#endregion

	/// <summary>
	/// Rotate towards the direction the Character is moving.
	/// </summary>
	private void RotateTowardsMovementDir()
	{
		if (characterController.inputMove != Vector3.zero)
		{
			if (debugMessages) { Debug.Log("RotateTowardsMovementDir"); }
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(characterController.inputMove), Time.deltaTime * rotationSpeed);
		}
	}
}
