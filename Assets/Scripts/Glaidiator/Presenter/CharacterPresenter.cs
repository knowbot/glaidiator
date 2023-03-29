using Glaidiator.Model;
using UnityEngine;

namespace Glaidiator.Presenter
{
	public class CharacterPresenter: MonoBehaviour
	{
		// Model
		protected Character Character;
		
		// Inputs
		private Input _inputs;
		public AInputProvider provider;

		// View
		private Transform _transform;
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
		}

		protected virtual void Awake()
		{
			_transform = transform;
			Character = new Character(_transform);
			animator = GetComponentInChildren<Animator>();
			animator.applyRootMotion = false;
		}

		protected virtual void OnEnable()
		{
			// Register observer methods
			Character.onMoveTick += OnMoveTick;
			Character.onMoveEnd += OnMoveEnd;
			Character.onAttackStart += OnAttackStart;
			Character.onBlockStart += OnBlockStart;
			Character.onBlockEnd += OnBlockEnd;
			Character.onDodgeStart += OnDodgeStart;
			Character.onDodgeTick += OnDodgeTick;
			Character.onDodgeEnd += OnDodgeEnd;
		}

		protected virtual void OnDisable()
		{
			// Deregister observer methods
			Character.onMoveTick -= OnMoveTick;
			Character.onMoveEnd -= OnMoveEnd;
			Character.onAttackStart -= OnAttackStart;
			Character.onBlockStart -= OnBlockStart;
			Character.onBlockEnd -= OnBlockEnd;
			Character.onDodgeStart -= OnDodgeStart;
			Character.onDodgeTick -= OnDodgeTick;
			Character.onDodgeEnd -= OnDodgeEnd;
		}

		
		protected virtual void Update()
		{
			// Process inputs and pass them onto the model
			_inputs = provider.GetInputs();
			Character.SetInputs(_inputs);
			// Advance the model
			Character.Tick(Time.deltaTime);
		}

		// Observer methods

		private void OnMoveTick()
		{
			_transform.position = Character.Movement.Position;
			_transform.rotation = Character.Movement.Rotation;
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
		
		private void OnDodgeTick()
		{
			_transform.position = Character.Movement.Position;
			_transform.rotation = Character.Movement.Rotation;
		}
		
		private void OnDodgeEnd()
		{
			animator.SetInteger(Action, 0);
			ResetTriggers();
		}
		
		private void SetTriggers()
		{
			animator.SetTrigger(Trigger);
			animator.SetInteger(TriggerNumber, Character.ActiveAction!.ID);
		}
		
		private void ResetTriggers()
		{
			animator.ResetTrigger(Trigger);
			animator.SetInteger(TriggerNumber, 0);
		}
	}
}