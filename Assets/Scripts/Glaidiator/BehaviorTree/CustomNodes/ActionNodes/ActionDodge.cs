using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.CustomNodes.ActionNodes
{
    public class ActionDodge : Action
    {
        public override NodeState Evaluate()
        {
            tree.Active = this;
            tree.Dodge = true;
            state = NodeState.SUCCESS;
            return state;
        }

        #region Genetic Programming
        public override Node Clone()
        {
            return new ActionDodge();
        }

        public override void Mutate()
        {
            return;
        }

        public override Node Randomized()
        {
            return Clone();
        }
        #endregion
    }
}