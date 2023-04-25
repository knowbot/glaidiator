using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Glaidiator.Model.Actions;
using Micosmo.SensorToolkit;
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
            var notChecked = new List<IHitbox>(_hitboxes);
            foreach (IHitbox hb in _hitboxes)
            {
                hb.Collider.Draw();
                hb.Update(Time.deltaTime);
                foreach (IHitbox other in from other in notChecked.ToList() 
                         where other.Active
                         where other != hb 
                         where other.Owner != hb.Owner 
                         where Differ.Collision.shapeWithShape(hb.Collider.Shape, other.Collider.Shape) != null 
                         select other)
                {
                    HandleCollision(hb, other);
                }
                notChecked.Remove(hb);
            }
            Cleanup();
        }

        private static void HandleCollision(IHitbox a, IHitbox b)
        {
            if (!a.Active || !b.Active) return;
            if (a is Hitbox<Attack> aAtk && b is Hitbox<Character> bChr)
            {
                bChr.Owner.Health.Subtract(aAtk.Origin.Damage);
                aAtk.Active = false;
            } else if (b is Hitbox<Attack> bAtk && a is Hitbox<Character> aChr){
                aChr.Owner.Health.Subtract(bAtk.Origin.Damage);
                bAtk.Active = false;
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

        private void Cleanup()
        {
            foreach (IHitbox hb in _hitboxes.Where(hb => hb.ToDestroy).ToList())
            {
                hb.Destroy();
            }
        }
        
    }
}