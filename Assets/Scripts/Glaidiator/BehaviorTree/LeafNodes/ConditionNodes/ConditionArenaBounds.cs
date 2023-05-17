using System;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using UnityEngine;

namespace Glaidiator.BehaviorTree.LeafNodes.ConditionNodes
{
    public class ConditionArenaBounds: ConditionNode<float>
    {
        public ConditionArenaBounds(float distance) : base(distance) {}
        
        public override NodeState Evaluate()
        {
            tree.currentNode = this;

            Vector3 target = owner.Movement.Position + (tree.Direction * value);
            if (target.x < 0f || target.x > Arena.MaxSize ||
                target.z < 0f || target.z > Arena.MaxSize)
            {
                state = NodeState.FAILURE;
                //Debug.Log("CheckArenaBounds outside target = " + target);
            }
            else
            {
                state = NodeState.SUCCESS;
            }
                
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