using System;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model.Actions;

namespace Glaidiator.BehaviorTree.LeafNodes.ConditionNodes
{
    public class ConditionCanDoAction: Condition<string>
    {

        public ConditionCanDoAction(string actionName) : base(actionName) {}

        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            if (tree == null) throw new NullReferenceException();

            IAction action = owner.Actions[value];
            float cost = action.Action.Cost;
            
            if (owner.Cooldowns.Contains((ICooldown)action) || cost > owner.Stamina.Current)
            {
                state = NodeState.FAILURE;
            }
            else
            {
                state = NodeState.SUCCESS;
            }
            
            return state;
        }
        
        
        public override Node Clone()
        {
            return new ConditionEnemyAction(value);
        }

        public override void Mutate()
        {
            return;
        }

        public override Node Randomized()
        {
            return new ConditionEnemyAction(value);
        }
    }
}