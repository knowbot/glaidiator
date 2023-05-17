using System.Collections.Generic;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.BehaviorTree.CustomBTs;

namespace Glaidiator.BehaviorTree
{
    public static class RandomTreeBuilder
    {
        private static int _minDepth = 2; // minimum depth of the tree (decorators excluded)
        private static int _minChildren = 2; // minimum children (leaf nodes) for each node
        private static int _maxDepth = 4; // maximum depth of the tree (decorators excluded)
        private static int _maxChildren = 5; // maximum children (leaf nodes) for each node
        public static List<Node> composites;
        public static List<Node> leafs;

        static RandomTreeBuilder()
        {
            composites = new List<Node>();
            composites.Add(new Selector());
            composites.Add(new Sequence());
        }

        public static EvoBT GenerateTree()
        {
            var tree = new EvoBT();
            
            return tree;
        }
    }
}