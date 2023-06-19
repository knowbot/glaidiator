using System;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using Glaidiator.Presenter;
using Glaidiator.Utils;
using UnityEngine;
using Input = Glaidiator.Model.Input;

namespace Glaidiator.BehaviorTree
{
    // TODO: change to integrate with SimManager
    public class BTInputProvider : IInputProvider
    {
        private BTree _btree;
        public String currentNode; // for debug info
        public float currentDistance;
        public Character Player;
        private Input _inputs;
        public Input Inputs
        {
            get => _inputs;
            private set => _inputs = value;
        }

        public BTInputProvider(BTree tree, Character owner, Character enemy)
        {
            _btree = tree;
            _btree.Owner = owner;
            _btree.Enemy = enemy;
        }

        public Input GetInputs()
        {
            _btree.Tick();
            
            //Vector3 dir = btree.Direction;
            //Inputs.move = btree.Move ? MathUtils.Get8DDirection(dir.x, dir.z) : Vector3.zero;

            Vector3 dir = MathStuff.Get8DDirection(_btree.Direction.x, _btree.Direction.z);
            //_inputs.facing = _btree.Direction;
            _inputs.facing = dir;
            _inputs.move = _btree.Move ? dir : Vector3.zero;
            _inputs.attackLight = _btree.AttackLight;
            _inputs.attackHeavy = _btree.AttackHeavy;
            _inputs.attackRanged = _btree.AttackRanged;
            _inputs.block = _btree.Block;
            _inputs.dodge = _btree.Dodge;
            _inputs.wait = _btree.Wait;

            currentNode = _btree.Active.ToString(); // for debug info
            currentDistance = _btree.enemyDistance;
            return _inputs;
        }

        // set and setup new BTree, for runtime use
        public void SetNewBTree(BTree tree, Character owner, Character player)
        {
            _btree = tree;
            _btree.Owner = owner;
            _btree.Enemy = player;
        }
    }
}