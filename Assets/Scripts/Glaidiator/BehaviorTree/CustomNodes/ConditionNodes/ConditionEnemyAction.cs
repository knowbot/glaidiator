using System;
using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using Glaidiator.Model.Actions;

namespace Glaidiator.BehaviorTree.CustomNodes.ConditionNodes
{
    public class ConditionEnemyAction: Condition
    {

        private string _actionName;

        public ConditionEnemyAction(string actionName)
        {
            _actionName = actionName;
        }

        public override NodeState Evaluate()
        {
            tree.Active = this;
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
            return new ConditionEnemyAction(_actionName);
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