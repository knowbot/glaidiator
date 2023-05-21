using System;
using System.Collections.Generic;
using Glaidiator.BehaviorTree.CustomBTs;
using Glaidiator.Model;
using Glaidiator.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Glaidiator.BehaviorTree.Base
{
    public abstract class BTree
    {
        protected Transform transform;
        protected Node root = null;
        protected Character enemy;
        protected Character owner;
        protected float fitness = 0; // remove in favor of evo manager candidate class?
        
        private readonly Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Vector3 Direction;

        public bool Move;
        public bool AttackLight;
        public bool AttackHeavy;
        public bool AttackRanged;
        public bool Block;
        public bool Dodge;

        // for editor debugging info
        public Node currentNode;
        public float enemyDistance;
        

        public BTree(Character owner)
        {
            this.owner = owner ?? throw new NullReferenceException("BTree init with null owner");
        }

        public BTree(Node root)
        {
            SetRoot(root);
        }

        public BTree()
        { }

        public void Init()
        {
            Direction = Vector3.forward;
            Move = false;
            AttackLight = false;
            AttackHeavy = false;
            AttackRanged = false; 
            Block = false;
            Dodge = false;
            root = SetupTree();
            //Debug.Log(this.GetType() + " init");
        }

        public void Tick()
        {
            Move = false;
            AttackLight = false;
            AttackHeavy = false;
            AttackRanged = false; 
            Block = false;
            Dodge = false;
            if (root != null) root.Evaluate();
        }

        protected abstract Node SetupTree();

        public abstract BTree Clone();

        public BTree Crossover(BTree mate)
        {
            Debug.Log("Starting crossover!");
            BTree child = new EvoBT();

            List<Node> nodes1 = new List<Node>();
            root.Flatten(nodes1);
            Node swapNode1 = nodes1[Random.Range(0, nodes1.Count)].Clone();
            Serializer.Serialize(swapNode1);

            List<Node> nodes2 = new List<Node>();
            mate.root.Flatten(nodes2);
            Node swapNode2 = nodes2[Random.Range(0, nodes2.Count)].Clone();
            Serializer.Serialize(swapNode2);
            
            Node parent1 = swapNode1.GetParent();
            Node parent2 = swapNode2.GetParent();
            
            if (parent1 != null) // check if not root
            {
                parent1.ReplaceChild(swapNode1, swapNode2);
            }
            else
            {
                child.SetRoot(swapNode2);
                child.root.SetParent(null); // a root node has no parent
            }

            if (parent2 != null) 
            {
                parent2.ReplaceChild(swapNode2, swapNode1);
            }
            else
            {
                child.SetRoot(swapNode1);
                child.root.SetParent(null); 
            }
            Debug.Log("Crossed over!");
            return child;
        }

        
        public void Mutate(float chance)
        {
            if (chance < Random.Range(0f, 1f)) return;

            List<Node> nodes = new List<Node>();
            root.Flatten(nodes);

            if (0.8f < Random.Range(0f, 1f)) // 80% chance of mutating a random child
            {
                // select random element in nodes and invoke mutate on it
                nodes[Random.Range(0, nodes.Count)].Mutate();
            }
            else
            {
                // sample node from EvolutionManager
                // replace random existing node with the sample
                int pSize = EvoManager.Instance.prototypes.Count;
                Node newNode = EvoManager.Instance.prototypes[Random.Range(0, pSize)].Randomized();
                Node oldNode = nodes[Random.Range(0, nodes.Count)];
                Node parent = oldNode.GetParent();
                parent?.ReplaceChild(oldNode, newNode);
            }
            
            // add dirty flag?
        }

        public void SetEnemyChar(Character enemy)
        {
            this.enemy = enemy;
        }

        public Character GetEnemyChar()
        {
            return enemy;
        }

        public void SetOwnerChar(Character owner)
        {
            this.owner = owner;
        }
        
        public Character GetOwnerChar()
        {
            if (owner == null) throw new NullReferenceException("tree has no owner character");
            return owner;
        }
        
        public Node GetRoot()
        {
            return root;
        }

        public void SetRoot(Node newRoot)
        {
            root = newRoot;
            root.SetTree(this);
        }

        public float GetFitness()
        {
            // TODO: Implement fitness calculation
            return fitness;
        }

        public void SetTransform(Transform newTransform)
        {
            transform = newTransform;
        }

        public Transform GetTransform()
        {
            return transform;
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