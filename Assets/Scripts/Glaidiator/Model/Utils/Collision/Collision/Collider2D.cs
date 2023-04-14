using UnityEngine;

namespace Glaidiator.Model.Utils.Collision
{
    public enum ColliderType
    {
        Environment = 0,
        Body = 1,
        Attack = 2,
        Block = 3
    }
    public abstract class Collider2D
    {
        protected Character Owner;
        protected Vector2 Center;
        protected ColliderType Type;
        public bool IsAttached { get; private set; }

        protected Collider2D(Character owner, Vector2 center, ColliderType type, bool isAttached)
        {
            Owner = owner;
            Center = center;
            Type = type;
        }

        public abstract bool Update();
    }
}