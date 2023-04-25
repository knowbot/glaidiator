using System.Collections;
using System.Collections.Generic;
using Glaidiator.Model;
using UnityEngine;


/**
 * Initial version based on Mina Pêcheux tutorial at:
 * https://medium.com/geekculture/how-to-create-a-simple-behaviour-tree-in-unity-c-3964c84c060e
 * https://github.com/MinaPecheux/UnityTutorials-BehaviourTrees
 *
 * Modified to support evolution
 */

namespace BehaviorTree
{
    public enum NodeState
    {
        RUNNING, SUCCESS, FAILURE
    }
    
    public abstract class Node
    {
        // TODO: consider access levels
        protected NodeState state;
        protected BTree tree;
        protected Character _ownerCharacter;
        protected Node parent;
        protected List<Node> children = new List<Node>();


        public Node() 
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node node in children) Attach(node);
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
            children.Add(node);
        }

        public void Detach(Node node) // remove child
        {
            if (children.Remove(node))
            {
                
            }
            else
            {
                // throw error?
            }
        }

        public void ReplaceChild(Node oldChild, Node newChild)
        {
            int i = children.IndexOf(oldChild);
            newChild.SetParent(this);
            newChild.SetTree(this.tree);
            children[i] = newChild;
        }
        
        // return flat list representation of tree/subtree of this node
        public virtual void Flatten(List<Node> nodes)
        {
            nodes.Add(this);
            foreach (Node child in children)
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

        public void SetTree(BTree newTree)
        {
            tree = newTree;
        }

        public void SetOwner(Character owner)
        {
            _ownerCharacter = owner;
        }

        public Character GetOwner()
        {
            return _ownerCharacter;
        }
        
        public void SetData(string key, object value)
        {
            //_dataContext[key] = value;
            tree.SetData(key, value);
        }

        
        public object GetData(string key)
        {
            /*
            object value = null;
            if (_dataContext.TryGetValue(key, out value)) 
                return value;
            
            Node node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null) 
                    return value;
                
                node = node.parent;
            }
            return null;
            */
            return tree.GetData(key);
        }


        public bool ClearData(string key)
        {
            /*
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared) return true;
                node = node.parent;
            }
            return false;
            */
            return tree.ClearData(key);
        }
        
        
        
    }
    
    
}











