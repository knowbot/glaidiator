using UnityEngine;

namespace WarriorAnims
{
	public class WarriorMovementController:SuperStateMachine
	{
		[Header("Components")]
		private WarriorController warriorController;

		[Header("Movement")]
		public float runSpeed = 6f;
		//private readonly float rotationSpeed = 100f;
		[HideInInspector] public Vector3 currentVelocity;
		[HideInInspector] public bool crouch;

		[Header("Debug")]
		public WarriorState warriorState;
		public bool debugMessages = true;

		public Vector3 lastMove = Vector3.zero;

		[HideInInspector] public Vector3 lookDirection { get; private set; }

		private void Start()
		{
			warriorController = GetComponent<WarriorController>();

			// Set currentState to Idle on start.
			currentState = WarriorState.Idle;
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
			//transform.position += currentVelocity * warriorController.superCharacterController.deltaTime;

			// // If alive and is moving, set animator.
			// if (!warriorController.isDead && warriorController.canMove && !warriorController.isBlocking) {
			// 	if (currentVelocity.magnitude > 0 && warriorController.HasMoveInput()) {
			// 		warriorController.isMoving = true;
			// 		warriorController.SetAnimatorBool("Moving", true);
			// 		warriorController.SetAnimatorFloat("Velocity Z", currentVelocity.magnitude);
			// 	}
			// 	else {
			// 		warriorController.isMoving = false;
			// 		warriorController.SetAnimatorBool("Moving", false);
			// 		warriorController.SetAnimatorFloat("Velocity Z", 0);
			// 	}
			// }
			//
			// if (warriorController.HasMoveInput() && warriorController.canMove && !warriorController.isBlocking)
			// {
			// 	RotateTowardsMovementDir();
			// }
			// 	
			//
			// // Update animator with local movement values.
			// warriorController.SetAnimatorFloat("Velocity X", transform.InverseTransformDirection(currentVelocity).x);
			// warriorController.SetAnimatorFloat("Velocity Z", transform.InverseTransformDirection(currentVelocity).z);

			//warriorState = (WarriorState)currentState;
		}

		#endregion
		

		#region States

		// Below are the state functions. Each one is called based on the name of the state, so when currentState = Idle, we call Idle_EnterState.
		private void Idle_EnterState()
		{
			if (debugMessages) { Debug.Log("Idle_EnterState"); }
			warriorController.superCharacterController.EnableSlopeLimit();
			warriorController.superCharacterController.EnableClamping();
		}

		// Run every frame we are in the idle state.
		private void Idle_SuperUpdate()
		{
			if (debugMessages) { Debug.Log("Idle_SuperUpdate"); }
			// Moving.
			if (warriorController.HasMoveInput() && warriorController.canMove && !warriorController.isBlocking) {
				currentState = WarriorState.Move;
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
		{ warriorController.SetAnimatorBool("Moving", true); }

		private void Move_EnterState()
		{
			if (debugMessages) {Debug.Log("Move_EnterState"); }
		}

		private void Move_SuperUpdate()
		{
			if (debugMessages) { Debug.Log("Move_SuperUpdate"); }
			// Set speed determined by movement type.
			if (warriorController.HasMoveInput() && warriorController.canMove) {
				
				// Run.
				currentVelocity = warriorController.moveInput * runSpeed;
			}
			// Not moving, Idle.
			else { currentState = WarriorState.Idle; }
		}

		#endregion

		/// <summary>
		/// Rotate towards the direction the Warrior is moving.
		/// </summary>
		private void RotateTowardsMovementDir()
		{
			if (warriorController.moveInput != Vector3.zero)
			{
				if (debugMessages) { Debug.Log("RotateTowardsMovementDir"); }
				//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(warriorController.moveInput), Time.deltaTime * rotationSpeed);
			}
		}
	}
}