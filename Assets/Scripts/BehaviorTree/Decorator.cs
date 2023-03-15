using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Decorator : Node
    {
        private Node _child;

        public Decorator() : base()
        {
        }

        public Decorator(List<Node> children) : base(children)
        {
            _child = children[0];
        }
    
        public Decorator(Node child)
        {
            _child = child;
        }

        public Decorator(BTree btree, Node child)
        {
            tree = btree;
            _child = child;
        }
        
        
        
    }
    
    
}