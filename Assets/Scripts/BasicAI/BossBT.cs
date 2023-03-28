using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

namespace BasicAI
{
    
    public class BossBT : BTree
    {

        public List<Vector2> waypoints = new List<Vector2>();

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
            Node root = new TaskPatrol(this, _transform, waypoints);

            return root;
        }

        public BossBT(Transform transform) : base(transform)
        {
        }
    }
}