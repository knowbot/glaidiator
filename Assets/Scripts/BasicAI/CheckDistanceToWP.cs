using System.Numerics;
using BehaviorTree;
using Glaidiator.Model.Utils;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace BasicAI
{
    public class CheckDistanceToWP : Node
    {
        private string _targetName;
        private float _threshold;
        
        public CheckDistanceToWP() : base()
        {
        }

        public CheckDistanceToWP(string targetName)
        {
            _targetName = targetName;
        }

        public CheckDistanceToWP(float threshold)
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
                Debug.Log("CheckDistanceToWP, no waypoint found");
                state = NodeState.FAILURE;
                return state;
            }
            
            //float signedDistance = MathStuff.GetSignedDistance(_ownerCharacter.Movement.Position, (Vector3)target, tree.Direction);
            float distance = Vector3.Distance(_ownerCharacter.Movement.Position, (Vector3)target);
            Vector3 nDir = ((Vector3)target - _ownerCharacter.Movement.Position).normalized;
            Vector3 nnDir = MathStuff.Get8DDirection(nDir.x, nDir.z);
            
            if (distance <= _threshold || nnDir != tree.Direction)
            {
                //Debug.Log("successfully reached waypoint = " + target);
                Debug.Log(_ownerCharacter.Movement.Position);
                state = NodeState.SUCCESS;
            }
            else
            {
                //Debug.Log("wp dist = "+distance);
                state = NodeState.FAILURE;
            }
            
            return state;
        }

        public override Node Clone()
        {
            throw new System.NotImplementedException();
        }

        public override void Mutate()
        {
            throw new System.NotImplementedException();
        }

        public override Node Randomized()
        {
            throw new System.NotImplementedException();
        }
    }
}