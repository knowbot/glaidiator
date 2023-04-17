using System;
using Glaidiator.Model.Actions.Interfaces;
using Glaidiator.Model.Utils.Collision;
using Unity.VisualScripting;
using Timer = Glaidiator.Model.Utils.Timer;

namespace Glaidiator.Model.Actions
{
    public class Attack : IAction, ICooldown, IHitbox
    {
        public ActionInfo Action { get; }
        public float Damage;
        public string Name { get; }
        public Timer Cooldown { get; }
        public Collider2D Hitbox { get; set; }

        // TODO: add hitbox info
        public Attack(ActionInfo action, float damage, float cooldownDuration = 0f)
        {
            Action = action;
            Damage = damage;
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