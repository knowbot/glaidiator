using System;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;

namespace Glaidiator.BehaviorTree.LeafNodes.ConditionNodes
{
    public class ConditionCompareStamina : Condition<float>
    {
        
        public ConditionCompareStamina(float ratio) : base(ratio) {}
        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            
            Character enemy = (Character)GetData("enemy");
            if (enemy == null)
            {
                state = NodeState.FAILURE;
                return state;
            }

            float ratio = owner.Stamina.Current / enemy.Stamina.Current; 
            state = ratio > value ? NodeState.SUCCESS : NodeState.FAILURE;
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