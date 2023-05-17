using System;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using Glaidiator.Model.Actions;

namespace Glaidiator.BehaviorTree.LeafNodes.ConditionNodes
{
    public class ConditionEnemyAction: ConditionNode<string>
    {

        public ConditionEnemyAction(string actionName) : base(actionName) {}

        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            if (tree == null) throw new NullReferenceException();
        
            IAction action = ((Character)GetData("enemy"))?.ActiveAction;
            if (action == null)
            {
                state = NodeState.FAILURE;
                return state;
            }

            state = value == action.Action.Name ? NodeState.SUCCESS : NodeState.FAILURE;
            
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