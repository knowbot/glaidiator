namespace Glaidiator.Model.Actions
{
    public interface ICooldown
    {
        public string Name { get; }
        public Timer Cooldown { get; }
        public ICooldown SetOnCooldown();
    }
}