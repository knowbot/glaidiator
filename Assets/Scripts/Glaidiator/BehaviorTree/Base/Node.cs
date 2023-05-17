using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Glaidiator.Model;

// ReSharper disable once InvalidXmlDocComment
/**
    * Initial version based on Mina Pêcheux tutorial at:
    * https://medium.com/geekculture/how-to-create-a-simple-behaviour-tree-in-unity-c-3964c84c060e
    * https://github.com/MinaPecheux/UnityTutorials-BehaviourTrees
    *
    * Modified to support evolution
*/

namespace Glaidiator.BehaviorTree.Base
{
    public enum NodeState
    {
        RUNNING, SUCCESS, FAILURE
    }
    public abstract class Node : IXmlSerializable
    {
        // TODO: consider access levels
        protected NodeState state;
        protected BTree tree;
        protected Character owner;
        protected Node parent;
        public readonly List<Node> Children = new List<Node>();


        public Node() 
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node node in children) Attach(node);
        }

        public Node(BTree btree)
        {
            tree = btree;
        }
        
        public Node(BTree btree, List<Node> children)
        {
            tree = btree;
            foreach (Node node in children) Attach(node);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        public abstract Node Clone();

        public abstract void Mutate();
        
        public abstract Node Randomized();
        

        public void Attach(Node node) // add child
        {
            node.parent = this;
            node.SetTree(tree);
            Children.Add(node);
        }

        public void Detach(Node node) // remove child
        {
            if (Children.Remove(node))
            {
                
            }
            else
            {
                // throw error?
            }
        }

        public void ReplaceChild(Node oldChild, Node newChild)
        {
            int i = Children.IndexOf(oldChild);
            newChild.SetParent(this);
            newChild.SetTree(this.tree);
            newChild.SetOwner(this.owner);
            Children[i] = newChild;
        }
        
        // return flat list representation of tree/subtree of this node
        public virtual void Flatten(List<Node> nodes)
        {
            nodes.Add(this);
            foreach (Node child in Children)
            {
                child.Flatten(nodes);
            }
        }

        public Node GetParent()
        {
            return parent;
        }

        public void SetParent(Node nParent)
        {
            this.parent = nParent;
        }

        public BTree GetTree()
        {
            return tree;
        }

        // invoke on root to set tree for all nodes in tree
        public virtual void SetTree(BTree newTree)
        {
            tree = newTree;
            foreach (Node child in Children)
            {
                child.SetTree(newTree);
            }
        }

        // invoke on root to set owner for all nodes in tree
        public virtual void SetOwner(Character owner)
        {
            this.owner = owner;
            foreach (Node child in Children)
            {
                child.SetOwner(owner);
            }
        }

        public Character GetOwner()
        {
            return owner;
        }
        
        public void SetData(string key, object value)
        {
            tree.SetData(key, value);
        }

        
        public object GetData(string key)
        {
            return tree.GetData(key);
        }


        public bool ClearData(string key)
        {
            return tree.ClearData(key);
        }

        public string Serialize()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            Type myType = this.GetType();
            
            sb.Append("<" + myType + ">");
            foreach (Node child in Children)
            {
                sb.AppendLine();
                sb.Append("\t" + child.Serialize());
            }
            sb.AppendLine();
            sb.Append("</" + myType + ">");
            return sb.ToString();
        }

        public XmlSchema GetSchema()
        {
            return(null);
        }

        public virtual void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public virtual void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(GetType().Name);
            foreach (var child in Children)
            {
                child.WriteXml(w);
            }
            w.WriteEndElement();
        }
    }
    
}











