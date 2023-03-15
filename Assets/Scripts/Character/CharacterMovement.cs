using System;
using UnityEngine;

namespace Character
{
    public class CharacterMovement
    {
	    private readonly float _speed = 6f;
		private float _rotSpeed = 100f;
		private Vector3 _currVelocity;

		public Vector3 Move(Vector3 move, float deltaTime)
		{
			_currVelocity = move * _speed;
			return _currVelocity * deltaTime;
		}
    }
}