using System;
using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using Glaidiator.Model.Actions;

namespace Glaidiator.BehaviorTree.CustomNodes.CheckNodes
{
    public class CheckEnemyCanDoAction: Check
    {
        private readonly string _actionName;

        public CheckEnemyCanDoAction(string actionName)
        {
            _actionName = actionName;
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            if (tree == null) throw new NullReferenceException();

            var enemy = ((Character)GetData("enemy"));
            IAction action = enemy.Actions[_actionName];

            if (enemy.IsOnCooldown(action.Action.Name) || !enemy.HasEnoughStamina(action))
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
            return new CheckEnemyCanDoAction(_actionName);
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