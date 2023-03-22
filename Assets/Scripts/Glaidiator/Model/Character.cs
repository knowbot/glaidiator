using System;
using System.Collections.Generic;
using RPGCharacterAnims.Actions;
using UnityEngine;

namespace Glaidiator.Model
{
    public class Character : StateMachine
    {
	    private Input _inputs;
	    public Action onMove;
	    public Action onStop;
	    public bool CanMove = true;
	    public bool CanAction = true;
	    public bool IsMoving = false; 
	    private Timer _lock = new Timer(0f, 0f);

	    private enum CharacterState
	    {
		    Idle = 0,
		    Move = 1,
		    Attack = 2,
		    Block = 3,
		    Dodge = 4
	    }
	    
        public readonly CharacterMovement Movement;

        public Character(Transform transform)
        {
	        Movement = new CharacterMovement(transform);
	        CurrentState = CharacterState.Idle;
        }

        public void SetInputs(Input inputs)
        {
	        _inputs = inputs;
        }
        
        public override void Tick(float deltaTime)
        {
	        if (!_lock.Active())
	        {
		        if (CanAction && _inputs.attackLight)
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
	        state.Tick(deltaTime);
        }

        public void Lock(float delay, float duration)
        {
	        _lock.Set(delay, duration);
        }

        #region States

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

        public void Attack() {}
        public void Block() { }
        public void Dodge() { }
        
        private void OnMove() => onMove?.Invoke();
        private void OnStop() => onStop?.Invoke();
    }
}