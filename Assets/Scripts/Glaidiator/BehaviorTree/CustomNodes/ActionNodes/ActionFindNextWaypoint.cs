using System;
using Glaidiator.BehaviorTree.Base;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.ActionNodes
{
    public class ActionFindNextWaypoint : Action
    {
        private Transform _transform;

        public override NodeState Evaluate()
        {
            throw new NotImplementedException();
        }

        #region Genetic Programming
        public override Node Clone()
        {
            return new ActionFindNextWaypoint();
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