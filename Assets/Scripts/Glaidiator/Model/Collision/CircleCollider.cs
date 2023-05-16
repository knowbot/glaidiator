using Differ.Shapes;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.Model.Collision
{
    public class CircleCollider : Collider2D
    {
        public override Shape Shape
        {
            get => (Circle)shape;
            set => shape = (Circle)value;
        }

        public float Radius => ((Circle)Shape).radius;

        public CircleCollider(Vector2 position, float radius, Vector2 offset, bool isTrigger) : 
            base(offset, isTrigger)
        {
            shape = new Circle(position.x, position.y, radius);
        }
        
        public override void WriteType()
        {
            
        }

    
        public override void Draw()
        {
            DebugStuff.DrawCircle(Position.x0y(), Radius, 20, Color.cyan);
        }
    }
}