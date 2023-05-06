using System;
using System.Collections;
using System.Collections.Generic;
using BasicAI;
using BehaviorTree;
using Glaidiator.Model;
using UnityEngine;

public class xTaskAttack : Node // deprecated
{

    private Movement _transform;
    
    public xTaskAttack(BTree bTree, Character transform)
    {
        tree = bTree;
        _transform = transform.Movement;
    }

    public xTaskAttack(BTree bTree, Movement transform)
    {
        tree = bTree;
        _transform = transform;
    }

    public xTaskAttack(Character transform)
    {
        _transform = transform.Movement;
    }
    
    public override NodeState Evaluate()
    {
        if (tree == null) throw new NullReferenceException();
        
        Movement target = (Movement)GetData("target");
        if (target == null)
        {
            state = NodeState.FAILURE;
            return state;
        }
        
        var currPos = _transform.Position;
        var targetPos = target.Position;

        tree.Direction = ((targetPos - currPos).normalized).xz();
        
        if (Vector3.Distance(currPos, targetPos) <= xBossBT.lightAtkRange)
        {
            tree.AttackLight = true;
            // TODO: Add attack collision
        }
        else
        {
            tree.AttackLight = false;
            tree.Move = true;
        }

        //if (tree.GetPlayerChar().state == )
        // TODO: return State = success if target is dead etc.

        state = NodeState.RUNNING;
        return state;
    }

    public override Node Clone()
    {
        Node clone = new xTaskAttack(tree, _ownerCharacter);
        return clone;
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

