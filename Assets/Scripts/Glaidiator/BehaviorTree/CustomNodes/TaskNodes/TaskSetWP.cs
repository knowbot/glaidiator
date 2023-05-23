using System;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.TaskNodes
{
    public class TaskSetWP : Task
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

        #region Genetic Programming
        public override Node Clone()
        {
            return new TaskSetWP(_distance);
        }

        public override void Mutate()
        {
            _distance += MathStuff.Rand.NextFloat(4f) - 2f;
            _distance = Mathf.Clamp(_distance, 0f, Arena.Diagonal);
        }

        public override Node Randomized()
        {
            return new TaskSetWP(MathStuff.Rand.NextFloat(Arena.Diagonal));
        }
        #endregion
    }
}