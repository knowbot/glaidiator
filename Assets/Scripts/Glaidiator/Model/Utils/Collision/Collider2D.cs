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
        public Vector2 Offset;
        public ColliderType Type { get; }
        public bool IsTrigger { get; }

        protected Collider2D(Character owner, Vector2 center, Vector2 offset, ColliderType type, bool isTrigger)
        {
            Owner = owner;
            Center = center;
            Offset = offset;
            Type = type;
            IsTrigger = isTrigger;
        }

        public abstract bool Update();

        public void Register()
        {
            _world.AddCollider(this);
        }
        
        public void Deregister()
        {
            _world.RemoveCollider(this);
        }
    }
}