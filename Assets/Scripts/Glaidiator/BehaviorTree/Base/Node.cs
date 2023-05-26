using System.Collections.Generic;
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
    * Modified to support genetic programming
*/

namespace Glaidiator.BehaviorTree.Base
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public abstract class Node : IXmlSerializable
    {
        // TODO: consider access levels
        internal NodeState state;
        internal BTree tree;
        internal Node parent;

        public Node()
        {
            parent = null;
        }

        public Node(BTree btree)
        {
            tree = btree;
        }

        public abstract NodeState Evaluate();

        public abstract void Flatten(List<Node> nodes);
        public abstract void ReplaceChild(Node oldChild, Node newChild);

        #region Genetic Programming

        public abstract Node Clone();

        public abstract void Mutate();

        public abstract Node Randomized();

        #endregion

        #region Getters and Setters
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
        
        #endregion

        public XmlSchema GetSchema()
        {
            return (null);
        }

        public abstract void ReadXml(XmlReader reader);
        public abstract void WriteXml(XmlWriter w);
    }
}











