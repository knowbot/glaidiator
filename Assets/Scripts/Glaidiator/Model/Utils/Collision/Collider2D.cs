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
        private World _world;
        protected Character Owner;
        public Vector2 Center;
        public ColliderType Type { get; }
        public bool IsTrigger { get; }

        protected Collider2D(Character owner, Vector2 center, ColliderType type, bool isTrigger)
        {
            Owner = owner;
            Center = center;
            Type = type;
            IsTrigger = isTrigger;
        }

        public abstract bool Update();

        public void Destroy()
        {
            _world.RemoveCollider(this);
        }
    }
}