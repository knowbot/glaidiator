using System;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree.LeafNodes.TaskNodes
{
    public class CheckEnemyDistance : Node
    {
        private float _threshold;
        public CheckEnemyDistance(float threshold)
        {
            _threshold = threshold;
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;// for debug info
            if (tree == null) throw new NullReferenceException();
        
            Movement target = ((Character)GetData("enemy"))?.Movement;
            if (target == null)
            {
                ////Debug.Log("CheckEnemyDistance target = null");
                state = NodeState.FAILURE;
                return state;
            }

            float dist = Vector3.Distance(owner.Movement.Position, target.Position);
            tree.enemyDistance = dist;

            if (dist <= _threshold)
            {
                state = NodeState.SUCCESS;
                //Debug.DrawLine(_ownerCharacter.Movement.Position, target.Position, Color.green, 0.1f);
                ////Debug.Log("CheckEnemyDistance state = "+state);
            }
            else
            {
                state = NodeState.FAILURE;
                //Debug.DrawLine(_ownerCharacter.Movement.Position, target.Position, Color.red, 0.1f);
            }
            
            return state;
        }
        
        
        public override Node Clone()
        {
            return new CheckEnemyDistance(_threshold);
        }

        public override void Mutate()
        {
            _threshold += MathStuff.Rand.NextFloat(2f) - 1f;
           _threshold = Mathf.Clamp(_threshold, 0f, Arena.MaxSize);
        }

        public override Node Randomized()
        {
            return new CheckEnemyDistance(MathStuff.Rand.NextFloat(Arena.MaxSize));
        }
    }
}