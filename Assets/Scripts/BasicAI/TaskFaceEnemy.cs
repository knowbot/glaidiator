using System;
using BehaviorTree;
using Glaidiator.Model;
using UnityEngine;

namespace BasicAI
{
    public class TaskFaceEnemy : Node
    {

        public TaskFaceEnemy()
        {
            
        }

        public override NodeState Evaluate()
        {
            if (tree == null) throw new NullReferenceException();
        
            Movement target = (Movement)GetData("target");
            if (target == null)
            {
                state = NodeState.FAILURE;
                return state;
            }

            Vector3 myPos = _ownerCharacter.Movement.Position;
            tree.Direction = (target.Position - myPos).normalized;

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