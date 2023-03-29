using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Glaidiator.Model;
using UnityEngine;

namespace BasicAI
{
    public class CheckEnemyInRange : Node
    {
        private Movement _transform;

        public CheckEnemyInRange(BTree btree, Character transform)
        {
            tree = btree;
            _transform = transform.Movement;
        }

        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                Movement targetTransform = tree.GetPlayerChar().Movement;
                float dist = Vector3.Distance(_transform.Position, targetTransform.Position);

                if (dist <= BossBT.aggroRange)
                {
                    SetData("target", targetTransform);
                    state = NodeState.SUCCESS;
                    return state;
                }
                
                state = NodeState.FAILURE;
                return state;
            } 
            else if (t.GetType() == typeof(Movement))
            {
                Movement tMovement = (Movement)t;
                float dist = Vector3.Distance(_transform.Position, tMovement.Position);
                if (dist > BossBT.aggroRange)
                {
                    ClearData("target");
                    state = NodeState.FAILURE;
                    return state;
                }
            }
            
            state = NodeState.SUCCESS;
            return state;
        }
    }
}