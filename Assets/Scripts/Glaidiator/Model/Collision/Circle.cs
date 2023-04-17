using Glaidiator.Model.Utils;
using UnityEngine;

namespace Glaidiator.Model.Collision
{
    public class Circle : Collider2D
    {
        public float Radius { get; }
        public Circle(Vector2 center, Vector2 offset, bool isTrigger, float radius) : 
            base(center, offset, isTrigger)
        {
            Radius = radius;
        }
        
        public override void WriteType()
        {
            Debug.Log("This is a Circle object");
        }

    
        public override void Draw()
        {
            DebugUtils.DrawCircle(Center.x0y(), Radius, 20, Color.yellow);
        }
    }
}