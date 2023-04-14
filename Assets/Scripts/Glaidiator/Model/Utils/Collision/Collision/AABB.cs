using UnityEngine;

namespace Glaidiator.Model.Utils.Collision
{
    public class AABB : Collider2D
    {
        protected Bounds bounds;

        public AABB(Character owner, Vector2 center, ColliderType type, bool isAttached, Vector2 size) : 
            base(owner, center, type, isAttached)
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