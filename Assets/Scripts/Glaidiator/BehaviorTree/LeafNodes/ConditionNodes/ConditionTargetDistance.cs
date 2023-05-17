using System;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree.LeafNodes.ConditionNodes
{
    public class ConditionTargetDistance: ConditionNode<float>
    {
        private string _targetName;

        public ConditionTargetDistance(string targetName, float distance) : base(distance)
        {
            _targetName = targetName;
        }
        
        public override NodeState Evaluate()
        {
            tree.currentNode = this;// for debug info
            var target = GetData(_targetName);
            if (target is not Vector3)
            {
                //Debug.Log("CheckDistanceToWP, no waypoint found");
                state = NodeState.FAILURE;
                return state;
            }
            
            //float signedDistance = MathStuff.GetSignedDistance(_ownerCharacter.Movement.Position, (Vector3)target, tree.Direction);
            float distance = Vector3.Distance(owner.Movement.Position, (Vector3)target);
            Vector3 nDir = ((Vector3)target - owner.Movement.Position).normalized;
            Vector3 nnDir = MathStuff.Get8DDirection(nDir.x, nDir.z);
            
            if (distance <= value || nnDir != tree.Direction)
            {
                ////Debug.Log("successfully reached waypoint = " + target);
                //Debug.Log(_ownerCharacter.Movement.Position);
                state = NodeState.SUCCESS;
            }
            else
            {
                ////Debug.Log("wp dist = "+distance);
                state = NodeState.FAILURE;
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