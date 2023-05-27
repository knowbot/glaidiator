namespace Glaidiator.Model.Actions
{
    public class Block : ICooldown, IAction
    {
        public ActionInfo Action { get; }

        public void End()
        {
        }

        public string DisplayName { get; }
        public Timer Cooldown { get; }

        public Block(ActionInfo action,float cooldownDuration = 0f)
        {
            Action = action;
            DisplayName = action.Name;
            Cooldown = new Timer(cooldownDuration);
        }
        public ICooldown SetOnCooldown()
        {
            Cooldown.Reset();
            return this;
        }
    }
}