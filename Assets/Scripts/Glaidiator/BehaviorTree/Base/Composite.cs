using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Glaidiator.Model;
using Glaidiator.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Glaidiator.BehaviorTree.Base
{
    public abstract class Composite : Node
    {
        public readonly List<Node> Children = new List<Node>();

        protected Composite() {}

        protected Composite(List<Node> children)
        {
            foreach (Node node in children) Attach(node);
        }


        protected Composite(BTree btree, List<Node> children)
        {
            tree = btree;
            foreach (Node node in children) Attach(node);
        }

        public override int GetDepth()
        {
            var maxDepth = Children.Select(c => c.GetDepth()).Prepend(0).Max();
            return maxDepth + 1;
        }
        public override int GetSize()
        {
            var branchSize = Children.Sum(c => c.GetSize());
            return branchSize + 1;
        }

        #region Children Management

        public void Attach(Node node) // add child
        {
            node.parent = this;
            node.SetTree(tree);
            Children.Add(node);
        }

        public void Detach(Node node) // remove child
        {
            node.parent = null;
            node.SetTree(null);
            Children.Remove(node);
        }

        public override void ReplaceChild(Node oldChild, Node newChild)
        {
            int i = Children.IndexOf(oldChild);
            if (i == -1)
                throw new Exception("Child not found");
            newChild.SetParent(this);
            newChild.SetTree(tree);
            Children[i] = newChild;
        }
        
        // return flat list representation of tree/subtree of this node
        public override void Flatten(List<Node> nodes)
        {
            nodes.Add(this);
            foreach (Node child in Children)
            {
                child.Flatten(nodes);
            }
        }
        
        #endregion
        
        #region Setters
        public override void SetTree(BTree newTree)
        {
            tree = newTree;
            foreach (Node child in Children)
            {
                child.SetTree(newTree);
            }
        }

        // invoke on root to set owner for all nodes in tree
        #endregion

        #region Genetic Programming
        public override Node Clone()
        {
            throw new NotImplementedException();
        }

        public override void Mutate()
        {
            switch (Random.Range(0, 3))
            {
                // remove random child
                case 0 when Children.Count > 0:
                    Detach(Children[Random.Range(0, Children.Count)]);
                    break;
                // add new random child
                case 1:
                    Attach(BTreeFactory.GetRandomNode().Randomized());
                    break;
                // shuffle order of children
                case 2:
                    Children.Shuffle();
                    break;
            }
        }

        public override Node Randomized()
        {
            throw new NotImplementedException();
        }
        
        #endregion

        #region Serialization

        public override void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(GetType().Name);
            foreach (Node child in Children)
            {
                child.WriteXml(w);
            }
            w.WriteEndElement();
        }

        public override void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}