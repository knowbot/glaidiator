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

    public override Node Clone()
    {
        throw new System.NotImplementedException();
    }

    public override void Mutate()
    {
        throw new System.NotImplementedException();
    }

    public override Node Randomized()
    {
        throw new System.NotImplementedException();
    }
}