using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;

        public Node currentNode;
        
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
    }
    
    
}