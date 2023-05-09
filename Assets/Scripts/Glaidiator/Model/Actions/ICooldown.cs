using Glaidiator.Model.Utils;

namespace Glaidiator.Model.Actions
{
    public interface ICooldown
    {
        public int ID { get; }
        public string Name { get; }
        public Timer Cooldown { get; }
        public ICooldown SetOnCooldown();
    }
}