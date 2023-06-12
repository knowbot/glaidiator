using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.CustomNodes.ActionNodes
{
    public class ActionClearWp : Action
    {
        public override NodeState Evaluate()
        {
            tree.currentNode = this;

            state = ClearData("wp") ? NodeState.SUCCESS : NodeState.FAILURE;

            return state;
        }

        #region Genetic Programming
        public override Node Clone()
        {
            return new ActionClearWp();
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