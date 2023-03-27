using System;

namespace Glaidiator.Model.Actions
{
    public abstract class AAction
    {
        public readonly int ID;
        private readonly float _duration;
        protected Timer Duration { get; set; }
        public bool CanMove { get; }
        public bool CanAction { get; }

        protected AAction(int id, float duration, bool canMove, bool canAction)
        {
            ID = id;
            _duration = duration;
            CanMove = canMove;
            CanAction = canAction;
            Duration = new Timer(_duration);
        }
        
        public abstract AAction Start();
        public bool Tick(float deltaTime)
        {
            return Duration.Tick(deltaTime);
        }
    }
}