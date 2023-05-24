using System;
using System.Xml;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.CustomNodes.CheckNodes
{
    public abstract class Check : Leaf
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