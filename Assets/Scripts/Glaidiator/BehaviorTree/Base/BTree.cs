using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Glaidiator.Model;
using Glaidiator.Utils;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Glaidiator.BehaviorTree.Base
{
    public class BTree : IXmlSerializable
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

        public Character Owner { get; internal set; }

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
        private Character _enemy;

        public BTree(Character owner)
        {
            Owner = owner ?? throw new NullReferenceException("BTree init with null owner");
            Init();
        }

        public BTree(Node root)
        {
            Root = root;
            root.SetTree(this);
            SetData("enemy", Enemy);
            Init();
        }

        public BTree()
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

        public BTree Clone()
        {
           return new BTree
            {
                Root = Root.Clone(),
                Fitness = Fitness
            };
        }

        public static BTree Crossover(BTree parent1, BTree parent2)
        {
            BTree p1 = parent1.Clone();
            BTree p2 = parent2.Clone();
            List<Node> nodes1 = new();
            p1.Root.Flatten(nodes1);
            Node swapNode1 = nodes1[Random.Range(0, nodes1.Count)]; // any node except root
            List<Node> nodes2 = new();
            p2.Root.Flatten(nodes2);
            Node swapNode2 = nodes2[Random.Range(0, nodes2.Count)];
            Node nodeP1 = swapNode1.GetParent();
            if(nodeP1 != null) // is not root
                nodeP1.ReplaceChild(swapNode1, swapNode2);
            else
            {
                p1.Root = swapNode2;
                p1.Root.SetParent(null);
            }
            p1.Fitness = 0;
            return p1;
        }

        
        public void Mutate()
        {
            List<Node> nodes = new();
            Root.Flatten(nodes);
            // select random element in nodes and invoke mutate on it
            nodes[Random.Range(0, nodes.Count)].Mutate();
        }
        
        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        
        public object GetData(string key)
        {
            return _dataContext.TryGetValue(key, out object value) ? value : null;
        }


        public bool ClearData(string key)
        {
            if (!_dataContext.ContainsKey(key)) return false;
            _dataContext.Remove(key);
            return true;

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
            w.WriteAttributeString("fitness", Fitness.ToString());
            Root.WriteXml(w);
            w.WriteEndElement();
        }
    }
    
    
}