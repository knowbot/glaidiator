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
            throw new NotImplementedException();
        }

        public override Node Randomized()
        {
            return Clone();
        }
        #endregion
    }
}