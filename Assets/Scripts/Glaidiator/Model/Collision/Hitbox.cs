using System;
using System.Numerics;
using Glaidiator.Model.Actions;
using Glaidiator.Model.Utils;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Glaidiator.Model.Collision
{
    public class Hitbox<T> : IHitbox, ICloneable
    {
        private World _world;
        private readonly Collider2D _collider;
        private readonly float _lifetime;

        public Collider2D Collider { get; private set; }
        public Character Owner { get; private set; }
        public T Origin { get; internal set; }
        public Timer Lifetime { get; private set; }
        public Vector2 Direction = Vector2.zero;

        public Hitbox(Collider2D collider, Character owner, float lifetime = 0f)
        {
            _world = World.instance;
            _collider = collider;
            Collider = (Collider2D)_collider.Clone();
            Owner = owner;
            _lifetime = lifetime;
            if(_lifetime > 0f) Lifetime = new Timer(_lifetime);
        }

        public void Create()
        {
            Collider.Center = Owner.Movement.Position.xz();
            _world.Add((Hitbox<T>)Clone());
        }
        
        public void Destroy()
        {
            Collider = null;
            _world.Remove(this);
        }
        
        public virtual void Update(float deltaTime)
        {
            // when lifetime is over, reset collider and deregister
            if (Lifetime is not null && Lifetime.Tick(deltaTime)) Destroy();
        }
        public object Clone()
        {
            Hitbox<T> clone = (Hitbox<T>)MemberwiseClone();
            clone.Collider = (Collider2D)_collider.Clone();
            clone.Direction = Direction;
            clone.Lifetime = _lifetime > 0f ? new Timer(_lifetime) : null;
            return clone;
        }
    }
}