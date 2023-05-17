using System;
using System.Collections.Generic;
using System.Xml;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.LeafNodes.ConditionNodes
{
    public abstract class Condition : Leaf
    {
        public override void Flatten(List<Node> nodes)
        {
            nodes.Add(this);
        }

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