using System;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;

namespace Glaidiator.BehaviorTree.LeafNodes.ConditionNodes
{
    public class ConditionEnemyStamina: ConditionNode<float>
    {

        public ConditionEnemyStamina(float threshold) : base(threshold) {}

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
            if (enemyStamina >= value)
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