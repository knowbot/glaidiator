using System;
using BehaviorTree;
using Glaidiator.Model;
using Glaidiator.Model.Utils;
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
            tree.currentNode = this;// for debug info
            if (tree == null) throw new NullReferenceException();
        
            Movement target = ((Character)GetData("enemy"))?.Movement;
            if (target == null)
            {
                state = NodeState.FAILURE;
                return state;
            }
            
            Vector3 myPos = _ownerCharacter.Movement.Position;
            Vector3 dir = (target.Position - myPos).normalized;
            tree.Direction = MathStuff.Get8DDirection(dir.x, dir.z);

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