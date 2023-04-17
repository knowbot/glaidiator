using UnityEngine;

namespace Glaidiator.Model.Utils.Collision
{
    public class AABB : Collider2D
    {
        protected Bounds bounds;

        public AABB(Character owner, Vector2 center, Vector2 offset, ColliderType type, bool isTrigger, Vector2 size) : 
            base(owner, center, offset, type, isTrigger)
        {
            bounds = new Bounds(Center, size); 
        }
        
        public override bool Update()
        {
            throw new System.NotImplementedException();
        }

        public bool Collision(AABB other)
        {
            return false;
        }

        public bool Collision(Circle other)
        {
            return false;
        }
    }
}