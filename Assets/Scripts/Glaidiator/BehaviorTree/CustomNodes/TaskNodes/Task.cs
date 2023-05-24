using System;
using System.Collections.Generic;
using System.Xml;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.CustomNodes.TaskNodes
{
    public abstract class Task : Leaf
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

        public override void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(GetType().Name);
            w.WriteEndElement();
        }
    }
}