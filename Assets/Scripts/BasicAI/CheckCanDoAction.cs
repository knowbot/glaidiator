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

            IAction action = _ownerCharacter._actions[_actionName];
            float cost = action.Action.Cost;
            
            if (_ownerCharacter.Cooldowns.Contains((ICooldown)action) || cost > _ownerCharacter.Stamina.Current)
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