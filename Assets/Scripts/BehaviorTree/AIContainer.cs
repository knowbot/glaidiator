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
        public String currentNode; // for debug info
        public GameObject PlayerObject;
        
        private void Awake()
        {
            //btree = new BossBT();
            btree = new CustomNodesBT();
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
            
            //Vector3 dir = btree.Direction;
            //Inputs.move = btree.Move ? MathUtils.Get8DDirection(dir.x, dir.z) : Vector3.zero;
            Vector3 dir = MathStuff.Get8DDirection(btree.Direction.x, btree.Direction.z);
            btree.Direction = dir;
            Inputs.move = btree.Move ? dir : Vector3.zero;
            
            Inputs.attackLight = btree.AttackLight;
            Inputs.attackHeavy = btree.AttackHeavy;
            Inputs.attackRanged = btree.AttackRanged;
            Inputs.block = btree.Block;
            Inputs.dodge = btree.Dodge;

            currentNode = btree.currentNode.ToString(); // for debug info
        }

        public override Input GetInputs()
        {
            return Inputs;
        }

        // set and setup new BTree, for runtime use
        public void SetCurrentBTree(BTree tree)
        {
            btree = tree;
            btree.SetOwnerChar(GetComponent<CharacterPresenter>().GetCharacter());
            btree.SetEnemyChar(PlayerObject.GetComponent<PlayerCharacterPresenter>().GetCharacter());
            btree.Start();
        }
    }
    
    
}