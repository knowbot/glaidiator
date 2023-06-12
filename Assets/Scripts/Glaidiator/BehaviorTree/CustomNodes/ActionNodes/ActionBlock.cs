using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.CustomNodes.ActionNodes
{
    public class ActionBlock : Action
    {
        public override NodeState Evaluate()
        {
            tree.currentNode = this;// for debug info
            tree.Block = true;

            state = NodeState.SUCCESS;
            return state;
        }

        #region Genetic Programming
        public override Node Clone()
        {
            return new ActionBlock();
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