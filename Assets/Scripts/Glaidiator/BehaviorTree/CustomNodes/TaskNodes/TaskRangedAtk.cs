using System;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.CustomNodes.TaskNodes
{
    public class TaskRangedAtk : Task
    {
        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            tree.AttackRanged = true;
            state = NodeState.SUCCESS;
            return state;
        }

        #region Genetic Programming
        public override Node Clone()
        {
            return new TaskRangedAtk();
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