using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Unity.VisualScripting;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes
{
    public class Module : Node
    {
        private readonly string _name;
        private readonly Composite _root;

        public Module(string name, Composite composite) : base()
        {
            _name = name;
            _root = composite;
            _root.SetTree(tree);
        }

        public override NodeState Evaluate()
        {
            return _root.Evaluate();
        }

        public override int GetDepth()
        {
            return _root.GetDepth();
        }
        public override int GetSize()
        {
            return _root.GetSize();
        }

        public override void Flatten(List<Node> nodes)
        {
            nodes.Add(this);
        }

        public override void ReplaceChild(Node oldChild, Node newChild)
        {
            return;
        }

        public override void SetTree(BTree newTree)
        {
            base.SetTree(newTree);
            _root.SetTree(tree);
        }

        public override Node Clone()
        {
            return new Module(_name, _root.Clone() as Composite);
        }

        public override void Mutate()
        {
            foreach (Node child in _root.Children)
            {
                child.Mutate();
            }
        }

        public override Node Randomized()
        {
            Node clone = Clone();
            clone.Mutate();
            return clone;
        }

        public override void ReadXml(XmlReader reader)
        {
            throw new System.NotImplementedException();
        }

        public override void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(_name);
            w.WriteAttributeString("root", _root.GetType().Name);
            foreach (Node child in _root.Children)
            {
                child.WriteXml(w);
            }
            w.WriteEndElement();
        }
    }
}