using System;
using RPGCharacterAnims.Actions;
using UnityEngine;

namespace Character.Model
{
    public class CharacterModel
    {
	    public Action onMove;

	    private bool _canMove;
	    private bool _canAction;
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

        public void Move(Vector3 input, float timeStep)
        {
	        if (_canMove && input != Vector3.zero)
	        {
		        Transform.Move(input, timeStep);
	        }
        }

        public void Attack() { }
        

    }
}