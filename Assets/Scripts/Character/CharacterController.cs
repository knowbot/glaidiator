using System.Collections;
using UnityEngine;

public class CharacterController : SuperStateMachine
{
	#region Components

	[Header("Components")]
	public Character character;
	public GameObject weapon;
	[HideInInspector] public SuperCharacterController superCharacterController;
	[HideInInspector] public CharacterMovementController characterMovementController;
	[HideInInspector] public CharacterInputController characterInputController;
	[HideInInspector] public CharacterTiming characterTiming;
	[HideInInspector] public Animator animator;
	// [HideInInspector] public CharacterIKHands ikHands;
	#endregion

	#region Inputs

	// Inputs.
	[HideInInspector] public bool inputAttackLight;
	[HideInInspector] public bool inputAttackHeavy;
	[HideInInspector] public bool inputAttackRanged;
	[HideInInspector] public bool inputBlock;
	[HideInInspector] public bool inputDodge;
	[HideInInspector] public Vector3 inputMove = Vector3.zero;

	public bool allowedInput { get { return _allowedInput; } }
	private bool _allowedInput = true;

	#endregion

	#region Variables

	// Variables.
	[HideInInspector] public bool isMoving;
	[HideInInspector] public bool isDead = false;
	[HideInInspector] public bool isBlocking = false;
	[HideInInspector] public bool waitingOnWeapons = true;
	[HideInInspector] public bool useRootMotion = false;
	private int attack;
	private int specialAttack;

	public bool canAction { get { return _canAction; } }
	private bool _canAction = true;

	public bool canBlock { get { return _canBlock; } }
	private bool _canBlock = true;

	public bool canMove { get { return _canMove; } }
	private bool _canMove = true;

	// Animation speed control. (doesn't affect lock timing)
	public float animationSpeed = 1;

	public Coroutine co;

	#endregion

	#region Initialization

	private void Awake()
	{
		// Get SuperCharacterController.
		superCharacterController = GetComponent<SuperCharacterController>();

		// Get Movement Controller.
		characterMovementController = GetComponent<CharacterMovementController>();

		// Add Timing Controllers.
		characterTiming = gameObject.AddComponent<CharacterTiming>();
		characterTiming.characterController = this;

		// Add IKHands.
		/*ikHands = GetComponentInChildren<CharacterIKHands>();
		if (ikHands != null) {
			if (character == Character.TwoHanded
				|| character == Character.Hammer
				|| character == Character.Crossbow
				|| character == Character.Spearman) {
				ikHands.canBeUsed = true;
				ikHands.BlendIK(true, 0, 0.25f);
			}
		}*/

		// Setup Animator, add AnimationEvents script.
		animator = GetComponentInChildren<Animator>();
		if (animator == null) {
			Debug.LogError("ERROR: There is no Animator component for character.");
			Debug.Break();
		} else {
			animator.gameObject.AddComponent<CharacterAnimatorEvents>();
			animator.GetComponent<CharacterAnimatorEvents>().characterController = this;
			animator.gameObject.AddComponent<CharacterAnimatorParentMove>();
			animator.GetComponent<CharacterAnimatorParentMove>().animator = animator;
			animator.GetComponent<CharacterAnimatorParentMove>().characterController = this;
			animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
			animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
		}

		// Determine input source.
		characterInputController = GetComponent<CharacterInputController>();
		
		currentState = CharacterState.Idle;
		SwitchCollisionOn();
	}

	#endregion

	#region Input

	/// <summary>
	/// Takes input from either CharacterInputController or CharacterInputSystemController.
	/// </summary>
	private void GetInput()
	{
		if (allowedInput) {
			
			inputAttackLight = characterInputController.inputAttackLight;
			inputAttackHeavy = characterInputController.inputAttackHeavy;
			inputAttackRanged = characterInputController.inputAttackRanged;
			inputBlock = characterInputController.hasBlockInput;
			inputDodge = characterInputController.inputDodge;
			inputMove = characterInputController.GetMove;
		}
	}

	/// <summary>
	/// Checks move input and returns if active.
	/// </summary>
	public bool HasMoveInput()
	{ return inputMove != Vector3.zero; }

	/// <summary>
	/// Checks block input and returns if true/false.
	/// </summary>
	public bool HasBlockInput()
	{ return inputBlock; }
	

	/// <summary>
	/// Shuts off input from CharacterInputController or CharacterInputSystemController. GUI still enabled.
	/// </summary>
	public void AllowInput(bool b)
	{ _allowedInput = b; }

	#endregion

	#region Updates

	private void Update()
	{
		GetInput();

		// Character is on ground.
		Attacking();
		if (canAction) {
			Blocking();
		}
		UpdateAnimationSpeed();
	}

	/// <summary>
	/// Updates the Animator with the animation speed multiplier.
	/// </summary>
	private void UpdateAnimationSpeed()
	{ SetAnimatorFloat("AnimationSpeed", animationSpeed); }

	#endregion

	#region Combat
	
