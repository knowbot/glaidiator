using System;
using Glaidiator.BehaviorTree.Base;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.TaskNodes
{
    public class TaskFindNextWaypoint : Task
    {
        private Transform _transform;

        public override NodeState Evaluate()
        {
            throw new NotImplementedException();
        }

        #region Genetic Programming
        public override Node Clone()
        {
            return new TaskFindNextWaypoint();
        }

        public override void Mutate()
        {
            return;
        }

        public override Node Randomized()
        {
            return Clone();
        }
        #endregion
    }
}