﻿using System;
using System.Numerics;
using Glaidiator.Model.Actions;
using Glaidiator.Model.Utils;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Glaidiator.Model.Collision
{
    public class Hitbox<T> : IHitbox, ICloneable
    {
        public World World { get; private set; }
        private readonly Collider2D _collider;
        private readonly float _lifetime;
        public bool ToDestroy { get; protected set; }

        public Collider2D Collider { get; private set; }
        public bool Active { get; set; } = true;
        public Character Owner { get; private set; }
        public T Origin { get; internal set; }
        public Timer Lifetime { get; private set; }

        public Vector2 Direction = Vector2.zero;

        public Hitbox(Collider2D collider, Character owner, float lifetime = 0f)
        {
            World = World.instance;
            _collider = collider;
            Collider = (Collider2D)_collider.Clone();
            Owner = owner;
            ToDestroy = false;
            _lifetime = lifetime;
            if(_lifetime > 0f) Lifetime = new Timer(_lifetime);
        }

        public IHitbox Create()
        {
            Hitbox<T> newHb = (Hitbox<T>)Clone();
            newHb.Active = true;
            newHb.Collider.Rotation = MathUtils.GetSignedAngle(Owner.Movement.Rotation, Quaternion.Euler(0, 0, 0), Vector3.up);
            newHb.Collider.Position = Owner.Movement.Position.xz() + Collider.Offset.Rotate(newHb.Collider.Rotation);
            return newHb;
        }
        
        public void Destroy()
        {
            Active = false;
            Collider = null;
            Deregister();
        }

        public void Register()
        {
            World.instance.Add(this);
        }

        public void Deregister()
        {
            World.instance.Remove(this);
        }
        
        public virtual void Update(float deltaTime)
        {
            // when lifetime is over, reset collider and deregister
            if (Lifetime is not null && !Lifetime.Tick(deltaTime)) ToDestroy = true;
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