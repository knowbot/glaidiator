using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Glaidiator.Model;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Glaidiator.BehaviorTree.Base
{
    public abstract class BTree : IXmlSerializable
    {
        private Node _root = null;
        private Transform _transform;

        public Node Root
        {
            get => _root;
            internal set
            {
                _root = value;
                _root?.SetTree(this);
            } 
        }

        public Character Owner
        {
            get => _owner;
            internal set
            {
                _owner = value;
                _root.SetOwner(_owner);
            }
        }

        public Character Enemy
        {
            get => _enemy;
            internal set
            {
                _enemy = value;
                SetData("enemy", _enemy);
            }
        }

        public float Fitness = 0; // remove in favor of evo manager candidate class?
        
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
        private Character _owner;
        private Character _enemy;

        protected BTree(Character owner)
        {
            Owner = owner ?? throw new NullReferenceException("BTree init with null owner");
            Init();
        }

        protected BTree(Node root)
        {
            Root = root;
            Init();
        }

        protected BTree()
        {
            Init();
        }

        private void Init()
        {
            Direction = Vector3.forward;
            Move = false;
            AttackLight = false;
            AttackHeavy = false;
            AttackRanged = false; 
            Block = false;
            Dodge = false;
            Root = SetupTree();
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
            Root?.Evaluate();
        }

        protected abstract Node SetupTree();

        public abstract BTree Clone();

        public static BTree[] Crossover(BTree parent1, BTree parent2)
        {
            BTree c1 = parent1.Clone();
            BTree c2 = parent2.Clone(); 
            List<Node> nodes1 = new List<Node>();
            c1.Root.Flatten(nodes1);
            Node swapNode1 = nodes1[Random.Range(0, nodes1.Count)];

            List<Node> nodes2 = new List<Node>();
            c2.Root.Flatten(nodes2);
            Node swapNode2 = nodes2[Random.Range(0, nodes2.Count)];

            Node nodeP1 = swapNode1.GetParent();
            Node nodeP2 = swapNode2.GetParent();
            
            if (nodeP1 != null) // check if not root
            {
                nodeP1.ReplaceChild(swapNode1, swapNode2);
            }
            else
            {
                c1.Root = nodeP2;
                c1.Root.SetOwner(swapNode1.GetOwner());
                c1.Root.SetParent(null); // a root node has no parent
            }

            if (nodeP2 != null) 
            {
                nodeP2.ReplaceChild(swapNode2, swapNode1);
            }
            else
            {
                c2.Root = swapNode1;
                c2.Root.SetOwner(swapNode2.GetOwner());
                c2.Root.SetParent(null); 
            }

            return new[] {c1, c2};
        }

        
        public BTree Mutate()
        {
            BTree child = Clone();
            List<Node> nodes = new();
            child.Root.Flatten(nodes);

            if (0.8f < Random.Range(0f, 1f)) // 80% chance of mutating a random child
            {
                // select random element in nodes and invoke mutate on it
                nodes[Random.Range(0, nodes.Count)].Mutate();
            }
            else
            {
                // replace random node with sample prototype
                int pSize = EvoManager.Instance.prototypes.Count;
                Node newNode = EvoManager.Instance.prototypes[Random.Range(0, pSize)].Randomized();
                Node oldNode = nodes[Random.Range(0, nodes.Count)];
                Node parent = oldNode.GetParent();
                parent?.ReplaceChild(oldNode, newNode);
            }
            return child;
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

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(GetType().Name);
            Root.WriteXml(w);
            w.WriteEndElement();
        }
    }
    
    
}