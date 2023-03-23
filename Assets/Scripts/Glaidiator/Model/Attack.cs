namespace Glaidiator.Model
{
    public class Attack : IHasCooldown
    {
        public float Damage;
        public float Duration { get; }
        public float Delay { get; }

        public Timer Cooldown { get; set; }

        // returns false if timer is missing/done/not started yet
        // returns true if timer is ticking
        // TODO: add hitbox info
        public Attack(float duration, float damage, float delay)
        {
            Duration = duration;
            Delay = delay;
            Damage = damage;
        }
    }
}