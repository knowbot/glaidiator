using Glaidiator.Model;
using UnityEngine;
using Input = Glaidiator.Model.Input;

namespace Glaidiator.Presenter
{
	public abstract class CharacterPresenter: MonoBehaviour
	{
		// Model
		protected Character Character;
		
		// Inputs
		protected Input inputs;
		protected IInputProvider provider;

		// View
		protected Transform Transform;
		[HideInInspector] public Animator animator;
		private static readonly int Moving = Animator.StringToHash("Moving");
		private static readonly int VelocityX = Animator.StringToHash("Velocity X");
		private static readonly int VelocityZ = Animator.StringToHash("Velocity Z");
		private static readonly int Action = Animator.StringToHash("Action");
		private static readonly int Trigger = Animator.StringToHash("Trigger");
		private static readonly int TriggerNumber = Animator.StringToHash("TriggerNumber");
		private static readonly int Blocking = Animator.StringToHash("Blocking");

		
		public Character GetCharacter()
		{
			return Character;
		}
		
		protected virtual void Start()
		{
			animator = GetComponentInChildren<Animator>();
			animator.applyRootMotion = false;
			Character.SetWorld(WorldObject.instance.World);
		}

		protected virtual void Awake()
		{
			Transform = transform;
			Character = new Character(Transform.position, Transform.rotation);
		}

		protected virtual void OnEnable()
		{
			// Register observer methods
			Character.onAlive += OnAlive;
			Character.onDeath += OnDeath;
			Character.onMoveTick += OnMoveTick;
			Character.onMoveEnd += OnMoveEnd;
			Character.onAttackStart += OnAttackStart;
			Character.onAttackEnd += OnAttackEnd;
			Character.onBlockStart += OnBlockStart;
			Character.onBlockEnd += OnBlockEnd;
			Character.onDodgeStart += OnDodgeStart;
			Character.onDodgeEnd += OnDodgeEnd;
		}

		protected virtual void OnDisable()
		{
			// Deregister observer methods
			Character.onAlive -= OnAlive;
			Character.onDeath -= OnDeath;
			Character.onMoveTick -= OnMoveTick;
			Character.onMoveEnd -= OnMoveEnd;
			Character.onAttackStart -= OnAttackStart;
			Character.onAttackEnd -= OnAttackEnd;
			Character.onBlockStart -= OnBlockStart;
			Character.onBlockEnd -= OnBlockEnd;
			Character.onDodgeStart -= OnDodgeStart;
			Character.onDodgeEnd -= OnDodgeEnd;
		}


		protected abstract void LateUpdate();

		// Observer methods
		private void OnAlive()
		{
			ResetTriggers();
			animator.SetInteger(Action, 0);
			animator.SetInteger(TriggerNumber, 0);
			animator.SetBool(Moving, false);
			animator.SetBool(Blocking, false);
			animator.SetFloat(VelocityX, 0);
			animator.SetFloat(VelocityZ, 0);
		}

		private void OnMoveTick()
		{
			animator.SetBool(Moving, true);
			animator.SetFloat(VelocityX, Transform.InverseTransformDirection(Character.Movement.CurrVelocity).x);
			animator.SetFloat(VelocityZ, Transform.InverseTransformDirection(Character.Movement.CurrVelocity).z);
		}

		private void OnMoveEnd()
		{
			animator.SetBool(Moving, false);
			animator.SetFloat(VelocityZ, 0);
		}

		private void OnAttackStart()
		{
			animator.SetInteger(Action, 1);
			SetTriggers();
		}
		
		private void OnAttackEnd()
		{
			animator.SetInteger(Action, 0);
			ResetTriggers();
		}

		private void OnBlockStart()
		{
			animator.SetBool(Blocking, true);
			SetTriggers();
		}
		
		private void OnBlockEnd()
		{
			animator.SetBool(Blocking, false);
			ResetTriggers();
		}
		
		private void OnDodgeStart()
		{
			animator.SetInteger(Action, 1);
			SetTriggers();
		}

		private void OnDodgeEnd()
		{
			animator.SetInteger(Action, 0);
			ResetTriggers();
		}
		
		private void OnDeath()
		{
			animator.SetTrigger(Trigger);
			animator.SetInteger(TriggerNumber, 9);
		}
		
		private void SetTriggers()
		{
			animator.SetTrigger(Trigger);
			animator.SetInteger(TriggerNumber, Character.ActiveAction!.Action.ID);
		}
		
		private void ResetTriggers()
		{
			animator.ResetTrigger(Trigger);
			animator.SetInteger(TriggerNumber, 0);
		}
	}
}