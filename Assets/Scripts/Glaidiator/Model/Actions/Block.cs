using System;
using Glaidiator.Model.Actions.Interfaces;
using Glaidiator.Model.Utils;

namespace Glaidiator.Model.Actions
{
    public class Block : ICooldown, IAction
    {
        public ActionInfo Action { get; }
        public string Name { get; }
        public Timer Cooldown { get; }

        public Block(ActionInfo action,float cooldownDuration = 0f)
        {
            Cooldown = new Timer(cooldownDuration);
        }
        public ICooldown SetOnCooldown()
        {
            Cooldown.Reset();
            return this;
        }
    }
}