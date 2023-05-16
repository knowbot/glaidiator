using System;
using BehaviorTree;
using Glaidiator.Model;

namespace BasicAI
{
    public class CheckEnemyStamina : Node
    {
        private float _threshold;

        public CheckEnemyStamina(float threshold)
        {
            _threshold = threshold;
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;

            Character enemy = (Character)GetData("enemy");
            if (enemy == null)
            {
                //Debug.Log("CheckEnemyStamina enemy = null");
                state = NodeState.FAILURE;
                return state;
            }

            float enemyStamina = enemy.Stamina.Current;
            if (enemyStamina >= _threshold)
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