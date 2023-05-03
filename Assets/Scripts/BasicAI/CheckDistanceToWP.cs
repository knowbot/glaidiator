using BehaviorTree;
using Glaidiator.Model.Utils;
using UnityEngine;

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
            
            float signedDistance = MathUtils.GetSignedDistance(_ownerCharacter.Movement.Position, (Vector3)target);
            
            if (signedDistance <= _threshold)
            {
                Debug.Log("successfully reached waypoint = " + target);
                state = NodeState.SUCCESS;
            }
            else
            {
                //Debug.Log("wp dist = "+signedDistance);
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