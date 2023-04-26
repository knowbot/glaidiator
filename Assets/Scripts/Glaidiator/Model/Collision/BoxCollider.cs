using System;
using Differ.Shapes;
using Glaidiator.Model.Utils;
using UnityEngine;

namespace Glaidiator.Model.Collision
{
    public class BoxCollider : Collider2D
    {
        public Vector2 Size;
        
        public override Shape Shape
        {
            get => (Polygon)shape;
            set => shape = (Polygon)value;
        }
        public BoxCollider(Vector2 position, Vector2 size, Vector2 offset, bool isTrigger) : 
            base(offset, isTrigger)
        {
            Size = size;
            shape = Polygon.rectangle(position.x, position.y, size.x, size.y);
        }

        public override void WriteType()
        {
       
        }

        public override void Draw()
        {
            DebugUtils.DrawRect(Position.x0y(), Quaternion.Euler(0, Rotation, 0), Size, Color.cyan);
        }
    }
}