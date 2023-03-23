namespace Glaidiator.Model.Actions
{

    public struct CooldownData
    {
        public readonly float Duration ;
        public readonly float Delay;

        public CooldownData(float duration = 0f, float delay = 0f)
        {
            Duration = duration;
            Delay = delay;
        }
    }

    public abstract class AHasCooldown
    {
        protected abstract CooldownData CooldownData { get; }

        protected virtual Timer Cooldown { get; set; }

        public bool HasCooldown()
        {
            return CooldownData.Duration != 0f;
        }

        public AHasCooldown SetOnCooldown()
        {
            Cooldown = new Timer(CooldownData.Delay, CooldownData.Duration);
            return this;
        }

        public bool Tick(float deltaTime)
        {
            return Cooldown != null && Cooldown.Tick(deltaTime);
        }
    }
}