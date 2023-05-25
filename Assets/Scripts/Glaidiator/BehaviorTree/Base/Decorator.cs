using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using Glaidiator.Model;

namespace Glaidiator.BehaviorTree.Base
{
    public abstract class Decorator<T> : Node where T : Node
    {
        private T _child;

        public T Child
        {
            get => _child;
            protected set
            {
                _child = value;
                _child.SetParent(this);
            }
        }

        public Decorator() : base()
        {
            
        }

        public Decorator(T child)
        {
            Child = child;
            child.SetParent(this);
        }

        protected Decorator(BTree btree, T child)
        {
            tree = btree;
            Child = child;
            child.SetParent(this);
        }

        public override void Flatten(List<Node> nodes)
        {
            nodes.Add(this);
            Child.Flatten(nodes);
        }
        
        public override void ReplaceChild(Node oldChild, Node newChild)
        {
            if (oldChild as T != Child) throw new ConstraintException("Child replaced different from current child");
            newChild.SetParent(this);
            newChild.SetTree(tree);
            Child = newChild as T;
        }

        #region Getters and Setters
        
        public override void SetTree(BTree newTree)
        {
            tree = newTree;
            Child.SetTree(newTree);
        }
        #endregion
        public override void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public override void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(GetType().Name);
            Child.WriteXml(w);
            w.WriteEndElement();
        }
    } 
}