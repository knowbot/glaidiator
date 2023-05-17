using System;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.BehaviorTree.CustomBTs;
using Glaidiator.Presenter;
using Glaidiator.Utils;
using UnityEngine;
using Input = Glaidiator.Model.Input;

namespace Glaidiator.BehaviorTree
{
    public class AIContainer : MonoBehaviour, IInputProvider
    {
        private BTree btree;
        public char selectTree;
        public String currentNode; // for debug info
        public float currentDistance;
        public GameObject PlayerObject;
        private Input _inputs;
        public Input Inputs
        {
            get => _inputs;
            private set => _inputs = value;
        }

        private void Start()
        {
            switch (selectTree) //ugly hack to allow tree selection in editor
            {
                case 'a':
                    btree = new CustomAshleyBT();
                    break;
                case 'b':
                    btree = new CustomBobBT();
                    break;
                case 'c':
                    btree = new CustomCharlieBT();
                    break;
                case 'd':
                    btree = new CustomMaximusDecimusMeridiusBT();
                    break;
                case 'e':
                    btree = new EvoBT();
                    break;
                default:
                    btree = new CustomAshleyBT();
                    break;
            }
            
            btree.SetOwnerChar(GetComponent<CharacterPresenter>().GetCharacter());
            btree.SetEnemyChar(PlayerObject.GetComponent<CharacterPresenter>().GetCharacter());
            btree.Init();
            TreeSerializer.Serialize(btree);
        }

        private void Update()
        {
            btree.Tick();
            
            //Vector3 dir = btree.Direction;
            //Inputs.move = btree.Move ? MathUtils.Get8DDirection(dir.x, dir.z) : Vector3.zero;

            Vector3 dir = MathStuff.Get8DDirection(btree.Direction.x, btree.Direction.z);
            //_inputs.facing = btree.Direction;
            _inputs.facing = dir;
            _inputs.move = btree.Move ? dir : Vector3.zero;
            _inputs.attackLight = btree.AttackLight;
            _inputs.attackHeavy = btree.AttackHeavy;
            _inputs.attackRanged = btree.AttackRanged;
            _inputs.block = btree.Block;
            _inputs.dodge = btree.Dodge;

            currentNode = btree.currentNode.ToString(); // for debug info
            currentDistance = btree.enemyDistance;
        }

        public Input GetInputs()
        {
            return Inputs;
        }

        // set and setup new BTree, for runtime use
        public void SetCurrentBTree(BTree tree)
        {
            btree = tree;
            btree.SetOwnerChar(GetComponent<CharacterPresenter>().GetCharacter());
            btree.SetEnemyChar(PlayerObject.GetComponent<PlayerCharacterPresenter>().GetCharacter());
            btree.Init();
        }
    }
    
    
}