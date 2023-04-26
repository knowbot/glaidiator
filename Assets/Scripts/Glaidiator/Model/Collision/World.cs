using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Differ.Data;
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
                         select other)
                {
                    ShapeCollision collisionInfo = Differ.Collision.shapeWithShape(hb.Collider.Shape, other.Collider.Shape);
                    if(collisionInfo != null)
                        HandleCollision(hb, other, collisionInfo);
                }
                notChecked.Remove(hb);
            }
            Cleanup();
        }

        private static void HandleCollision(IHitbox a, IHitbox b, ShapeCollision info)
        {
            if (!a.Active || !b.Active) return;
            switch (a)
            {
                case Hitbox<Character> chr:
                    switch (b)
                    {
                        case Hitbox<Attack> atk:
                            chr.Owner.GetHit(atk.Origin);
                            atk.Active = false;
                            break;
                    }

                    break;
                case Hitbox<Attack> atk:
                    switch (b)
                    {
                        case Hitbox<Character> chr:
                            chr.Owner.GetHit(atk.Origin);
                            atk.Active = false;
                            break;
                    }

                    break;
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