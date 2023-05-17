using System;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;

namespace Glaidiator.BehaviorTree.LeafNodes.TaskNodes.CheckNodes
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
            state = enemyHealth >= _threshold ? NodeState.SUCCESS : NodeState.FAILURE;
            
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