using System.Collections;
using System.Collections.Generic;
using Glaidiator.Model;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Decorator : Node
    {
        protected Node _child;

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
        
        public override void SetTree(BTree newTree)
        {
            tree = newTree;
            _child.SetTree(newTree);
        }
        
        public override void SetOwner(Character owner)
        {
            _ownerCharacter = owner;
            _child.SetOwner(owner);
        }
        
    }
    
    
}