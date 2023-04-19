using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            foreach (IHitbox hb in _hitboxes)
            {
                hb.Collider.Draw();
                hb.Update(Time.deltaTime);
            }
            Cleanup();
        }
        public void Add(IHitbox collider)
        {
            _hitboxes.Add(collider);
            Debug.Log(_hitboxes.Count);
        }
        public void Remove(IHitbox collider)
        {
            _hitboxes.Remove(collider);
        }

        private void Cleanup()
        {
            foreach (IHitbox hb in _hitboxes.Where(hb => hb.ToDestroy).ToList())
            {
                hb.Destroy();
            }
        }
        
    }
}