using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class TaskAttack : Node
{

    public TaskAttack()
    {
        
    }

    public override NodeState Evaluate()
    {
        
        
        
        state = NodeState.RUNNING;
        return state;
    }
}

