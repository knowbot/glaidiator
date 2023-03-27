#nullable enable
using System;
using System.Collections.Generic;
using Glaidiator.Model.Actions;
using Glaidiator.Model.Actions.Lookups;
using RPGCharacterAnims.Actions;
using Unity.VisualScripting;
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
	    public bool IsMoving = false; 
	    
	    #endregion
	    
	    #region Attributes
	    
	    public readonly Movement Movement;
	    
	    private Input _inputs;

	    private readonly Dictionary<string, Attack> _attacks = new Dictionary<string, Attack>();
	    private readonly List<IHasCooldown> _cooldowns  = new List<IHasCooldown>();
	    public AAction? ActiveAction { get; private set; }
	    #endregion

	    #region Actions
		
	    public Action? onMove;
	    public Action? onStop;
	    public Action? onAttack;
	    
	    private void OnMove() => onMove?.Invoke();
	    private void OnStop() => onStop?.Invoke();
	    private void OnAttack() => onAttack?.Invoke();
	    
	    #endregion

	    #region Initialization

	    public Character(Transform transform)
	    {
		    Movement = new Movement(transform);
		    CurrentState = CharacterState.Idling;
	        
		    // init attacks
		    _attacks.Add("atkLight", new Attack((int)ActionLookup.AttackLight, 10f, 1.0f, false, false));
		    _attacks.Add("atkHeavy", new Attack((int)ActionLookup.AttackHeavy, 25f, 2.0f, false, false, 1.5f));
		    _attacks.Add("atkRanged", new Attack((int)ActionLookup.AttackRanged, 10f, 1.5f, false, false, 4.0f));
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
		    Cooldowns(deltaTime);

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
	    private void Cooldowns(float deltaTime)
	    {
		    for (int index = 0; index < _cooldowns.Count; index++)
		    {
			    IHasCooldown cd = _cooldowns[index];
			    if (!cd.Cooldown.Tick(deltaTime))
				    _cooldowns.RemoveAt(index);
		    }
	    }

	    #endregion
	    
        #region States

        #region Moving

        private void Moving_Enter()
        {
	        IsMoving = true;
        }
        
        private void Moving_Tick(float deltaTime)
        {
	        Movement.Move(_inputs.move, deltaTime);
	        Movement.Rotate(_inputs.move, deltaTime);
	        OnMove();
        }
        
        private void Moving_Exit()
        {
	        IsMoving = false;
	        Movement.Stop();
	        OnStop();
        }

        #endregion

        #region Attacking
		
        private void Attacking_Enter()
        {
	        Attack attack;
	        if (_inputs.attackLight)
	        {
		        attack = _attacks["atkLight"];
	        }
	        else if (_inputs.attackHeavy)
	        {
		        attack = _attacks["atkHeavy"];
	        }
	        else if (_inputs.attackRanged)
	        {
		        attack = _attacks["atkRanged"];
	        }
	        else
	        {
		        Debug.LogError("Trying to enter Attack state with no attack input.");
		        return;
	        }

	        if (attack is null || _cooldowns.Contains(attack)) return;
	        SetCanFlags(attack.CanMove, attack.CanAction);
	        ActiveAction = attack;
	        ActiveAction.Start();
	        OnAttack();
        }
        
        private void Attacking_Tick(float deltaTime)
        {
	       // do activeattack stuff
        }
        
        private void Attacking_Exit()
        {
	        SetCanFlags(true, true);
	        // safe downcast to Attack type to put it on cooldown
	        if (ActiveAction is Attack a) _cooldowns.Add(a.SetOnCooldown());
	        ActiveAction = null;
        }

        #endregion

        #endregion
    }
}