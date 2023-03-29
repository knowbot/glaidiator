using System;
using System.Collections;
using System.Collections.Generic;
using Glaidiator.Model;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class BTree
    {
        protected Transform _transform;
        protected Node _root = null;
        protected Character _playerChar;
        protected Character _bossChar;

        public Node currentNode;
        public Node _current;
        
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Vector2 Direction;
        public bool Move;
        public bool AttackLight;
        public bool AttackHeavy;
        public bool AttackRanged;
        public bool Block;
        public bool Dodge;

        public BTree(Transform transform)
        {
            _transform = transform;
        }

        public BTree(Character character)
        {
            _bossChar = character;
        }

        public void Awake()
        {
            Direction = Vector2.down;
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
            if (_root != null) _root.Evaluate();
        }

        protected abstract Node SetupTree();

        public void SetPlayerChar(Character player)
        {
            _playerChar = player;
        }

        public Character GetPlayerChar()
        {
            return _playerChar;
        }

        public Character GetBossChar()
        {
            return _bossChar;
        }
        
        public Node GetRoot()
        {
            return _root;
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