using System;
using UnityEngine;

namespace Glaidiator.Model
{
    public class Movement
    {
	    private const float SPEED = 6f;
	    private const float DODGE_SPEED = 10f;
	    private const float ROT_SPEED = 100f;

	    public Vector3 CurrVelocity { get; private set; }


	    public Movement(Transform transform)
	    {
		    Position = transform.position;
		    Rotation = transform.rotation;
	    }
	    public Vector3 Position { get; private set; }

	    public Quaternion Rotation { get; private set; }


	    public void Move(Vector3 dir, float deltaTime)
	    {
		    Rotate(dir, deltaTime);
			CurrVelocity = dir * SPEED;
			Position += CurrVelocity * deltaTime;
		}

	    public void Rotate(Vector3 dir, float deltaTime)
		{
			Rotation = Quaternion.Slerp(Rotation, Quaternion.LookRotation(dir), deltaTime * ROT_SPEED);
		}

	    public void Dodge(Vector3 dir, float deltaTime)
	    {
		    
	    }
	    
	    public void Stop()
	    {
		    CurrVelocity = Vector3.zero;
	    }
    }
}