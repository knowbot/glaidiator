using System;
using Glaidiator.BehaviorTree.Base;
using UnityEngine;

namespace Glaidiator.BehaviorTree.LeafNodes.TaskNodes
{
    public class TaskSetWP : Node
    {

        private float _distance;
        
        public TaskSetWP(float distance)
        {
            _distance = distance;
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;// for debug info
            Vector3 wp = owner.Movement.Position + (tree.Direction * _distance);
            SetData("wp", wp);
            ////Debug.Log("set waypoint: " + wp);
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