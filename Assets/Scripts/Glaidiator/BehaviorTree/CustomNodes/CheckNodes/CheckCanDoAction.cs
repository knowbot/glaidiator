using System;
using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model.Actions;

namespace Glaidiator.BehaviorTree.CustomNodes.CheckNodes
{
    public class CheckCanDoAction: Check
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

            IAction action = owner.Actions[_actionName];
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
            return new CheckEnemyAction(_actionName);
        }

        public override void Mutate()
        {
            return;
        }

        public override Node Randomized()
        {
            return Clone();
        }
        
        public override void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(GetType().Name);
            w.WriteAttributeString("actionName", _actionName);
            w.WriteEndElement();
        }
    }
}