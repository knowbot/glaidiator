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
            if (tree == null) throw new NullReferenceException();
        
            Movement target = (Movement)GetData("target");
            if (target == null)
            {
                state = NodeState.FAILURE;
                return state;
            }

            if (Vector3.Distance(_ownerCharacter.Movement.Position, target.Position) <= _threshold)
            {
                state = NodeState.SUCCESS;
            }
            else
            {
                state = NodeState.FAILURE;
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