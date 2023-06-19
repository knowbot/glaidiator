using System;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using Glaidiator.Presenter;
using Glaidiator.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using Input = Glaidiator.Model.Input;

namespace Glaidiator.BehaviorTree
{
    public class AIContainer : MonoBehaviour, IInputProvider
    {
        private BTree _tree;
        private Character _owner;
        private Character _enemy;

        public BTree Tree
        {
            get => _tree;
            set
            {
                _tree = value;
                _tree.Owner = _owner;
                _tree.Enemy = _enemy;
            }
        }
        
        public char selectTree;
        public String currentNode; // for debug info
        public float currentDistance;
        public GameObject EnemyObject;
        private Input _inputs;
        public Input Inputs
        {
            get => _inputs;
            private set => _inputs = value;
        }

        private void Start()
        {
            _owner = GetComponent<CharacterPresenter>().GetCharacter();
            _enemy = EnemyObject.GetComponent<CharacterPresenter>().GetCharacter();
            switch (selectTree) //editor tree selection
            {
                case 'a':
                    Tree = BTreeFactory.CreateAshley();
                    break;
                case 'b':
                    Tree = BTreeFactory.CreateBob();
                    break;
                case 'c':
                    Tree = BTreeFactory.CreateCharlie();
                    break;
                case 'd':
                    break;    
                case 'e':
                    Tree = BTreeFactory.CreateBaselineEvo();
                    break;
                case 'f':
                    break;
                case 'g': // g is for groot
                    break;
                case 't':
                    Tree = BTreeFactory.CreateTreeTester();
                    break;
                case 'z':
                    Tree = BTreeFactory.CreateEmpty();
                    break;
                default:
                    Tree = BTreeFactory.CreateAshley();
                    break;
            }
        }

        private void Update()
        {
            _tree.Tick();
            
            //Vector3 dir = btree.Direction;
            //Inputs.move = btree.Move ? MathUtils.Get8DDirection(dir.x, dir.z) : Vector3.zero;

            Vector3 dir = MathStuff.Get8DDirection(_tree.Direction.x, _tree.Direction.z);
            //_inputs.facing = btree.Direction;
            _inputs.facing = dir;
            _inputs.move = _tree.Move ? dir : Vector3.zero;
            _inputs.attackLight = _tree.AttackLight;
            _inputs.attackHeavy = _tree.AttackHeavy;
            _inputs.attackRanged = _tree.AttackRanged;
            _inputs.block = _tree.Block;
            _inputs.dodge = _tree.Dodge;
            _inputs.wait = _tree.Wait;

            currentNode = _tree.Active.ToString(); // for debug info
            currentDistance = _tree.enemyDistance;
        }

        public Input GetInputs()
        {
            return Inputs;
        }
    }
    
    
}