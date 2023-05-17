using System;
using System.Globalization;
using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using UnityEngine;
// ReSharper disable All

namespace Glaidiator.BehaviorTree.LeafNodes.ConditionNodes
{
    public class ConditionArenaBounds: Condition
    {
        private float _distance;

        public ConditionArenaBounds(float distance)
        {
            _distance = distance;
        }
        
        public override NodeState Evaluate()
        {
            tree.currentNode = this;

            Vector3 target = owner.Movement.Position + (tree.Direction * _distance);
            if (target.x < 0f || target.x > Arena.MaxSize ||
                target.z < 0f || target.z > Arena.MaxSize)
            {
                state = NodeState.FAILURE;
                //Debug.Log("CheckArenaBounds outside target = " + target);
            }
            else
            {
                state = NodeState.SUCCESS;
            }
                
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
            w.WriteAttributeString("distance", _distance.ToString());
            w.WriteEndElement();
        }
        
    }
}