	/// <summary>
	/// The different attack types.
	/// </summary>
	private void Attacking()
	{
		if (canAction) {
			if (inputAttackLight) { AttackLight(1); }
			if (inputAttackHeavy) { AttackHeavy(1); }
			if (inputAttackRanged) { AttackRanged(1); }
		}
	}

	/// <summary>
	/// Light attack.
	/// </summary>
	public void AttackLight(int attackNumber)
	{
		if (canAction) {
			Lock(true, true, true, 0, characterTiming.TimingLock(character, ("light" + attackNumber.ToString())));
			SetAnimatorInt("Action", attackNumber);
			SetAnimatorTrigger(AnimatorTrigger.AttackLightTrigger);
			// if (character == Character.Spearman && attackNumber == 4 && ikHands != null)
			// { ikHands.SetIKPause(characterTiming.TimingLock(character, "attack4")); }
			// if (character == Character.Spearman && attackNumber == 5 && ikHands != null)
			// { ikHands.SetIKPause(characterTiming.TimingLock(character, "attack5")); }
		}
	}
	

	/// <summary>
	/// Heavy attack.
	/// </summary>
	public void AttackHeavy(int attackNumber)
	{
		specialAttack = attackNumber;
		SetAnimatorInt("Action", attackNumber);
		SetAnimatorTrigger(AnimatorTrigger.AttackHeavyTrigger);
		Lock(true, true, true, 0, characterTiming.TimingLock(character, ("heavy" + attackNumber.ToString() )));
		// if (character == Character.Crossbow || character == Character.Hammer && ikHands != null)
		// { ikHands.SetIKPause(characterTiming.TimingLock(character, "special" + attackNumber.ToString())); }
	}
	

	/// <summary>
	/// Ranged attack.
	/// </summary>
	public void AttackRanged(int attackNumber)
	{
		specialAttack = attackNumber;
		SetAnimatorInt("Action", attackNumber);
		SetAnimatorTrigger(AnimatorTrigger.AttackRangedTigger);
		Lock(true, true, true, 0, characterTiming.TimingLock(character, ("ranged" + attackNumber.ToString())));
		// if (character == Character.TwoHanded) { StartCoroutine(_TwoHandedRangeAttack()); }
	}
	
	/// <summary>
	/// Puts the Character in blocking state.
	/// </summary>
	public void Blocking()
	{
		if (canBlock) {
			if (!isBlocking) {
				if (HasBlockInput()) {
					isBlocking = true;
					_canMove = false;
					SetAnimatorBool("Blocking", true);
					SetAnimatorTrigger(AnimatorTrigger.BlockTrigger);
					// if (character == Character.Crossbow && ikHands != null) { ikHands.BlendIK(false, 0, 0.1f); }
				}
			}
			else {
				if (!HasBlockInput()) {
					isBlocking = false;
					_canMove = true;
					SetAnimatorBool("Blocking", false);
					// if (character == Character.Crossbow && ikHands != null) { ikHands.BlendIK(true, 0, 0.1f); }
				}
			}
		}
	}

	/// <summary>
	/// Character takes a light hit.
	/// </summary>
	public void GetHit()
	{
		// SetAnimatorInt("Action", 1);
		SetAnimatorTrigger(AnimatorTrigger.LightHitTrigger);
		Lock(true, true, true, 0, characterTiming.TimingLock(character, ("lighthit1".ToString())));
	}

	/// <summary>
	/// Kills the Character, or if already dead, revives. If blocking, BlockBreak.
	/// </summary>
	private void DeathRevive()
	{
		// TODO: look at it again once HP system is in place
		// // Death/Revive.
		// if (inputDeath) {
		// 	if (!isDead) {
		// 		if (isBlocking) { BlockBreak(); }
		// 		else { Death(); }
		// 	}
		// 	else { Revive(); }
		// }
	}

	/// <summary>
	/// Kills the Character.
	/// </summary>
	public void Death()
	{
		SetAnimatorTrigger(AnimatorTrigger.DeathTrigger);
		Lock(true, true, false, 0.1f, 0f);
		// if (character == Character.Crossbow || character == Character.TwoHanded && ikHands != null) { ikHands.SetIKOff(); }
		isDead = true;
	}

	/// <summary>
	/// Revives the Character from Death.
	/// </summary>
	public void Revive()
	{
		// TODO: look at it again once HP system is in place
		// SetAnimatorTrigger(AnimatorTrigger.ReviveTrigger);
		// Lock(true, true, true, 0, characterTiming.TimingLock(character, "revive"));
		// isDead = false;
		// if (character == Character.Crossbow || character == Character.TwoHanded && ikHands != null) { ikHands.BlendIK(true, 1f, 0.25f); }
	}

	/// <summary>
	/// Character Dash.
	public void Dodge(int dodge)
	{
		Lock(true, true, true, 0, characterTiming.TimingLock(character, "dash"));
		SetAnimatorInt("Action", dodge);
		SetAnimatorTrigger(AnimatorTrigger.DodgeTrigger);
	}

	#endregion

	#region Locks

