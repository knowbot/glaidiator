using System;
using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.CheckNodes
{
    public class CheckCompareStamina : Check
    {
        
        private float _ratio;
        public CheckCompareStamina(float ratio)
        {
            _ratio = ratio;
        }
        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            
            Character enemy = (Character)GetData("enemy");
            if (enemy == null)
            {
                state = NodeState.FAILURE;
                return state;
            }

            float percentRatio = 100f * (owner.Stamina.Current - enemy.Stamina.Current) / ((owner.Stamina.Current + enemy.Stamina.Current) / 2); 
            state = percentRatio > _ratio ? NodeState.SUCCESS : NodeState.FAILURE;
            return state;
        }

        public override Node Clone()
        {
            return new CheckCompareStamina(_ratio);
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