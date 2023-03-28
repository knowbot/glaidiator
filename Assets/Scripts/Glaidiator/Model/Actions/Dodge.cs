using System;
using UnityEngine;

namespace Glaidiator.Model.Actions
{
    public class Dodge : AAction, IHasCooldown
    {
        public string Name { get; } 
        public Timer Cooldown { get; }
        public Vector3 Direction { get; set; }
        
        public Dodge(int id, string name, float cost, bool canMove, bool canAction, float duration, float cooldownDuration = 0f) 
            : base(id, cost, canMove, canAction, duration)
        {
            Name = name;
            Cooldown = new Timer(cooldownDuration);
        }
        public IHasCooldown SetOnCooldown()
        {
            Cooldown.Reset();
            return this;
        }

        public override AAction Start()
        {
            Duration.Reset();
            return this;
        }
    }
}