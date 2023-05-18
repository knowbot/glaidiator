using System;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.CustomNodes.TaskNodes
{
    public class TaskStop : Task
    {
        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            
            tree.Move = false;
            tree.Block = false;
            tree.Dodge = false;
            tree.AttackHeavy = false;
            tree.AttackLight = false;
            tree.AttackRanged = false;
            state = NodeState.SUCCESS;
            return state;
        }

        #region Genetic Programming
        public override Node Clone()
        {
            return new TaskStop();
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