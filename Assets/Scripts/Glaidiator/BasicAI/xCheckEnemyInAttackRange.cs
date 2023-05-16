using System;
using BehaviorTree;
using UnityEngine;

namespace BasicAI
{
    public class xCheckEnemyInAttackRange : Node // deprecated
    {
        private Transform _transform;
        //private Animator _animator;

        public xCheckEnemyInAttackRange(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {

            object t = GetData("target");
            if (t == null)
            {
                state = NodeState.FAILURE;
                return state;
            }

            Transform target = (Transform)t;
            if (Vector3.Distance(_transform.position, target.position) <= xGuardBT.attackRange)
            {
                // check = true

                state = NodeState.SUCCESS;
                return state;
            }


            state = NodeState.FAILURE;
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