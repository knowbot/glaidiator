#nullable enable
using System;
using System.Collections.Generic;
using Glaidiator.Model.Actions;
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
	    private AAction? _activeAction;
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
		    _attacks.Add("atkLight", new Attack(10f, 1.0f, false, false));
		    _attacks.Add("atkHeavy", new Attack(25f, 2.0f, false, false));
		    _attacks.Add("atkRanged", new Attack(10f, 1.0f, false, false, 1.0f));
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
		    var newState = CharacterState.Idling;
		    
		    // if active action is done, set it to null
		    if (_activeAction is not null && !_activeAction.Tick(deltaTime)) _activeAction = null;
		    
		    if (CanMove && _inputs.move != Vector3.zero) newState = CharacterState.Moving;

		    if(CanAction)
		    {
			    if (_inputs.attackLight || _inputs.attackHeavy || _inputs.attackRanged)
				    newState = CharacterState.Attacking;
			    else if (_inputs.block)
				    newState = CharacterState.Blocking;
			    else if (_inputs.dodge)
				    newState = CharacterState.Dodging;
		    }
		    
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
			    if (cd.Cooldown.Tick(deltaTime))
				    _cooldowns.RemoveAt(index);
		    }
	    }

	    #endregion
	    
        #region States

        #region Move

        private void Move_Enter()
        {
	        IsMoving = true;
        }
        
        private void Move_Tick(float deltaTime)
        {
	        Movement.Move(_inputs.move, deltaTime);
	        Movement.Rotate(_inputs.move, deltaTime);
	        OnMove();
        }
        
        private void Move_Exit()
        {
	        IsMoving = false;
	        Movement.Stop();
	        OnStop();
        }

        #endregion

        #region Attack
		
        private void Attack_Enter()
        {
	        if (_inputs.attackLight)
	        {
		        _activeAction = _attacks["attackLight"];
	        }
	        else if (_inputs.attackHeavy)
	        {
		        _activeAction = _attacks["attackHeavy"];
	        }
	        else if (_inputs.attackRanged)
	        {
		        _activeAction = _attacks["attackRanged"];
	        }
	        else
	        {
		        Debug.LogError("Trying to enter Attack state with no attack input.");
		        return;
	        }
	        SetCanFlags(_activeAction.CanMove, _activeAction.CanAction);
	        OnAttack();
        }
        
        private void Attack_Tick(float deltaTime)
        {
	       // do activeattack stuff
        }
        
        private void Attack_Exit()
        {
	        SetCanFlags(true, true);
	        // safe downcast to Attack type to put it on cooldown
	        if (_activeAction is Attack { Cooldown: { } } a) _cooldowns.Add(a.SetOnCooldown());
	        _activeAction = null;
        }

        #endregion

        #endregion
    }
}