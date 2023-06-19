using System;
using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model.Actions;

namespace Glaidiator.BehaviorTree.CustomNodes.ConditionNodes
{
    public class ConditionCanDoAction: Condition
    {
        private string _actionName;

        public ConditionCanDoAction(string actionName)
        {
            _actionName = actionName;
        }

        public override NodeState Evaluate()
        {
            tree.Active = this;
            if (tree == null) throw new NullReferenceException();

            IAction action = tree.Owner.Actions[_actionName];

            if (tree.Owner.IsOnCooldown(action.Action.Name) || !tree.Owner.HasEnoughStamina(action))
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
            return new ConditionCanDoAction(_actionName);
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