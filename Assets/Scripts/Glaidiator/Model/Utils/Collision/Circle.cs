using UnityEngine;

namespace Glaidiator.Model.Utils.Collision
{
    public class Circle : Collider2D
    {
        public float Radius { get; }
        public Circle(Character owner, Vector2 center, Vector2 offset, ColliderType type, bool isTrigger, float radius) : 
            base(owner, center, offset, type, isTrigger)
        {
            Radius = radius;
        }

        public override bool Update()
        {
            throw new System.NotImplementedException();
        }
    }
}