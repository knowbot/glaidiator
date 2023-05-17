using System;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.LeafNodes.TaskNodes
{
    public class CheckHasWP : Node
    {
        public CheckHasWP()
        {
            
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            
            state = GetData("wp") != null ? NodeState.SUCCESS : NodeState.FAILURE;

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