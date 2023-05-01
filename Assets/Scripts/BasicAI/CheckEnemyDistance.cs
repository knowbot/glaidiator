using System;
using UnityEngine;
using BehaviorTree;
using Glaidiator.Model;

namespace BasicAI
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
        
            Movement target = (Movement)GetData("enemy");
            if (target == null)
            {
                Debug.Log("CheckEnemyDistance target = null");
                state = NodeState.FAILURE;
                return state;
            }
            

            if (Vector3.Distance(_ownerCharacter.Movement.Position, target.Position) <= _threshold)
            {
                state = NodeState.SUCCESS;
                Debug.DrawLine(_ownerCharacter.Movement.Position, target.Position, Color.green, 0.1f);
                //Debug.Log("CheckEnemyDistance state = "+state);
            }
            else
            {
                state = NodeState.FAILURE;
                Debug.DrawLine(_ownerCharacter.Movement.Position, target.Position, Color.red, 0.1f);
            }
            
            return state;
        }
        
        
        public override Node Clone()
        {
            throw new System.NotImplementedException();
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