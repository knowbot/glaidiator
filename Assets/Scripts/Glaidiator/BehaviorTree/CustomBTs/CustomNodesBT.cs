using System;
using System.Collections.Generic;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.BehaviorTree.LeafNodes.ConditionNodes;
using Glaidiator.BehaviorTree.LeafNodes.TaskNodes;

namespace Glaidiator.BehaviorTree.CustomBTs
{
    public class CustomNodesBT : BTree
    {
        private static float aggroDist = 6f;
        private static float atkDist = 2f;
        
        public CustomNodesBT() : base()
        {
            
        }
            
        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {
                // approach and attack behaviour
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new ConditionEnemyDistance(atkDist),
                        new TaskFaceEnemy(),
                        new TaskLightAtk(),
                    }),
                    new Sequence(new List<Node>
                    {
                        new ConditionEnemyDistance(aggroDist),
                        new TaskFaceEnemy(),
                        new TaskWait(),
                        new TaskMoveForward(),                                    
                    })
                }),
                // default patrol behaviour
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node> {
                        new Inverter(new ConditionHasTarget("wp")),
                        new TaskSetWP(4f)
                    }),
                    new Sequence(new List<Node>
                    {
                        new ConditionHasTarget("wp"),
                        new ConditionTargetDistance("wp", 0.01f),
                        new TaskClearWP(),
                        new TaskTurnRight(),
                        new TaskSetWP(4f)
                    }),
                    new Sequence(new List<Node>
                    {
                        new TaskMoveForward()
                    })
                })
                
                
            });
            
            root.SetOwner(owner);
            root.SetTree(this);
            SetData("enemy", enemy);
            return root;
        }

        public override BTree Clone()
        {
            throw new NotImplementedException();
        }
    }
}