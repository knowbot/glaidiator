using System;
using BasicAI;
using Glaidiator;
using Glaidiator.Model;
using Glaidiator.Model.Utils;
using Glaidiator.Presenter;
using UnityEngine;
using Input = Glaidiator.Input;

namespace BehaviorTree
{
    public class BTInputProvider : IInputProvider
    {
        private BTree _btree;
        public String currentNode; // for debug info
        public float currentDistance;
        public Character player;
        private Input _inputs;
        public Input Inputs
        {
            get => _inputs;
            private set => _inputs = value;
        }

        public BTInputProvider(Character owner, Character player)
        {
            _btree = new CustomAshleyBT();
            _btree.SetOwnerChar(owner);
            _btree.SetEnemyChar(player);
            _btree.Init();
        }

        public Input GetInputs()
        {
            _btree.Tick();
            
            //Vector3 dir = btree.Direction;
            //Inputs.move = btree.Move ? MathUtils.Get8DDirection(dir.x, dir.z) : Vector3.zero;

            Vector3 dir = MathStuff.Get8DDirection(_btree.Direction.x, _btree.Direction.z);
            _inputs.facing = _btree.Direction;
            _inputs.move = _btree.Move ? dir : Vector3.zero;
            _inputs.attackLight = _btree.AttackLight;
            _inputs.attackHeavy = _btree.AttackHeavy;
            _inputs.attackRanged = _btree.AttackRanged;
            _inputs.block = _btree.Block;
            _inputs.dodge = _btree.Dodge;

            currentNode = _btree.currentNode.ToString(); // for debug info
            currentDistance = _btree.enemyDistance;
            return Inputs;
        }

        // set and setup new BTree, for runtime use
        public void SetNewBTree(BTree tree, Character owner, Character player)
        {
            _btree = tree;
            _btree.SetOwnerChar(owner);
            _btree.SetEnemyChar(player);
        }
    }
}