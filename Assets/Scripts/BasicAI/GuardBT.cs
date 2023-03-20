using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Tree = BehaviorTree.Tree;

namespace BasicAI
{
    public class GuardBT : Tree
    {
        private Transform _transform;

        //public Transform[] waypoints;
        public List<Vector3> waypoints;

        public static float speed = 2f;
        public static float fovRange = 4f;
        public static float attackRange = 0.5f;
        public static float patrolStep = 2f;
        public static bool isPatrolRight = true;


        protected override Node SetupTree()
        {
            Debug.Log("Setup GuardBT");
            _transform = transform;
            //Node root = new TaskPatrol(transform, waypoints);

            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new CheckEnemyInRange(_transform),
                    new TaskGoToTarget(_transform),
                }),
                new TaskPatrol(_transform, waypoints),
            });


            return root;
        }



    }
}