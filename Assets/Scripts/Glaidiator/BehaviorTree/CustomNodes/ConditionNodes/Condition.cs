using System;
using System.Xml;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.CustomNodes.ConditionNodes
{
    public abstract class Condition : Leaf
    {
        public override void ReplaceChild(Node oldChild, Node newChild)
        {
            throw new NotImplementedException();
        }

        public override void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }
    }
}