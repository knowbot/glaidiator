namespace Glaidiator.Model.Actions
{
    public interface IAction
    {
        public ActionInfo Action { get; }
        
        public void Start()
        {
            Action.Duration.Reset();
        }
        
        public bool Tick(float deltaTime)
        {
            return Action.Duration.Tick(deltaTime);
        }

        public void End();
    }
}