using System.Collections.Generic;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.BehaviorTree.CustomBTs;
using Glaidiator.BehaviorTree.LeafNodes;
using Glaidiator.BehaviorTree.LeafNodes.ConditionNodes;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree
{
    public static class RandomTreeBuilder
    {
        private static readonly int _maxDepth = 2; // maximum depth of the tree (decorators excluded)
        private static readonly int _minChildren = 2; // minimum children (leaf nodes) for each node
        private static readonly int _maxChildren = 5; // maximum children (leaf nodes) for each node
        public static List<Composite> composites;
        public static List<Leaf> leafs;

        static RandomTreeBuilder()
        {
            composites = new List<Composite>();
            composites.Add(new Selector());
            composites.Add(new Sequence());
        }

        public static EvoBT GenerateTree()
        {
            var tree = new EvoBT();
            var root = composites[Random.Range(0, composites.Count)].Clone() as Composite;
            for (int i = 0; i < Random.Range(0, _maxChildren); i++)
            {
                RecursiveAddNodes(root, 0);
            }
            return tree;
        }

        public static Composite RecursiveAddNodes(Composite root, int currDepth)
        {
            currDepth++;
            if (currDepth == _maxDepth) return root;
            if (currDepth == _maxDepth - 1)
            {
                for (int i = 0; i < Random.Range(0, _maxChildren); i++)
                {
                    Leaf child = leafs[Random.Range(0, leafs.Count)];
                    if (child is Condition && MathStuff.Rand.NextFloat() > 0.5f) // 50% to invert conditions
                        root.Attach(new Inverter(child));
                    else
                        root.Attach(child);
                }

                return root;
            }
            for (int i = 0; i < Random.Range(0, _maxChildren); i++)
            {
                Leaf child = leafs[Random.Range(0, leafs.Count)];
                if (child is Condition && MathStuff.Rand.NextFloat() > 0.5f) // 50% to invert conditions
                    root.Attach(new Inverter(child));
                else
                    root.Attach(child);
            }
            
            return root;
        }
    }
}