	/// <summary>
	/// Lock character movement and/or action, on a delay for a set time.
	/// </summary>
	/// <param name="lockMovement">If set to <c>true</c> lock movement.</param>
	/// <param name="lockAction">If set to <c>true</c> lock action.</param>
	/// <param name="timed">If set to <c>true</c> timed.</param>
	/// <param name="delayTime">Delay time.</param>
	/// <param name="lockTime">Lock time.</param>
	public void Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime)
	{
		if (co != null) { StopCoroutine(co); }
		co = StartCoroutine(_Lock(lockMovement, lockAction, timed, delayTime, lockTime));
	}

	//Timed -1 = infinite, 0 = no, 1 = yes.
	public IEnumerator _Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime)
	{
		if (delayTime > 0) { yield return new WaitForSeconds(delayTime); }
		if (lockMovement) { LockMove(true); }
		if (lockAction) { LockAction(true); }
		if (timed) {
			if (lockTime > 0) {
				yield return new WaitForSeconds(lockTime);
				UnLock(lockMovement, lockAction);
			}
		}
	}

	/// <summary>
	/// Keep character from moving and use or diable Rootmotion.
	/// </summary>
	public void LockMove(bool b)
	{
		_canMove = !b;
		SetAnimatorRootMotion(b);
		if (b) {
			SetAnimatorBool("Moving", false);
			inputMove = Vector3.zero;
		}
	}

	/// <summary>
	/// Keep character from doing actions.
	/// </summary>
	public void LockAction(bool b)
	{ _canAction = !b; }

	/// <summary>
	/// Keep character from blocking.
	/// </summary>
	public void LockBlock(bool b)
	{ _canBlock = !b; }
	

	/// <summary>
	/// Let character move and act again.
	/// </summary>
	private void UnLock(bool movement, bool actions)
	{
		if (movement) { LockMove(false); }
		if (actions) { LockAction(false); }
	}

	#endregion

	#region Misc

	/// <summary>
	/// Locks the movement of the Character and turns off its collision.
	/// </summary>
	public void SwitchCollisionOff()
	{
		_canMove = false;
		superCharacterController.enabled = false;
		SetAnimatorRootMotion(true);
		// if (rb != null) { rb.isKinematic = false; }
	}

	/// <summary>
	/// Unlocks the movement of the Character and turns on its collision.
	/// </summary>
	public void SwitchCollisionOn()
	{
		_canMove = true;
		superCharacterController.enabled = true;
		SetAnimatorRootMotion(false);
		//TODO: collision checks should not rely on Rigidbody
	}

	/// <summary>
	/// Set Animator Trigger.
	/// </summary>
	public void SetAnimatorTrigger(AnimatorTrigger trigger)
	{
		Debug.Log("SetAnimatorTrigger: " + trigger + " - " + ( int )trigger);
		animator.SetInteger("TriggerNumber", ( int )trigger);
		animator.SetTrigger("Trigger");
	}

	/// <summary>
	/// Set Animator Bool.
	/// </summary>
	public void SetAnimatorBool(string name, bool b)
	{ animator.SetBool(name, b); }

	/// <summary>
	/// Set Animator float.
	/// </summary>
	public void SetAnimatorFloat(string name, float f)
	{ animator.SetFloat(name, f); }

	/// <summary>
	/// Set Animator integer.
	/// </summary>
	public void SetAnimatorInt(string name, int i)
	{ animator.SetInteger(name, i); }

	/// <summary>
	/// Set Animator to use root motion or not.
	/// </summary>
	public void SetAnimatorRootMotion(bool b)
	{ useRootMotion = b; }
	
	/// <summary>
	/// Prints out all the CharacterController variables.
	/// </summary>
	public void ControllerDebug()
	{
		Debug.Log("CONTROLLER SETTINGS---------------------------");
		Debug.Log($"allowedInput:{allowedInput}   isMoving:{isMoving}     " +
			$"isDead:{isDead}    isBlocking:{isBlocking}       " +
			$"waitingOnWeapons:{waitingOnWeapons}     " +
			$"useRootMotion:{useRootMotion}    attack:{attack}     " +
			$"specialAttack:{specialAttack}    canAction:{canAction}" +
			$"canMove:{canMove}    attack:{attack}     " +
			$"animationSpeed:{animationSpeed}");
	}

	/// <summary>
	/// Prints out all the Animator parameters.
	/// </summary>
	public void AnimatorDebug()
	{
		Debug.Log("ANIMATOR SETTINGS---------------------------");
		Debug.Log($"Moving:{animator.GetBool("Moving")}    Stunned:{animator.GetBool("Stunned")}     " +
			$"Blocking:{animator.GetBool("Blocking")}    Weapons:{animator.GetBool("Weapons")}     " +
			$"Action:{animator.GetInteger("Action")}    TriggerNumber:{animator.GetInteger("TriggerNumber")}    Velocity X:{animator.GetFloat("Velocity X")}     " +
			$"Velocity Z:{animator.GetFloat("Velocity Z")}");
	}

	#endregion
}