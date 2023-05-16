using System;
using BehaviorTree;
using Glaidiator.Utils;
using UnityEngine;

namespace BasicAI
{
    public class CheckWPDistance : Node
    {
        private string _targetName;
        private float _threshold;
        
        public CheckWPDistance() : base()
        {
        }

        public CheckWPDistance(string targetName)
        {
            _targetName = targetName;
        }

        public CheckWPDistance(float threshold)
        {
            _targetName = "wp";
            _threshold = threshold;
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
            
            if (distance <= _threshold || nnDir != tree.Direction)
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