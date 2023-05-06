using System;
using UnityEngine;
using BehaviorTree;
using Glaidiator.Model;
using Glaidiator.Model.Actions;

namespace BasicAI
{
    public class CheckCanDoAction : Node
    {

        private string _actionName;
        
        public CheckCanDoAction(string actionName)
        {
            _actionName = actionName;
        }

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

            state = _actionName == action.Action.Name ? NodeState.SUCCESS : NodeState.FAILURE;
            
            return state;
        }
        
        
        public override Node Clone()
        {
            return new CheckEnemyAction(_actionName);
        }

        public override void Mutate()
        {
            return;
        }

        public override Node Randomized()
        {
            return new CheckEnemyAction(_actionName);
        }
    }
}