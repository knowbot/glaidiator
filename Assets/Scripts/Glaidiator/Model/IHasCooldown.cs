using UnityEngine;

namespace Glaidiator.Model
{
    public interface IHasCooldown
    {
        float Duration { get; }
        float Delay { get; }
        
        public Timer Cooldown
        {
            get => Cooldown;
            set => SetOnCooldown();
        }

        public void SetOnCooldown()
        {
            Cooldown = new Timer(Delay, Duration);
        }

        public bool Tick(float deltaTime)
        {
            return Cooldown != null && Cooldown.Tick(deltaTime);
        }
    }
}