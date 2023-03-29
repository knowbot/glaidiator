#nullable enable
namespace Glaidiator.Model.Actions
{
    public interface IHasCooldown
    {
        public string Name { get; }
        public Timer Cooldown { get; }
        public IHasCooldown SetOnCooldown();
    }
}