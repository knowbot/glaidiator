#nullable enable
using System;
using System.Collections.Generic;
using Glaidiator.Model.Actions;
using Glaidiator.Model.Actions.Lookups;
using Glaidiator.Model.Resources;
using UnityEngine;
using Attack = Glaidiator.Model.Actions.Attack;

namespace Glaidiator.Model
{
    public class Character : StateMachine
    {
	    private enum CharacterState
	    {
		    Idling = 0,
		    Moving = 1,
		    Attacking = 2,
		    Blocking = 3,
		    Dodging = 4
	    }

	    #region Flags
	    
	    public bool CanMove = true;
	    public bool CanAction = true;

	    #endregion
	    
	    #region Attributes

	    private Enum _newState;
	    
	    public readonly Movement Movement;
	    public readonly Health Health;
	    public readonly Stamina Stamina;
	    
	    private Input _inputs;

	    private readonly Dictionary<string, AAction> _actions = new Dictionary<string, AAction>();
	    public readonly List<IHasCooldown> Cooldowns  = new List<IHasCooldown>();
	    public AAction? ActiveAction { get; private set; }
	    #endregion

	    #region Events

	    public Action? onLowStamina;
	    public Action? onMoveTick;
	    public Action? onMoveEnd;
	    public Action? onAttackStart;
	    // public Action? onAttackEnd;
	    public Action? onBlockStart;
	    public Action? onBlockEnd;
	    public Action? onDodgeStart;
	    public Action? onDodgeTick;
	    public Action? onDodgeEnd;

	    private void OnLowStamina() => onLowStamina?.Invoke();
	    private void OnMoveTick() => onMoveTick?.Invoke();
	    private void OnMoveEnd() => onMoveEnd?.Invoke();
	    private void OnAttackStart() => onAttackStart?.Invoke();
	    // private void OnAttackEnd() => onAttackEnd?.Invoke();
	    private void OnBlockStart() => onBlockStart?.Invoke();
	    private void OnBlockEnd() => onBlockEnd?.Invoke();
	    private void OnDodgeStart() => onDodgeStart?.Invoke();
	    private void OnDodgeTick() => onDodgeTick?.Invoke();
	    private void OnDodgeEnd() => onDodgeEnd?.Invoke();
	    
	    #endregion

	    #region Initialization

	    public Character(Transform transform)
	    {
		    _newState = state.current;
		    Movement = new Movement(transform);
		    Health = new Health(100.0f);
		    Stamina = new Stamina(100.0f, 0.05f);
		    CurrentState = CharacterState.Idling;

		    // init attacks
		    _actions.Add("atkLight", new Attack((int)ActionLookup.AttackLight, "Light Attack", 10f,  10f, false, false, 0.9f));
		    _actions.Add("atkHeavy", new Attack((int)ActionLookup.AttackHeavy, "Heavy Attack",25f, 20f, false, false, 1.8f, 3.3f));
		    _actions.Add("atkRanged", new Attack((int)ActionLookup.AttackRanged, "Ranged Attack",10f, 15f, false, false, 1.5f, 5.5f));
		    _actions.Add("block", new Block((int)ActionLookup.Block, "Block",10f, false, false,1.0f, 3.0f));
		    _actions.Add("dodge", new Dodge((int)ActionLookup.Dodge, "Dodge",25f,false, false,0.5f, 1.0f));
	    }
	    
	    #endregion

	    #region Checks

	    private bool HasEnoughStamina(AAction action)
	    {
		    return action.Cost <= Stamina.Current;
	    }

	    private bool IsOnCooldown<T>(object obj) where T : class, IHasCooldown
	    {
		    return Cooldowns.Contains((obj as T)!);
	    }

	    #endregion

	    #region Setters

	    public void SetInputs(Input inputs)
	    {
		    _inputs = inputs;
	    }

	    private void SetCanFlags(bool movement, bool action)
	    {
		    CanMove = movement;
		    CanAction = action;
	    }

	    private void SetActiveAction(AAction action)
	    {
		    Stamina.Subtract(action.Cost);
		    ActiveAction = action;
		    SetCanFlags(ActiveAction.CanMove, ActiveAction.CanAction);
		    ActiveAction.Start();
	    }
	    
	    private void ResetActiveAction()
	    {
		    ActiveAction = null;
		    SetCanFlags(true, true);
	    }

	    #endregion

	    #region Update

