using System;
using System.Collections;
using System.Collections.Generic;
using BasicAI;
using Glaidiator;
using Glaidiator.Model.Utils;
using Glaidiator.Presenter;
using UnityEngine;
using Input = Glaidiator.Input;

namespace BehaviorTree
{
    public class AIContainer : AInputProvider
    {
        private BTree btree;

        public GameObject PlayerObject;
        
        private void Awake()
        {
            //btree = new BossBT(transform);s
            btree = new BossBT();
            btree.Awake();
        }

        void Start()
        {
            btree.SetOwnerChar(GetComponent<CharacterPresenter>().GetCharacter());
            btree.SetEnemyChar(PlayerObject.GetComponent<PlayerCharacterPresenter>().GetCharacter());
            btree.Start();
        }

        void Update()
        {
            btree.Update();
            Vector3 dir = btree.Direction.x0y();
            //Vector2 dir = btree.Direction;
            Inputs.move = btree.Move ? MathUtils.Get8DDirection(dir.x, dir.z) : Vector3.zero;
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