﻿using System;

namespace Glaidiator.Model.Actions
{
    public class Block : AAction, IHasCooldown
    {
        public string Name { get; }
        public Timer Cooldown { get; }
        
        public Block(int id, string name, float duration, bool canMove, bool canAction, float cooldownDuration = 0f) 
            : base(id, duration, canMove, canAction)
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