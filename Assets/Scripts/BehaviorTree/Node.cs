using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BehaviorTree
{
    public enum NodeState
    {
        RUNNING, SUCCESS, FAILURE
    }

    
    public class Node : MonoBehaviour
    {

        protected NodeState state;

        public Node parent;
        private List<Node> children = new List<Node>();
        
        
        public Node() 
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node node in children) Attach(node);
        }

        private void Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }
        
        
        
    }
    
    
}