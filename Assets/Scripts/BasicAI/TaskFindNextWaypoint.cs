using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class TaskFindNextWaypoint : Node
{
    private Transform _transform;

    public TaskFindNextWaypoint(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        return state;
    }
}