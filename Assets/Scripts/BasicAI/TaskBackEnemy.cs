using BehaviorTree;
using Glaidiator.Model;
using UnityEngine;

namespace BasicAI
{
    public class TaskBackEnemy : Node
    {
        public TaskBackEnemy(){}


        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            
            Movement target = ((Character)GetData("enemy"))?.Movement;
            if (target == null)
            {
                state = NodeState.FAILURE;
                return state;
            }
            
            Vector3 myPos = _ownerCharacter.Movement.Position;
            tree.Direction = (myPos - target.Position).normalized;

            state = NodeState.SUCCESS;
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