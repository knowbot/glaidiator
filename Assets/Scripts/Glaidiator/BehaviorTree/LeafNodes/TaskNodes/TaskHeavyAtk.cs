using System;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.LeafNodes.TaskNodes
{
    public class TaskHeavyAtk : TaskNode
    {

        public TaskHeavyAtk()
        {
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;// for debug info
            tree.AttackHeavy = true;

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