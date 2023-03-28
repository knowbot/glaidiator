#nullable enable
using System;
using System.Collections.Generic;
using Glaidiator.Model.Actions;
using Glaidiator.Model.Actions.Lookups;
using RPGCharacterAnims.Actions;
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
	    
	    public readonly Movement Movement;
	    
	    private Input _inputs;

	    private readonly Dictionary<string, AAction> _actions = new Dictionary<string, AAction>();
	    public readonly List<IHasCooldown> Cooldowns  = new List<IHasCooldown>();
	    public AAction? ActiveAction { get; private set; }
	    #endregion

	    #region Actions
		
	    public Action? onMove;
	    public Action? onStop;
	    public Action? onAttackStart;
	    public Action? onAttackEnd;
	    public Action? onBlockStart;
	    public Action? onBlockEnd;
	    public Action? onDodgeStart;
	    public Action? onDodgeEnd;
	    
	    private void OnMove() => onMove?.Invoke();
	    private void OnStop() => onStop?.Invoke();
	    private void OnAttackStart() => onAttackStart?.Invoke();
	    private void OnAttackEnd() => onAttackEnd?.Invoke();
	    private void OnBlockStart() => onBlockStart?.Invoke();
	    private void OnBlockEnd() => onBlockEnd?.Invoke();
	    private void OnDodgeStart() => onDodgeStart?.Invoke();
	    private void OnDodgeEnd() => onDodgeEnd?.Invoke();
	    
	    #endregion

	    #region Initialization

	    public Character(Transform transform)
	    {
		    Movement = new Movement(transform);
		    CurrentState = CharacterState.Idling;
	        
		    // init attacks
		    _actions.Add("atkLight", new Attack((int)ActionLookup.AttackLight, "Light Attack", 10f,  10, false, false, 0.9f));
		    _actions.Add("atkHeavy", new Attack((int)ActionLookup.AttackHeavy, "Heavy Attack",25f, 20, false, false, 1.8f, 3.3f));
		    _actions.Add("atkRanged", new Attack((int)ActionLookup.AttackRanged, "Ranged Attack",10f, 15, false, false, 1.5f, 5.5f));
		    _actions.Add("block", new Block((int)ActionLookup.Block, "Block",10, false, false,1.0f, 3.0f));
		    _actions.Add("dodge", new Dodge((int)ActionLookup.Dodge, "Dodge",25,false, false,0.5f, 1.0f));
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

	    #endregion

	    #region Update

	    public override void Tick(float deltaTime)
	    {
		    // update cooldowns
		    UpdateCooldowns(deltaTime);

		    // init new state 
		    Enum newState = CurrentState;

		    // if not doing anything and not moving
		    if (ActiveAction is null || !ActiveAction.Tick(deltaTime))
		    {
			    newState = CharacterState.Idling;
		    }

		    // if can move and there is an input
		    if (CanMove && _inputs.move != Vector3.zero) newState = CharacterState.Moving;

		    // if can action
		    if(CanAction)
		    {
			    // if there is an input
			    if (_inputs.attackLight || _inputs.attackHeavy || _inputs.attackRanged)
				    newState = CharacterState.Attacking;
			    else if (_inputs.block)
				    newState = CharacterState.Blocking;
			    else if (_inputs.dodge)
				    newState = CharacterState.Dodging;
		    }
		    // need to only switch state once to avoid calling enter/exit methods uselessly
		    CurrentState = newState;
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

	    #endregion
	    
        #region States

        #region Moving

        private void Moving_Enter()
        {
	       
        }
        
        private void Moving_Tick(float deltaTime)
        {
	        Movement.Move(_inputs.move, deltaTime);
	        OnMove();
        }
        
        private void Moving_Exit()
        {
	        Movement.Stop();
	        OnStop();
        }

        #endregion

        #region Attacking
		
        private void Attacking_Enter()
        {
	        AAction attack;
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
	        else
	        {
		        Debug.LogError("Trying to enter Attack state with no attack input.");
		        return;
	        }

	        if (attack is null || Cooldowns.Contains((attack as Attack)!)) return;
	        ActiveAction = attack;
	        SetCanFlags(ActiveAction.CanMove, ActiveAction.CanAction);
	        ActiveAction.Start();
	        Cooldowns.Add(((ActiveAction as Attack)!).SetOnCooldown());
	        OnAttackStart();
        }
        
        private void Attacking_Tick(float deltaTime)
        {
	       // do activeattack stuff
        }
        
        private void Attacking_Exit()
        {
	        SetCanFlags(true, true);
	        ActiveAction = null;
        }

        #endregion
        
        #region Blocking
        private void Blocking_Enter()
        {
	        if (Cooldowns.Contains((_actions["block"] as Block)!)) return;
	        ActiveAction = _actions["block"];
	        SetCanFlags(ActiveAction.CanMove, ActiveAction.CanAction);
	        ActiveAction.Start();
	        Cooldowns.Add(((ActiveAction as Block)!).SetOnCooldown());
	        OnBlockStart();
        }
        
        private void Blocking_Tick(float deltaTime)
        {
        }
        
        private void Blocking_Exit()
        {
	        SetCanFlags(true, true);
	        ActiveAction = null;
	        OnBlockEnd();
        }
        #endregion
        
        #region Dodging
        private void Dodging_Enter()
        {
	        // SET ACTION
	        if (Cooldowns.Contains((_actions["dodge"] as Dodge)!)) return;
	        ActiveAction = _actions["dodge"];
	        SetCanFlags(ActiveAction.CanMove, ActiveAction.CanAction);
	        // DO LOGIC
	        (ActiveAction as Dodge)!.Direction = (_inputs.move == Vector3.zero) ? _inputs.move : Movement.LastDir;
	        // SET OFF TIMERS
	        ActiveAction.Start();
	        Cooldowns.Add(((ActiveAction as Dodge)!).SetOnCooldown());
	        // CALL OBSERVER METHOD
	        OnDodgeStart();
        }
        
        private void Dodging_Tick(float deltaTime)
        {
	        Debug.Log("DODGING");
	        Movement.Dodge(Movement.LastDir, deltaTime);
	        OnMove();
        }
        
        private void Dodging_Exit()
        {
	        SetCanFlags(true, true);
	        ActiveAction = null;
	        OnDodgeEnd();
	        OnStop();
        }
        #endregion

        #endregion
    }
}