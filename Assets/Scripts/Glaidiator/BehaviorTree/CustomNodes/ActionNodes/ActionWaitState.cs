using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.CustomNodes.ActionNodes
{
    public class ActionWaitState : Action
    {
        public override NodeState Evaluate()
        {
            tree.Active = this;
            tree.Wait = true;
            return NodeState.SUCCESS;
        }

        public override Node Clone()
        {
            throw new System.NotImplementedException();
        }

        public override void Mutate()
        {
            throw new System.NotImplementedException();
        }

        public override Node Randomized()
        {
            throw new System.NotImplementedException();
        }
    }
}