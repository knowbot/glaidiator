using System;
using Glaidiator.BehaviorTree.Base;
using UnityEngine;

namespace Glaidiator.BehaviorTree.LeafNodes.TaskNodes
{
    public class TaskFindNextWaypoint : TaskNode
    {
        private Transform _transform;

        public TaskFindNextWaypoint()
        {
        
        }

        public override NodeState Evaluate()
        {
            throw new NotImplementedException();
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