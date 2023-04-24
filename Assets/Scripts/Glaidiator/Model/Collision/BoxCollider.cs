using System;
using Differ.Shapes;
using Glaidiator.Model.Utils;
using UnityEngine;

namespace Glaidiator.Model.Collision
{
    public class BoxCollider : Collider2D
    {
        public override Shape Shape
        {
            get => (Polygon)shape;
            set => shape = (Polygon)value;
        }
        public BoxCollider(Vector2 position, Vector2 size, Vector2 offset, bool isTrigger) : 
            base(offset, isTrigger)
        {
            shape = Polygon.rectangle(position.x, position.y, size.x, size.y);
        }

        public override void WriteType()
        {
            Debug.Log("This is an box object");
        }

        public override void Draw()
        {
            Debug.Log("tryna draw a box");
        }
    }
}