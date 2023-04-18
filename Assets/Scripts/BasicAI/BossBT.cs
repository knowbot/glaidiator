using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Glaidiator.Model;
using UnityEngine;

namespace BasicAI
{
    
    public class BossBT : BTree
    {

        public static float aggroRange = 3f;
        public static float lightAtkRange = 1.5f;

        protected override Node SetupTree()
        {
            
            Node root = new Selector(this, new List<Node>
            {
                new Sequence(this, new List<Node>
                {
                    new CheckEnemyInRange(this, GetOwnerChar()),
                    new TaskGoToTarget(this, GetOwnerChar()),
                    new TaskAttack(this, GetOwnerChar())
                }),
                new TaskPatrol(this, GetOwnerChar()),
            });
            return root;
        }

        public override BTree Clone()
        {
            BTree newTree = new BossBT(_ownerChar);
            newTree.SetRoot(_root.Clone());
            return newTree;
        }

        //public BossBT(Transform transform) : base(transform)
        //{
        //}

        public BossBT(Character character) : base(character)
        {
        }
    }
}