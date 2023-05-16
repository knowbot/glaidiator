using Glaidiator.Model.Collision;
using UnityEngine;

namespace Glaidiator.Model.Actions
{
    public class Attack : IAction, ICooldown
    {
        public ActionInfo Action { get; }
        public string Name { get; }
        public readonly float Damage;
        public Timer Cooldown { get; }
        public Timer Delay { get; }
        private Hitbox<Attack> Hitbox { get; set; }
        public IHitbox ActiveHitbox { get; private set; }



        // TODO: add hitbox info
        public Attack(ActionInfo action, Hitbox<Attack> hitbox, float damage, float cooldown = 0f, float delay = 0f)
        {
            Action = action;
            Name = action.Name;
            Damage = damage;
            Cooldown = new Timer(cooldown);
            Delay = new Timer(delay);
            Hitbox = hitbox;
            Hitbox.Origin = this;
        }

        public ICooldown SetOnCooldown()
        {
            Cooldown.Reset();
            return this;
        }

        public void SpawnHitbox(Vector2 direction)
        {
            Hitbox.Direction = direction;
            ActiveHitbox = Hitbox.Create();
            ActiveHitbox.Register();
        }

        public void End()
        {
            ActiveHitbox = null;
            Delay.Reset();
        }
    }
}