using System;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.LeafNodes.ConditionNodes
{
    public class ConditionOwnStamina: ConditionNode<float>
    {
        
        
        public ConditionOwnStamina(float threshold) : base(threshold) {}

        public override NodeState Evaluate()
        {
            tree.currentNode = this;

            if (owner.Stamina.Current >= value)
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