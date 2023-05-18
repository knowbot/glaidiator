using System;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.TaskNodes
{
    public class TaskBackEnemy : Task
    {
        public override NodeState Evaluate()
        {
            tree.currentNode = this;

            Movement target = ((Character)GetData("enemy"))?.Movement;
            if (target == null)
            {
                state = NodeState.FAILURE;
                return state;
            }

            Vector3 myPos = owner.Movement.Position;
            tree.Direction = (myPos - target.Position).normalized;

            state = NodeState.SUCCESS;
            return state;
        }

        #region Genetic Programming
        public override Node Clone()
        {
            return new TaskBackEnemy();
        }

        public override void Mutate()
        {
            throw new NotImplementedException();
        }

        public override Node Randomized()
        {
            return Clone();
        }
        #endregion
        
    }
}