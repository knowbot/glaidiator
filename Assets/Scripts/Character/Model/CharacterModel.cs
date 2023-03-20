using System;
using JetBrains.Annotations;
using RPGCharacterAnims.Actions;
using UnityEngine;

namespace Character.Model
{
    public class CharacterModel
    {
	    public Action onMove;

	    private bool _canMove;
	    private bool _canAction;
	    private Lock _lock = new Lock(0f, 0f);
	    public enum State
	    {
		    Idle = 0,
		    Move = 1,
		    Attack = 2,
		    Block = 3,
		    Dodge = 4
	    }
	    
        public readonly CharacterTransform Transform;
        public State state = State.Idle;

        public CharacterModel(Transform transform)
        {
	        Transform = new CharacterTransform(transform);
        }

        public void Update(float deltaTime)
        {
	        bool locked = _lock.Tick(deltaTime);
        }

        public void Move(Vector3 input, float deltaTime)
        {
	        if (_canMove && input != Vector3.zero)
	        {
		        Transform.Move(input, deltaTime);
	        }
        }

        public void Lock(float delay, float duration)
        {
	        _lock.SetLock(delay, duration);
        }
        

        public void Attack() { }
        public void Block() { }
        public void Dodge() { }
    }
}