using System.Xml;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.CustomNodes.ConditionNodes
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
            tree.Active = this;
            
            state = GetData(_targetName) != null ? NodeState.SUCCESS : NodeState.FAILURE;

            return state;
        }

        public override Node Clone()
        {
            return new ConditionHasTarget(_targetName);
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
            w.WriteAttributeString("target", _targetName.ToString());
            w.WriteEndElement();
        }
    }
}