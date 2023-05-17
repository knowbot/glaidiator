using System;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree.LeafNodes.TaskNodes
{
    public class TaskFaceEnemy : TaskNode
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
            
            Vector3 myPos = owner.Movement.Position;
            Vector3 dir = (target.Position - myPos).normalized;
            tree.Direction = MathStuff.Get8DDirection(dir.x, dir.z);

            state = NodeState.SUCCESS;
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