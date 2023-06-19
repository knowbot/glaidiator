using System;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.Model
{
    public class Movement
    {
	    private const float SPEED = 6f;
	    private const float DODGE_SPEED = 10f;
	    private const float ROT_SPEED = 100f;
	    public Vector3 LastDir = Direction.Get(Direction.Name.Up);
	    private Vector3 _position;

	    public Vector3 CurrVelocity { get; private set; }


	    public Movement(Vector3 position, Quaternion rotation)
	    {
		    Position = position;
		    Rotation = rotation;
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

	    private void Rotate(Vector3 dir, float deltaTime)
	    {
		    if (dir == Vector3.zero) return;
			Rotation = Quaternion.Slerp(Rotation, Quaternion.LookRotation(dir), deltaTime * ROT_SPEED);
	    }

	    public void Face(Vector3 dir)
	    {
		    Rotation = Quaternion.LookRotation(dir);
		    if (dir!= Vector3.zero) LastDir = dir;
	    }

	    public void Dodge(Vector3 dir, float deltaTime)
	    {
		    Rotate(dir, deltaTime);
		    CurrVelocity = dir * DODGE_SPEED;
		    Position += CurrVelocity * deltaTime;
		    if (dir!= Vector3.zero) LastDir = dir;
	    }
	    
	    public void Stop()
	    {
		    CurrVelocity = Vector3.zero;
	    }
    }
}