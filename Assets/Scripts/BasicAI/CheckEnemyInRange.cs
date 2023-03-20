using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

namespace BasicAI
{
    public class CheckEnemyInRange : Node
    {
        private Transform _transform;

        //private static int _enemyLayerMask = 1 << 6;

        public CheckEnemyInRange(BTree btree, Transform transform)
        {
            tree = btree;
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                // FIXME: limit layer check
                Collider[] colliders = Physics.OverlapSphere(_transform.position, GuardBT.fovRange);

                if (colliders.Length > 1) // +1 since we find our own collider
                {
                    //Debug.Log("targets = " + colliders.Length);
                    //parent.parent.SetData("target", colliders[0].transform);
                    SetData("target", colliders[0].transform);
                    //Debug.Log("target at: " + colliders[0].transform.position);

                    state = NodeState.SUCCESS;
                    return state;
                }

                state = NodeState.FAILURE;
                return state;
            }
            
            state = NodeState.SUCCESS;
            return state;
        }
    }
}