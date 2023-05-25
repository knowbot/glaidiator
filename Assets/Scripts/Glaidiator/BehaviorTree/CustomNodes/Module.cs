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
        public readonly string Name;
        public readonly Composite Root;

        public Module(string name, Composite composite) : base()
        {
            Name = name;
            Root = composite;
            Root.SetTree(tree);
        }

        public override NodeState Evaluate()
        {
            return Root.Evaluate();
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
            Root.SetTree(tree);
        }

        public override Node Clone()
        {
            return new Module(Name, Root.Clone() as Composite);
        }

        public override void Mutate()
        {
            foreach (Node child in Root.Children)
            {
                child.Mutate();
            }
        }

        public override Node Randomized()
        {
            var newModule = Clone() as Module;
            var newChildren = Root.Children.Select(c => c.Randomized());
            for (int i = 0; i < newModule.Root.Children.Count; i++)
            {
                var c = newModule.Root.Children[i];
                newModule.Root.ReplaceChild(c, c.Randomized());
            }
            return newModule;
        }

        public override void ReadXml(XmlReader reader)
        {
            throw new System.NotImplementedException();
        }

        public override void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(Name);
            w.WriteAttributeString("root", Root.GetType().Name);
            foreach (Node child in Root.Children)
            {
                child.WriteXml(w);
            }
            w.WriteEndElement();
        }
    }
}