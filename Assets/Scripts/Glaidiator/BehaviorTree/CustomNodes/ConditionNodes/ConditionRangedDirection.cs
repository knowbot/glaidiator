using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.ConditionNodes
{
    public class ConditionRangedDirection: Condition
    {
        private float _maxAngle;

        public ConditionRangedDirection(float maxAngle = 5f)
        {
            _maxAngle = maxAngle;
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            
            Movement target = ((Character)GetData("enemy"))?.Movement;
            if (target == null)
            {
                //Debug.Log("CheckRangedDirection target = null");
                state = NodeState.FAILURE;
                return state;
            }

            // calculate angle difference and check for some maximum offset before success
            Vector3 targetDirection = (target.Position - tree.Owner.Movement.Position).normalized;
            Vector3 currDirection = tree.Direction;
            //float angle = Vector3.Angle(currDirection, targetDirection);
            float angle = Vector3.SignedAngle(currDirection, targetDirection, Vector3.up);
            
            if (angle <= _maxAngle && angle >= (_maxAngle*-1f))
            {
                ////Debug.Log("aim angle = " + angle);
                //Debug.DrawLine(_tree.OwnerCharacter.Movement.Position, target.Position, Color.cyan, .1f);
                state = NodeState.SUCCESS;
            }
            else
            {
                //Debug.DrawLine(_tree.OwnerCharacter.Movement.Position, target.Position, Color.yellow, .1f);
                state = NodeState.FAILURE;
            }
            
            return state;
        }

        public override Node Clone()
        {
            return new ConditionRangedDirection(_maxAngle);
        }

        public override void Mutate()
        {
            _maxAngle += MathStuff.Rand.NextFloat(5f) - 2.5f;
            _maxAngle = Mathf.Clamp(_maxAngle, 0f, 22.5f);
        }

        public override Node Randomized()
        {
            return new ConditionRangedDirection(MathStuff.Rand.NextFloat(22.5f));
        }
        
        public override void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(GetType().Name);
            w.WriteAttributeString("maxAngle", _maxAngle.ToString());
            w.WriteEndElement();
        }
    }
}