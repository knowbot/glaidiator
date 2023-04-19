using System;
using Glaidiator.Model.Utils;
using Unity.VisualScripting;
using UnityEngine;

namespace Glaidiator.Model.Collision
{
    public abstract class Collider2D : ICloneable
    {
        public Vector2 Center;
        public Vector2 Offset;
        public bool IsTrigger { get; private set; }

        protected Collider2D(Vector2 center, Vector2 offset, bool isTrigger)
        {
            Center = center;
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

        public abstract void Draw();
    }
}