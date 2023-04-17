using System;
using System.Collections.Generic;
using UnityEngine;

namespace Glaidiator.Model.Collision
{
    public class World : MonoBehaviour
    {
        public static World instance;
        
        private readonly List<IHitbox> _hitboxes = new List<IHitbox>();

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            foreach (IHitbox hitbox in _hitboxes)
            {
                hitbox.Update(Time.deltaTime);
                hitbox.Collider.Draw();
            }
        }

        public void Add(IHitbox hitbox)
        {
            _hitboxes.Add(hitbox);
        }
        public void Remove(IHitbox hitbox)
        {
            _hitboxes.Remove(hitbox);
        }
    }
}