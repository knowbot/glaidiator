using Glaidiator.Model;
using Glaidiator.Model.Utils;
using UnityEngine;

namespace Glaidiator.Presenter
{
	public class CharacterPresenter: MonoBehaviour
	{
		// Model
		protected Character Character;
		
		// Inputs
		protected Input inputs;
		protected IInputProvider provider;

		// View
		protected Transform _transform;
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
		}

		protected virtual void Awake()
		{
			_transform = transform;
			Character = new Character(WorldObject.instance.World, _transform.position, _transform.rotation);
			Character.Hitbox.Register();
		}

		protected virtual void OnEnable()
		{
			// Register observer methods
			Character.onDeath += OnDeath;
			Character.onMoveTick += OnMoveTick;
			Character.onMoveEnd += OnMoveEnd;
			Character.onAttackStart += OnAttackStart;
			Character.onBlockStart += OnBlockStart;
			Character.onBlockEnd += OnBlockEnd;
			Character.onDodgeStart += OnDodgeStart;
			Character.onDodgeEnd += OnDodgeEnd;
		}

		protected virtual void OnDisable()
		{
			// Deregister observer methods
			Character.onDeath -= OnDeath;
			Character.onMoveTick -= OnMoveTick;
			Character.onMoveEnd -= OnMoveEnd;
			Character.onAttackStart -= OnAttackStart;
			Character.onBlockStart -= OnBlockStart;
			Character.onBlockEnd -= OnBlockEnd;
			Character.onDodgeStart -= OnDodgeStart;
			Character.onDodgeEnd -= OnDodgeEnd;
		}

		
		protected virtual void Update()
		{
			// Process inputs and pass them onto the model
			inputs = provider.GetInputs();
			Character.SetInputs(inputs);
			// Advance the model
			Character.Tick(Time.deltaTime);
			_transform.position = Character.Movement.Position;
			_transform.rotation = Character.Movement.Rotation;
		}

		// Observer methods

		private void OnMoveTick()
		{
			animator.SetBool(Moving, true);
			animator.SetFloat(VelocityX, _transform.InverseTransformDirection(Character.Movement.CurrVelocity).x);
			animator.SetFloat(VelocityZ, _transform.InverseTransformDirection(Character.Movement.CurrVelocity).z);
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

		private void OnBlockStart()
		{
			animator.SetBool(Blocking, true);
			SetTriggers();
		}
		
		private void OnBlockEnd()
		{
			animator.SetBool(Blocking, false);
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