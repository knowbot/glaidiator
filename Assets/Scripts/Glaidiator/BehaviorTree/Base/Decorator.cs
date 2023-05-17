using System.Collections.Generic;
using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;

namespace Glaidiator.BehaviorTree.Base
{
    public abstract class Decorator : Node
    {
        public Node Child { get; protected set; }

        public Decorator() : base()
        {
        }

        public Decorator(List<Node> children) : base(children)
        {
            Child = children[0];
        }
    
        public Decorator(Node child)
        {
            Child = child;
        }

        public Decorator(BTree btree, Node child)
        {
            tree = btree;
            Child = child;
        }
        
        public override void SetTree(BTree newTree)
        {
            tree = newTree;
            Child.SetTree(newTree);
        }
        
        public override void SetOwner(Character owner)
        {
            base.owner = owner;
            Child.SetOwner(owner);
        }
        
        public override void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(GetType().Name);
            Child.WriteXml(w);
            w.WriteEndElement();
        }
    } 
}