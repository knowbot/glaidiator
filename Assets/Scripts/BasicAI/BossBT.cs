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

        protected override Node SetupTree()
        {
            /*
            Node root = new Repeater(new Selector(this, new List<Node>
            {
                new Sequence(this, new List<Node>
                {
                    new CheckEnemyInRange(this, _transform),
                    new TaskGoToTarget(this, _transform),
                }),
                new TaskPatrol(this, _transform, waypoints),
            }));
            */
            //Node root = new TaskPatrol(this, _transform);
            //Node root = new TaskPatrol(this, GetBossChar());
            Node root = new Selector(this, new List<Node>
            {
                new Sequence(this, new List<Node>
                {
                    new CheckEnemyInRange(this, GetBossChar()),
                    new TaskGoToTarget(this, GetBossChar()),
                }),
                new TaskPatrol(this, GetBossChar()),
            });
            return root;
        }

        public BossBT(Transform transform) : base(transform)
        {
        }

        public BossBT(Character character) : base(character)
        {
        }
    }
}