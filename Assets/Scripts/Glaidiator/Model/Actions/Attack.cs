namespace Glaidiator.Model.Actions
{
    public class Attack : AHasCooldown, IAction
    {
        public float Damage;
        public float Duration { get; }
        public bool CanMove { get; }
        public bool CanAction { get; }
        protected override CooldownData CooldownData { get; }

        protected override Timer Cooldown { get; set; }

        // returns false if timer is missing/done/not started yet
        // returns true if timer is ticking
        // TODO: add hitbox info
        public Attack(float damage, float duration, bool allowMove, bool allowAction, CooldownData cooldownData = default)
        {
            Damage = damage;
            Duration = duration;
            CanMove = allowMove;
            CanAction = allowAction;
            CooldownData = cooldownData;
        }
    }
}