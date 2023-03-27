using System;
using System.Collections;
using System.Collections.Generic;
using BasicAI;
using UnityEngine;

namespace BehaviorTree
{
    public class AIContainer : MonoBehaviour
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
            
        }
    }
    
    
}