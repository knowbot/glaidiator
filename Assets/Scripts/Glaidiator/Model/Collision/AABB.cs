using System;
using Glaidiator.Model.Utils;
using UnityEngine;

namespace Glaidiator.Model.Collision
{
    public class AABB : Collider2D
    {
        protected Bounds bounds;

        public AABB(Vector2 center, Vector2 offset, bool isTrigger, Vector2 size) : 
            base(center, offset, isTrigger)
        {
            bounds = new Bounds(Center, size); 
        }

        public bool Collision(AABB other)
        {
            return false;
        }

        public bool Collision(Circle other)
        {
            return false;
        }

        public override object Clone()
        {
            var clone = (AABB)MemberwiseClone();
            clone.bounds = new Bounds(bounds.Center, bounds.Size);
            return clone;
        }

        public override void WriteType()
        {
            Debug.Log("This is an AABB object");
        }

        public override void Draw()
        {
            DebugUtils.DrawRect(bounds.Min.x0y(), bounds.Max.x0y(), Color.green);
        }
    }
}