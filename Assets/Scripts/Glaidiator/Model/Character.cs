#nullable enable
using System;
using System.Collections.Generic;
using RPGCharacterAnims.Actions;
using UnityEngine;

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
	    private List<IHasCooldown> _cooldowns  = new List<IHasCooldown>();
	    private Attack? _activeAttack;
	    #endregion

	    #region Notifiers
		
	    public Action? onMove;
	    public Action? onStop;
	    
	    #endregion


	    public Character(Transform transform)
        {
	        Movement = new Movement(transform);
	        CurrentState = CharacterState.Idle;
	        
	        // init attacks
	        _attacks.Add("atkLight", new Attack(1.0f, 10, 0.0f));
	        _attacks.Add("atkHeavy", new Attack(2.0f, 25, 0.0f));
	        _attacks.Add("atkRanged", new Attack(1.0f, 10, 2.0f));
        }

        public void SetInputs(Input inputs)
        {
	        _inputs = inputs;
        }

        private void SetCanFlags(bool movement, bool action)
        {
	        CanMove = movement;
	        CanAction = action;
        }
        
        public override void Tick(float deltaTime)
        {
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
		        IHasCooldown? cd = _cooldowns[index];
		        if (cd.Tick(deltaTime))
			        _cooldowns.RemoveAt(index);
	        }

	        state.Tick(deltaTime);
        }

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
	        SetCanFlags(false, false);
	        if (_inputs.attackLight)
	        {
		        _activeAttack = _attacks["attackLight"];
	        }
	        else if (_inputs.attackHeavy)
	        {
		        _activeAttack = _attacks["attackHeavy"];
	        }
	        else if (_inputs.attackRanged)
	        {
		        _activeAttack = _attacks["attackRanged"];
	        }
	        else
	        {
		        Debug.LogError("Trying to enter Attack state with no attack input.");
	        }
        }
        
        private void Attack_Tick(float deltaTime)
        {
	       // do activeattack stuff
        }
        
        private void Attack_Exit()
        {
	        SetCanFlags(true, true);
	        if (_activeAttack != null) _cooldowns.Add(_activeAttack);
	        _activeAttack = null;
        }

        #endregion

        #endregion
        
        public void Block() { }
        public void Dodge() { }
        
        private void OnMove() => onMove?.Invoke();
        private void OnStop() => onStop?.Invoke();
    }
}