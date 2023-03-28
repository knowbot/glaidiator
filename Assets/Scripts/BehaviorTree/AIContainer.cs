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
            Inputs.move = btree.Move ? dir : Vector3.zero;
            //Inputs.attackLight = _playerActions.Gameplay.AttackLight.WasPressedThisFrame();
            //Inputs.attackHeavy  = _playerActions.Gameplay.AttackHeavy.WasPressedThisFrame();
            //Inputs.attackRanged  = _playerActions.Gameplay.AttackRanged.WasPressedThisFrame();
            //Inputs.block  = _playerActions.Gameplay.Block.WasPressedThisFrame();
            //Inputs.dodge  = _playerActions.Gameplay.Dodge.WasPressedThisFrame();
            //Inputs.move = GetCameraRelativeMovement(_playerActions.Gameplay.Move.ReadValue<Vector2>());
        }

        public override Input GetInputs()
        {
            return Inputs;
        }
    }
    
    
}