using System;
using BehaviorTree;
using Glaidiator.Model;
using UnityEngine;

namespace BasicAI
{
    public class xCheckEnemyInRange : Node // deprecated
    {
        private Movement _transform;

        public xCheckEnemyInRange(BTree btree, Character transform)
        {
            tree = btree;
            _transform = transform.Movement;
        }
        
        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                Movement targetTransform = tree.GetEnemyChar().Movement;
                float dist = Vector3.Distance(_transform.Position, targetTransform.Position);

                if (dist <= xBossBT.aggroRange)
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
                if (dist > xBossBT.aggroRange)
                {
                    ClearData("target");
                    state = NodeState.FAILURE;
                    return state;
                }
            }
            
            state = NodeState.SUCCESS;
            return state;
        }

        public override Node Clone()
        {
            Node clone = new xCheckEnemyInRange(tree, owner);
            return clone;
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