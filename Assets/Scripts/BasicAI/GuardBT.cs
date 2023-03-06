using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Tree = BehaviorTree.Tree;

public class GuardBT : Tree
{

    public Transform[] waypoints;

    public static float speed = 2f;
    
    protected override Node SetupTree()
    {
        Node root = new TaskPatrol(transform, waypoints);
        return root;
    }
}
