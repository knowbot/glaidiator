using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class BTree : MonoBehaviour
    {
        private Node _root = null;

        public Node currentNode;
        public Node _current;
        
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();
        
        protected void Start()
        {
            _root = SetupTree();
            Debug.Log(_root.GetType() + " init");
        }

        private void Update()
        {
            if (_root != null) _root.Evaluate();
        }

        protected abstract Node SetupTree();

        public Node GetRoot()
        {
            return _root;
        }
        
        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        
        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value)) 
                return value;
            
            return null;
        }


        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }
            
            return false;
        }
    }
    
    
}