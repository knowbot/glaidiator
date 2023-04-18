using System;
using Glaidiator.Model.Utils;
using UnityEngine;

namespace Glaidiator.Model.Collision
{
    public class AABB : Collider2D
    {
        public Vector2 Size;
        public Vector2 Extents => Size / 2;
        public Vector2 Max => Center + Extents;
        public Vector2 Min => Center - Extents;
        public AABB(Vector2 center, Vector2 offset, bool isTrigger, Vector2 size) : 
            base(center, offset, isTrigger)
        {
            Size = size;
        }

        public bool Collision(AABB other)
        {
            return false;
        }

        public bool Collision(Circle other)
        {
            return false;
        }
        

        public override void WriteType()
        {
            Debug.Log("This is an AABB object");
        }

        public override void Draw()
        {
            Debug.Log("tryna draw a box");
            DebugUtils.DrawRect(Min.x0y(), Max.x0y(), Color.red);
        }
    }
}