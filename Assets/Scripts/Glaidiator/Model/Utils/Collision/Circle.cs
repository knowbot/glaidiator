using UnityEngine;

namespace Glaidiator.Model.Utils.Collision
{
    public class Circle : Collider2D
    {
        public float Radius { get; }
        public Vector2 Offset { get; }
        public Circle(Character owner, Vector2 center, ColliderType type, bool isTrigger, float radius, Vector2 offset) : 
            base(owner, center, type, isTrigger)
        {
            Radius = radius;
            Offset = offset;
        }

        public override bool Update()
        {
            throw new System.NotImplementedException();
        }
    }
}