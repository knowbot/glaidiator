using System;
using System.Collections.Generic;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.BehaviorTree.CustomNodes.CheckNodes;
using Glaidiator.BehaviorTree.CustomNodes.TaskNodes;
using Glaidiator.BehaviorTree.LeafNodes.ConditionNodes;

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
                        new CheckEnemyDistance(atkDist),
                        new TaskFaceEnemy(),
                        new TaskLightAtk(),
                    }),
                    new Sequence(new List<Node>
                    {
                        new CheckEnemyDistance(aggroDist),
                        new TaskFaceEnemy(),
                        new TaskWait(),
                        new TaskMoveForward(),                                    
                    })
                }),
                // default patrol behaviour
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node> {
                        new Inverter(new CheckHasTarget("wp")),
                        new TaskSetWP(4f)
                    }),
                    new Sequence(new List<Node>
                    {
                        new CheckHasTarget("wp"),
                        new CheckTargetDistance("wp", 0.01f),
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