namespace Glaidiator.Model.Actions
{
    public class Block : AAction, IHasCooldown
    {
        public string Name { get; }
        public Timer Cooldown { get; }
        
        public Block(int id, string name, float cost, bool canMove, bool canAction, float duration, float cooldownDuration = 0f) 
            : base(id, cost, canMove, canAction, duration)
        {
            Name = name;
            Cooldown = new Timer(cooldownDuration);
        }
        public IHasCooldown SetOnCooldown()
        {
            Cooldown.Reset();
            return this;
        }

        public override AAction Start()
        {
            Duration.Reset();
            return this;
        }
    }
}