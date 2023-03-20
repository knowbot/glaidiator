using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace BasicAI
{
    public class TaskGoToTarget : Node
    {

        private Transform _transform;

        public TaskGoToTarget(BTree btree, Transform transform)
        {
            tree = btree;
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            if (target == null)
            {
                state = NodeState.FAILURE;
                return state;
            }
            

            var currPos = _transform.position;
            var targetPos = target.position;
            Debug.DrawLine(currPos, targetPos, Color.red);

            if (Vector3.Distance(currPos, targetPos) > 0.01f)
            {
                _transform.position =
                    Vector3.MoveTowards(currPos, targetPos, GuardBT.speed * Time.deltaTime);

                _transform.LookAt(targetPos);
            }

            state = NodeState.RUNNING;
            return state;
        }
    }
}