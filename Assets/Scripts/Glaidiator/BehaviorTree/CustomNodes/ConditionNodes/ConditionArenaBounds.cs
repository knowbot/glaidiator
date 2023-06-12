using System;
using System.Globalization;
using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.BehaviorTree.CustomNodes;
using Glaidiator.BehaviorTree.CustomNodes.ConditionNodes;
using Glaidiator.Model;
using Glaidiator.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable All

namespace Glaidiator.BehaviorTree.LeafNodes.ConditionNodes
{
    public class ConditionArenaBounds : Condition
    {
        private float _distance;

        public ConditionArenaBounds(float distance)
        {
            _distance = distance;
        }
        
        public override NodeState Evaluate()
        {
            tree.currentNode = this;

            Vector3 target = tree.Owner.Movement.Position + (tree.Direction * _distance);
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
            return new ConditionArenaBounds(_distance);
        }

        public override void Mutate()
        {
            _distance += MathStuff.Rand.NextFloat(4f) - 2f;
            _distance = Mathf.Clamp(_distance, 0f, Arena.Diagonal);
        }

        public override Node Randomized()
        {
            var newNode = new ConditionArenaBounds(MathStuff.Rand.NextFloat(Arena.Diagonal));
            return MathStuff.Rand.NextFloat() > 0.5 ? new Inverter(newNode) : newNode;
        }
        public override void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(GetType().Name);
            w.WriteAttributeString("distance", _distance.ToString());
            w.WriteEndElement();
        }
    }
}