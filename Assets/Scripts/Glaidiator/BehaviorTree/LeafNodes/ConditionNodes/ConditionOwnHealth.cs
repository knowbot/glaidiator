﻿using System;
using System.Xml;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.LeafNodes.ConditionNodes
{
    public class ConditionOwnHealth : Condition
    {
        private float _threshold;
        public ConditionOwnHealth(float threshold)
        {
            _threshold = threshold;
        }
        public override NodeState Evaluate()
        {
            tree.currentNode = this;

            if (owner.Health.Current >= _threshold)
            {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }

        public override Node Clone()
        {
            throw new NotImplementedException();
        }

        public override void Mutate()
        {
            throw new NotImplementedException();
        }

        public override Node Randomized()
        {
            throw new NotImplementedException();
        }
        
        public override void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(GetType().Name);
            w.WriteAttributeString("threshold", _threshold.ToString());
            w.WriteEndElement();
        }
    }
}