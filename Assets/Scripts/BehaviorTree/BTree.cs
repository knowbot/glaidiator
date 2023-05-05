using System;
using System.Collections;
using System.Collections.Generic;
using Glaidiator.Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BehaviorTree
{
    public abstract class BTree
    {
        protected Transform _transform;
        protected Node _root = null;
        protected Character _enemyChar;
        protected Character _ownerChar;
        protected float _fitness; // remove in favor of evo manager candidate class?
        
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Vector3 Direction;

        public bool Move;
        public bool AttackLight;
        public bool AttackHeavy;
        public bool AttackRanged;
        public bool Block;
        public bool Dodge;

        public Node currentNode;

        public BTree(Character owner)
        {
            if (owner == null) throw new NullReferenceException("BTree init with null owner");
            _ownerChar = owner;
        }

        public BTree(Node root)
        {
            SetRoot(root);
        }

        public BTree() { }

        public void Awake()
        {
            Direction = Vector3.forward;
            Move = false;
            AttackLight = false;
            AttackHeavy = false;
            AttackRanged = false; 
            Block = false;
            Dodge = false;
        }

        public void Start()
        {
            _root = SetupTree();
            Debug.Log(_root.GetType() + " init");
        }

        public void Update()
        {
            Move = false;
            AttackLight = false;
            AttackHeavy = false;
            AttackRanged = false; 
            Block = false;
            Dodge = false;
            if (_root != null) _root.Evaluate();
        }

        protected abstract Node SetupTree();

        public abstract BTree Clone();

        public void Crossover(BTree mate, float chance)
        {
            if (chance < Random.Range(0f, 1f)) return;

            List<Node> nodes1 = new List<Node>();
            _root.Flatten(nodes1);
            Node swapNode1 = nodes1[Random.Range(0, nodes1.Count)];

            List<Node> nodes2 = new List<Node>();
            mate._root.Flatten(nodes2);
            Node swapNode2 = nodes2[Random.Range(0, nodes2.Count)];

            Node parent1 = swapNode1.GetParent();
            Node parent2 = swapNode2.GetParent();
            
            if (parent1 != null) // check if not root
            {
                parent1.ReplaceChild(swapNode1, swapNode2);
            }
            else
            {
                _root = parent2;
                _root.SetOwner(swapNode1.GetOwner());
                _root.SetParent(null); // a root node has no parent
            }

            if (parent2 != null) 
            {
                parent2.ReplaceChild(swapNode2, swapNode1);
            }
            else
            {
                mate._root = swapNode1;
                mate._root.SetOwner(swapNode2.GetOwner());
                mate._root.SetParent(null); 
            }
            
            // dirty flag?
        }

        
        public void Mutate(float chance)
        {
            if (chance < Random.Range(0f, 1f)) return;

            List<Node> nodes = new List<Node>();
            _root.Flatten(nodes);

            if (0.8f < Random.Range(0f, 1f)) // 80% chance of mutating a random child
            {
                // select random element in nodes and invoke mutate on it
                nodes[Random.Range(0, nodes.Count)].Mutate();
            }
            else
            {
                // sample node from EvolutionManager
                // replace random existing node with the sample
                int pSize = EvolutionManager.prototypes.Count;
                Node newNode = EvolutionManager.prototypes[Random.Range(0, pSize)].Randomized();
                Node oldNode = nodes[Random.Range(0, nodes.Count)];
                Node parent = oldNode.GetParent();
                if (parent != null) parent.ReplaceChild(oldNode, newNode);
            }
            
            // add dirty flag?
        }

        public void SetEnemyChar(Character enemy)
        {
            _enemyChar = enemy;
        }

        public Character GetEnemyChar()
        {
            return _enemyChar;
        }

        public void SetOwnerChar(Character owner)
        {
            _ownerChar = owner;
        }
        
        public Character GetOwnerChar()
        {
            if (_ownerChar == null) throw new NullReferenceException("tree has no owner character");
            return _ownerChar;
        }
        
        public Node GetRoot()
        {
            return _root;
        }

        public void SetRoot(Node newRoot)
        {
            _root = newRoot;
        }

        public float GetFitness()
        {
            // TODO: Implement fitness calculation
            return _fitness;
        }

        public void SetTransform(Transform newTransform)
        {
            _transform = newTransform;
        }

        public Transform GetTransform()
        {
            return _transform;
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