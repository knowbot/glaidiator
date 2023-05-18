using System;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.CustomNodes.TaskNodes
{
    public class TaskDodge : Task
    {
        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            tree.Dodge = true;
            state = NodeState.SUCCESS;
            return state;
        }

        #region Genetic Programming
        public override Node Clone()
        {
            return new TaskDodge();
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