	    public override void Tick(float deltaTime)
	    {
		    // general system ticks
		    UpdateCooldowns(deltaTime);
		    UpdateActiveAction(deltaTime);
		    Stamina.Regen(deltaTime);

		    // init new state 
		    _newState = CurrentState;
		    
		    Idling();// if not doing anything and not moving

		    // if can move
		    if (CanMove) Moving();

		    // if can action
		    if(CanAction)
		    {
			    if (_inputs.attackLight || _inputs.attackHeavy || _inputs.attackRanged)
					Attacking();
			    else if (_inputs.block)
				    Blocking();
			    else if (_inputs.dodge)
				    Dodging();
		    }
		    // switch state
		    CurrentState = _newState;
		    state.Tick(deltaTime);
	    }
		/**
		 * make cooldowns tick!
		 */
	    private void UpdateCooldowns(float deltaTime)
	    {
		    for (int index = 0; index < Cooldowns.Count; index++)
		    {
			    IHasCooldown cd = Cooldowns[index];
			    if (!cd.Cooldown.Tick(deltaTime))
				    Cooldowns.RemoveAt(index);
		    }
	    }

		private void UpdateActiveAction(float deltaTime)
		{
			if (ActiveAction is not null && !ActiveAction.Tick(deltaTime)) ActiveAction = null;
		}

	    #endregion
	    
        #region States

        #region Idling
        
        private void Idling()
        {
	        if (ActiveAction is null) _newState = CharacterState.Idling;
        }

        #endregion

        #region Moving
        private void Moving()
        {
	        if (_inputs.move != Vector3.zero) _newState = CharacterState.Moving;
        }

        private void Moving_Enter()
        {
	       
        }
        
        private void Moving_Tick(float deltaTime)
        {
	        Movement.Move(_inputs.move, deltaTime);
	        OnMoveTick();
        }
        
        private void Moving_Exit()
        {
	        Movement.Stop();
	        OnMoveEnd();
        }

        #endregion

        #region Attacking
        
        private void Attacking()
        {
	        AAction? attack = null;
	        if (_inputs.attackLight)
	        {
		        attack = _actions["atkLight"];
	        }
	        else if (_inputs.attackHeavy)
	        {
		        attack = _actions["atkHeavy"];
	        }
	        else if (_inputs.attackRanged)
	        {
		        attack = _actions["atkRanged"];
	        }
	        if (attack is null || IsOnCooldown<Attack>(attack)) return;
	        if (!HasEnoughStamina(attack))
	        {
		        OnLowStamina();
		        return;
	        }
	        SetActiveAction(attack);
	        _newState = CharacterState.Attacking;
        }
		
        private void Attacking_Enter()
        {
	        if (ActiveAction is null or not Attack) return;
	        Cooldowns.Add((ActiveAction as Attack)!.SetOnCooldown());
	        OnAttackStart();
        }
        
        private void Attacking_Tick(float deltaTime)
        {
	       // do activeattack stuff
        }
        
        private void Attacking_Exit()
        {
	        ResetActiveAction();
        }

        #endregion
        
        #region Blocking

        private void Blocking()
        {
	        AAction block = _actions["block"];
	        if (IsOnCooldown<Block>(block)) return;
	        if (!HasEnoughStamina(block))
	        {
		        OnLowStamina();
		        return;
	        }
			SetActiveAction(block);
			_newState = CharacterState.Blocking;
        }
        private void Blocking_Enter()
        {
	        if (ActiveAction is null or not Block) return;
	        Cooldowns.Add((ActiveAction as Block)!.SetOnCooldown());
	        OnBlockStart();
        }
        
        private void Blocking_Tick(float deltaTime)
        {
        }
        
        private void Blocking_Exit()
        {
	        ResetActiveAction();
	        OnBlockEnd();
        }
        #endregion
        
        #region Dodging

        private void Dodging()
        {
	        AAction dodge = _actions["dodge"];
	        if (IsOnCooldown<Dodge>(dodge)) return;
	        if (!HasEnoughStamina(dodge))
	        {
		        OnLowStamina();
		        return;
	        }
	        SetActiveAction(dodge);
	        _newState = CharacterState.Dodging;
        }
        private void Dodging_Enter()
        {
	        if (ActiveAction is null or not Dodge) return;
	        (ActiveAction as Dodge)!.Direction = _inputs.move == Vector3.zero ? _inputs.move : Movement.LastDir;
	        Cooldowns.Add((ActiveAction as Dodge)!.SetOnCooldown());
	        OnDodgeStart();
        }
        
        private void Dodging_Tick(float deltaTime)
        {
	        Movement.Dodge(Movement.LastDir, deltaTime);
	        OnDodgeTick();
        }
        
        private void Dodging_Exit()
        {
	        ResetActiveAction();
	        OnDodgeEnd();
        }
        #endregion

        #endregion
    }
}