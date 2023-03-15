using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Based on Mina Pêcheux tutorial at:
 * https://medium.com/geekculture/how-to-create-a-simple-behaviour-tree-in-unity-c-3964c84c060e
 * https://github.com/MinaPecheux/UnityTutorials-BehaviourTrees
 */

namespace BehaviorTree
{
    public enum NodeState
    {
        RUNNING, SUCCESS, FAILURE
    }
    
    public class Node
    {

        protected NodeState state;
        protected BTree tree;

        public Node parent;
        protected List<Node> children = new List<Node>();

        //private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

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

        private void Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

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











