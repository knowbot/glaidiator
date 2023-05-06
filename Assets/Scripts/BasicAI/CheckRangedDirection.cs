using BehaviorTree;
using Glaidiator.Model;
using UnityEngine;

namespace BasicAI
{
    public class CheckRangedDirection : Node
    {
        private float _maxAngle;

        public CheckRangedDirection()
        {
            _maxAngle = 5f;
        }

        public CheckRangedDirection(float maxAngle)
        {
            _maxAngle = maxAngle;
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            
            Movement target = ((Character)GetData("enemy"))?.Movement;
            if (target == null)
            {
                Debug.Log("CheckRangedDirection target = null");
                state = NodeState.FAILURE;
                return state;
            }

            // calculate angle difference and check for some maximum offset before success
            Vector3 targetDirection = (target.Position - _ownerCharacter.Movement.Position).normalized;
            Vector3 currDirection = tree.Direction;
            float angle = Vector3.Angle(currDirection, targetDirection);
            
            if (angle <= _maxAngle)
            {
                Debug.DrawLine(_ownerCharacter.Movement.Position, target.Position, Color.cyan, 1f);
                state = NodeState.SUCCESS;
            }
            else
            {
                Debug.DrawLine(_ownerCharacter.Movement.Position, target.Position, Color.yellow, 1f);
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