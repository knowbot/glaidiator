using System;

namespace Glaidiator.Model.Actions
{
    public abstract class AAction
    {
        public int ID { get; }
        public int Cost { get; }
        public bool CanMove { get; }
        public bool CanAction { get; }
        protected Timer Duration { get; set; }

        protected AAction(int id, int cost, bool canMove, bool canAction, float duration)
        {
            ID = id;
            Cost = cost;
            CanMove = canMove;
            CanAction = canAction;
            Duration = new Timer(duration);
        }
        
        public abstract AAction Start();
        public bool Tick(float deltaTime)
        {
            return Duration.Tick(deltaTime);
        }
    }
}