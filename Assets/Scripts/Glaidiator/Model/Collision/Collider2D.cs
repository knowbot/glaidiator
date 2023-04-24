using System;
using Differ.Math;
using Differ.Shapes;
using Glaidiator.Model.Utils;
using Unity.VisualScripting;
using UnityEngine;

namespace Glaidiator.Model.Collision
{
    public abstract class Collider2D : ICloneable
    {
        protected Shape shape;
        public abstract Shape Shape { get ; set; }
        public Vector2 Offset;

        public Vector2 Position
        {
            get => new Vector2(Shape.x, Shape.y);
            set => Shape.position = new Vector(value.x + Offset.x, value.y + Offset.y);
        }
        
        public bool IsTrigger { get; private set; }

        protected Collider2D(Vector2 offset, bool isTrigger)
        {
            Offset = offset;
            IsTrigger = isTrigger;
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }
        
        public virtual void WriteType()
        {
            Console.WriteLine("This is a Collider2D object");
        }

        public virtual void Draw()
        {
            
        }
    }
}