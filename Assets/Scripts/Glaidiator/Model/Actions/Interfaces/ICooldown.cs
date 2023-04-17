using Glaidiator.Model.Utils;

namespace Glaidiator.Model.Actions.Interfaces
{
    public interface ICooldown
    {
        public string Name { get; }
        public Timer Cooldown { get; }
        public ICooldown SetOnCooldown();
    }
}