#nullable enable
namespace Glaidiator.Model.Actions
{
    public interface IHasCooldown
    {
        public Timer Cooldown { get; }
        public IHasCooldown SetOnCooldown();
    }
}