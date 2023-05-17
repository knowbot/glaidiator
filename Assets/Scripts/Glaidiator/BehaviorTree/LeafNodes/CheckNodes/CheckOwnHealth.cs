using System;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.LeafNodes.TaskNodes
{
    public class CheckOwnHealth : Node
    {
        private float _threshold;
        
        public CheckOwnHealth(float threshold)
        {
            _threshold = threshold;
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;

            if (owner.Health.Current >= _threshold)
            {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
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