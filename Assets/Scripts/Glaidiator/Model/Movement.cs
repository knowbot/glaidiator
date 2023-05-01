using System;
using UnityEngine;

namespace Glaidiator.Model
{
    public class Movement
    {
	    private const float SPEED = 6f;
	    private const float DODGE_SPEED = 10f;
	    private const float ROT_SPEED = 100f;
	    public Vector3 LastDir;
	    private Vector3 _position;

	    public Vector3 CurrVelocity { get; private set; }


	    public Movement(Transform transform)
	    {
		    Position = transform.position;
		    Rotation = transform.rotation;
	    }

	    public Vector3 Position
	    {
		    get => _position;
		    private set => _position = new Vector3(Math.Clamp(value.x, 0f, Arena.Size.x), 0, Math.Clamp(value.z, 0f, Arena.Size.y));
	    }

	    public Quaternion Rotation { get; private set; }
	    
	    public void Move(Vector3 dir, float deltaTime)
	    {
		    Rotate(dir, deltaTime);
			CurrVelocity = dir * SPEED;
			Position += CurrVelocity * deltaTime;
			if (dir!= Vector3.zero) LastDir = dir;
	    }
	    

	    public void Rotate(Vector3 dir, float deltaTime)
		{
			Rotation = Quaternion.Slerp(Rotation, Quaternion.LookRotation(dir), deltaTime * ROT_SPEED);
		}

	    public void Dodge(Vector3 dir, float deltaTime)
	    {
		    Rotate(dir, deltaTime);
		    CurrVelocity = dir * DODGE_SPEED;
		    Position += CurrVelocity * deltaTime;
	    }
	    
	    public void Stop()
	    {
		    CurrVelocity = Vector3.zero;
	    }
    }
}