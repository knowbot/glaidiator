﻿namespace Glaidiator.Model.Actions
{
    public class Attack : AAction, IHasCooldown
    {
        public float Damage;
        public string Name { get; }
        public Timer Cooldown { get; }

        // returns false if timer is missing/done/not started yet
        // returns true if timer is ticking
        // TODO: add hitbox info
        public Attack(int id, string name, float damage, float actionDuration, bool canMove, bool canAction, float cooldownDuration = 0f) 
            : base(id, actionDuration, canMove, canAction)
        {
            Damage = damage;
            Name = name;
            Cooldown = new Timer(cooldownDuration);
        }

        public override AAction Start()
        {
            Duration.Reset();
            return this;
        }

        public IHasCooldown SetOnCooldown()
        {
            Cooldown.Reset();
            return this;
        }
    }
}