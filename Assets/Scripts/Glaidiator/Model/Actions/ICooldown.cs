namespace Glaidiator.Model.Actions
{
    public interface ICooldown
    {
        public string DisplayName { get; }
        public Timer Cooldown { get; }
        public ICooldown SetOnCooldown();
    }
}