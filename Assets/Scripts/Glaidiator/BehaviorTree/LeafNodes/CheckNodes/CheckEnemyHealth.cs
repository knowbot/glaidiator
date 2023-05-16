using System;
using BehaviorTree;
using Glaidiator.Model;

namespace BasicAI
{
    public class CheckEnemyHealth : Node
    {
        private float _threshold;

        public CheckEnemyHealth(float threshold)
        {
            _threshold = threshold;
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;

            Character enemy = (Character)GetData("enemy");
            if (enemy == null)
            {
                //Debug.Log("CheckEnemyHealth enemy = null");
                state = NodeState.FAILURE;
                return state;
            }

            float enemyHealth = enemy.Health.Current;
            if (enemyHealth >= _threshold)
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