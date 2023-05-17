using System;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.LeafNodes.ConditionNodes
{
    public class ConditionHasTarget: ConditionNode<string>
    {
        public ConditionHasTarget(string targetName) : base(targetName) { }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            
            state = GetData(value) != null ? NodeState.SUCCESS : NodeState.FAILURE;

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