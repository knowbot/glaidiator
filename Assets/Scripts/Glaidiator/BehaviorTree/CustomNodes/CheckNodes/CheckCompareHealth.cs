using System;
using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.CheckNodes
{
    public class CheckCompareHealth : Check
    {
        private float _ratio;
        public CheckCompareHealth(float ratio)
        {
            _ratio = ratio;
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            
            var enemy = (Character)GetData("enemy");
            if (enemy == null)
            {
                state = NodeState.FAILURE;
                return state;
            }

            float percentRatio = 100f * (owner.Health.Current - enemy.Health.Current) / ((owner.Health.Current + enemy.Health.Current) / 2); 
            state = percentRatio > _ratio ? NodeState.SUCCESS : NodeState.FAILURE;
            return state;
        }

        public override Node Clone()
        {
            return new CheckCompareHealth(_ratio);
        }

        public override void Mutate()
        {
            _ratio += MathStuff.Rand.NextFloat(10f) - 5f;
            _ratio = Mathf.Clamp(_ratio, 0f, Arena.Diagonal);
        }

        public override Node Randomized()
        {
            return new CheckCompareHealth(MathStuff.Rand.NextFloat(-200f, 200f));
        }
        
        public override void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(GetType().Name);
            w.WriteAttributeString("ratio", _ratio.ToString());
            w.WriteEndElement();
        }
    }
}