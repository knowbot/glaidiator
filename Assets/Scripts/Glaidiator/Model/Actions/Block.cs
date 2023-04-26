using System;
using Glaidiator.Model.Actions;
using Glaidiator.Model.Utils;

namespace Glaidiator.Model.Actions
{
    public class Block : ICooldown, IAction
    {
        public ActionInfo Action { get; }
        public void End()
        {
        }

        public string Name { get; }
        public Timer Cooldown { get; }

        public Block(ActionInfo action,float cooldownDuration = 0f)
        {
            Action = action;
            Name = action.Name;
            Cooldown = new Timer(cooldownDuration);
        }
        public ICooldown SetOnCooldown()
        {
            Cooldown.Reset();
            return this;
        }
    }
}