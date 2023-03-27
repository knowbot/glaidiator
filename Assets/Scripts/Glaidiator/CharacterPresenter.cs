using System.Linq;
using Glaidiator.Model;
using Glaidiator.Model.Actions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Glaidiator
{
	public class CharacterPresenter: MonoBehaviour
	{
		// Model
		private Model.Character _character;
		
		// Inputs
		private Input _inputs;
		public TextMeshProUGUI displayState;
		public TextMeshProUGUI displayCooldowns;
		public AInputProvider Provider;

		// View
		[HideInInspector] private Transform _transform;
		[HideInInspector] public Animator animator;
		private static readonly int Moving = Animator.StringToHash("Moving");
		private static readonly int VelocityX = Animator.StringToHash("Velocity X");
		private static readonly int VelocityZ = Animator.StringToHash("Velocity Z");
		private static readonly int Action = Animator.StringToHash("Action");
		private static readonly int Trigger = Animator.StringToHash("Trigger");
		private static readonly int TriggerNumber = Animator.StringToHash("TriggerNumber");
		private static readonly int Blocking = Animator.StringToHash("Blocking");


		private void Awake()
		{
			_transform = transform;
			_character = new Model.Character(_transform);
			animator = GetComponentInChildren<Animator>();
			animator.applyRootMotion = false;
		}

		private void OnEnable()
		{
			// Register observer methods
			_character.onMove += OnMove;
			_character.onStop += OnStop;
			_character.onAttackStart += OnAttackStart;
			_character.onBlockStart += OnBlockStart;
			_character.onBlockEnd += OnBlockEnd;
		}

		private void OnDisable()
		{
			// Deregister observer methods
			_character.onMove -= OnMove;
			_character.onStop -= OnStop;
			_character.onAttackStart -= OnAttackStart;
			_character.onBlockStart -= OnBlockStart;
			_character.onBlockEnd -= OnBlockEnd;
		}

		
		private void Update()
		{
			// Process inputs and pass them onto the model
			_inputs = Provider.GetInputs();
			_character.SetInputs(_inputs);
			// Advance the model
			_character.Tick(Time.deltaTime);
			displayState.text = _character.CurrentState.ToString();
			displayCooldowns.text = "";
			foreach (IHasCooldown cd in _character.Cooldowns.OrderBy(cd => cd.Name))
			{
				displayCooldowns.text += cd.Name + ": " + cd.Cooldown.Duration.ToString("0.00") + "\n";
			}
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


		private void SetTriggers()
		{
			animator.SetTrigger(Trigger);
			animator.SetInteger(TriggerNumber, _character.ActiveAction!.ID);
		}
	}
}