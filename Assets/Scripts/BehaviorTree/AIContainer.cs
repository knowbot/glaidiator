using System;
using System.Collections;
using System.Collections.Generic;
using BasicAI;
using Glaidiator;
using UnityEngine;
using Input = Glaidiator.Input;

namespace BehaviorTree
{
    public class AIContainer : AInputProvider
    {
        private BTree btree;

        
        private void Awake()
        {
            btree = new BossBT(transform);
            btree.Awake();
        }

        void Start()
        {
            btree.Start();
        }

        void Update()
        {
            btree.Update();
            Vector3 dir = new Vector3(btree.Direction.x, 0f, btree.Direction.y);
            //Vector2 dir = btree.Direction;
            Inputs.move = btree.Move ? dir : Vector3.zero;
            Inputs.attackLight = btree.AttackLight;
            Inputs.attackHeavy = btree.AttackHeavy;
            Inputs.attackRanged = btree.AttackRanged;
            Inputs.block = btree.Block;
            Inputs.dodge = btree.Dodge;
            
        }

        public override Input GetInputs()
        {
            return Inputs;
        }
    }
    
    
}