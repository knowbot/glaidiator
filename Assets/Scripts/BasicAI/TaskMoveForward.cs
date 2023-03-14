using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace BasicAI
{
    public class TaskMoveForward : Node
    {

        private Transform _transform;

        public TaskMoveForward(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            var currPos = _transform.position;
            var fwdDir = _transform.forward;
            var stepPos = currPos + (fwdDir);
            Debug.DrawLine(currPos, stepPos, Color.green);

            if (Vector3.Distance(currPos, stepPos) > 0.01f)
            {
                _transform.position =
                    Vector3.MoveTowards(currPos, stepPos, GuardBT.speed * Time.deltaTime);
                // move towards "look at" position
            }

            state = NodeState.RUNNING;
            return state;
        }
    }
}