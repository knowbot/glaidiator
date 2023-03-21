using System;
using UnityEngine;

namespace Glaidiator.Model
{
    public class CharacterMovement
    {
	    public Action onPositionChanged;
	    public Action onRotationChanged;
	    private const float SPEED = 6f;
	    private const float ROT_SPEED = 100f;
	    
	    private Vector3 _position;
	    private Quaternion _rotation;
	    private Vector3 _currVelocity;


	    public CharacterMovement(Transform transform)
	    {
		    Position = transform.position;
		    Rotation = transform.rotation;
	    }
	    public Vector3 Position { 
		    get => _position;
		    set
		    {
			    _position = value;
			    OnPositionChanged();
		    }
	    }
	    public Quaternion Rotation { 
		    get => _rotation;
		    set
		    {
			    _rotation = value;
			    OnRotationChanged();
		    }
	    }
	    

	    public void Move(Vector3 dir, float deltaTime)
		{
			Debug.Log("Tryna run!");
			_currVelocity = dir * SPEED;
			Position += _currVelocity * deltaTime;
		}

	    public void Rotate(Vector3 dir, float deltaTime)
		{
			Rotation = Quaternion.Slerp(Rotation, Quaternion.LookRotation(dir), deltaTime * ROT_SPEED);
		}
	    
	    public void Stop()
	    {
		    _currVelocity = Vector3.zero;
	    }

		private void OnPositionChanged() => onPositionChanged?.Invoke();
		private void OnRotationChanged() => onRotationChanged?.Invoke();
    }
}