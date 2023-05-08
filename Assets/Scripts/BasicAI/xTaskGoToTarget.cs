using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Glaidiator.Model;

namespace BasicAI
{
    public class xTaskGoToTarget : Node // deprecated
    {

        private Movement _transform;

        public xTaskGoToTarget(BTree btree, Character transform)
        {
            tree = btree;
            _transform = transform.Movement;
        }

        public override NodeState Evaluate()
        {
            Movement target = (Movement)GetData("target");
            if (target == null)
            {
                state = NodeState.FAILURE;
                return state;
            }

            var currPos = _transform.Position;
            var targetPos = target.Position;
            //Debug.DrawLine(currPos, targetPos, Color.red);

            if (Vector3.Distance(currPos, targetPos) > 0.01f)
            {
                
                tree.Direction = ((targetPos - currPos).normalized).xz();
                tree.Move = true;

                if (Vector3.Distance(currPos, targetPos) < xBossBT.lightAtkRange)
                {
                    state = NodeState.SUCCESS;
                    return state;
                }
            }

            state = NodeState.RUNNING;
            return state;
        }

        public override Node Clone()
        {
            Node clone = new xTaskGoToTarget(tree, _ownerCharacter);
            return clone;
        }

        public override void Mutate()
        {
            throw new System.NotImplementedException();
        }

        public override Node Randomized()
        {
            throw new System.NotImplementedException();
        }
    }
}