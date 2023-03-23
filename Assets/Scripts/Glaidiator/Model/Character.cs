#nullable enable
using System;
using System.Collections.Generic;
using Glaidiator.Model.Actions;
using RPGCharacterAnims.Actions;
using UnityEngine;
using Attack = Glaidiator.Model.Actions.Attack;

namespace Glaidiator.Model
{
    public class Character : StateMachine
    {
	    private enum CharacterState
	    {
		    Idle = 0,
		    Move = 1,
		    Attack = 2,
		    Block = 3,
		    Dodge = 4
	    }

	    #region Flags
	    
	    public bool CanMove = true;
	    public bool CanAction = true;
	    public bool IsMoving = false; 
	    
	    #endregion
	    
	    #region Attributes
	    
	    public readonly Movement Movement;
	    
	    private Input _inputs;
	    
	    private Timer _lock = new Timer(0f, 0f);

	    private readonly Dictionary<string, Attack> _attacks = new Dictionary<string, Attack>();
	    private List<AHasCooldown> _cooldowns  = new List<AHasCooldown>();
	    private IAction? _activeAction;
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
		    CurrentState = CharacterState.Idle;
	        
		    // init attacks
		    _attacks.Add("atkLight", new Attack(10f, 1.0f, false, false));
		    _attacks.Add("atkHeavy", new Attack(25f, 2.0f, false, false));
		    _attacks.Add("atkRanged", new Attack(10f, 1.0f, false, false, new CooldownData(1.0f)));
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
		    if(!CanAction && _activeAction.)
		    if (!_lock.Tick(deltaTime))
		    {
			    if (CanAction && (_inputs.attackLight || _inputs.attackHeavy || _inputs.attackRanged))
				    CurrentState = CharacterState.Attack;
			    else if (CanMove && _inputs.move != Vector3.zero)
			    {
				    CurrentState = CharacterState.Move;
			    }
			    else
			    {
				    CurrentState = CharacterState.Idle;
			    }
		    }

		    for (int index = 0; index < _cooldowns.Count; index++)
		    {
			    AHasCooldown? cd = _cooldowns[index];
			    if (cd.Tick(deltaTime))
				    _cooldowns.RemoveAt(index);
		    }

		    state.Tick(deltaTime);
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
	        if (_activeAction is Attack a && a.HasCooldown()) _cooldowns.Add(a.SetOnCooldown());
	        _activeAction = null;
        }

        #endregion

        #endregion
    }
}