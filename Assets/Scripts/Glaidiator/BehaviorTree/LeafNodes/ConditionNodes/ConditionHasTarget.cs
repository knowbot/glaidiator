using System;
using System.Xml;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.LeafNodes.ConditionNodes
{
    public class ConditionHasTarget: Condition
    {
        private string _targetName;

        public ConditionHasTarget(string targetName)
        {
            _targetName = targetName;
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            
            state = GetData(_targetName) != null ? NodeState.SUCCESS : NodeState.FAILURE;

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
        
        public override void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(GetType().Name);
            w.WriteAttributeString("target", _targetName.ToString());
            w.WriteEndElement();
        }
    }
}