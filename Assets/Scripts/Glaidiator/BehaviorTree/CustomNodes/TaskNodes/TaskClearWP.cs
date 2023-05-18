using System;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.CustomNodes.TaskNodes
{
    public class TaskClearWP : Task
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
            return new TaskClearWP();
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