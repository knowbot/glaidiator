using System;
using System.Collections.Generic;
using UnityEngine;

namespace Glaidiator.Model
{
    public class Character : StateMachine
    {
	    public Action onMove;
	    public Action onStop;
	    private bool _canMove = true;
	    private bool _canAction = true;
	    private Lock _lock = new Lock(0f, 0f);

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

        public override void Tick(float deltaTime)
        {
	        bool locked = _lock.Tick(deltaTime);
        }

        public void Move(Vector3 input, float deltaTime)
        {
	        if (_canMove && input != Vector3.zero)
	        {
		        Movement.Move(input, deltaTime);
		        Movement.Rotate(input, deltaTime);
	        }
        }

        public void Lock(float delay, float duration)
        {
	        _lock.SetLock(delay, duration);
        }

        #region State Methods
        private void Move_Enter()
        {
	        
        }
        
        private void Move_Tick(float deltaTime)
        {
	        
        }
        
        private void Move_Exit()
        {
	        
        }

        #endregion

        public void Attack() { }
        public void Block() { }
        public void Dodge() { }
        
        private void OnMove() => onMove?.Invoke();
        private void OnStop() => onStop?.Invoke();
    }
}