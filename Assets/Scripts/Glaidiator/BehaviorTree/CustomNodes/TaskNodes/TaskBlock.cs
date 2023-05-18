using System;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.CustomNodes.TaskNodes
{
    public class TaskBlock : Task
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
            return new TaskBlock();
        }

        public override void Mutate()
        {
            throw new NotImplementedException();
        }

        public override Node Randomized()
        {
            return Clone();
        }
        #endregion
    }
}