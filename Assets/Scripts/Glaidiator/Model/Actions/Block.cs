using System;

namespace Glaidiator.Model.Actions
{
    public class Block : AAction, IHasCooldown
    {
        public Block(int id, float duration, bool canMove, bool canAction, float cooldownDuration = 0f) : base(id, duration, canMove, canAction)
        {
            Cooldown = new Timer(cooldownDuration);
        }

        public Timer Cooldown { get; }
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