using System;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.LeafNodes.TaskNodes.TaskNodes
{
    public class TaskBlock : Node
    {
        public TaskBlock()
        {
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;// for debug info
            tree.Block = true;

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