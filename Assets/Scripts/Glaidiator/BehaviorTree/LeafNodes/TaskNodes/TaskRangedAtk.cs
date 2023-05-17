using System;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.LeafNodes.TaskNodes
{
    public class TaskRangedAtk : TaskNode
    {
        public TaskRangedAtk(){}

        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            tree.AttackRanged = true;
            state = NodeState.SUCCESS;
            return state;
        }

        public override Node Clone()
        {
            throw new NotImplementedException();
        }

        public override void Mutate()
        {
            throw new NotImplementedException();
        }

        public override Node Randomized()
        {
            throw new NotImplementedException();
        }
    }
}