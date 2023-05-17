using System;
using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using UnityEngine;

namespace Glaidiator.BehaviorTree.LeafNodes.ConditionNodes
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
            Vector3 targetDirection = (target.Position - owner.Movement.Position).normalized;
            Vector3 currDirection = tree.Direction;
            //float angle = Vector3.Angle(currDirection, targetDirection);
            float angle = Vector3.SignedAngle(currDirection, targetDirection, Vector3.up);
            
            if (angle <= _maxAngle && angle >= (_maxAngle*-1f))
            {
                ////Debug.Log("aim angle = " + angle);
                //Debug.DrawLine(_ownerCharacter.Movement.Position, target.Position, Color.cyan, .1f);
                state = NodeState.SUCCESS;
            }
            else
            {
                //Debug.DrawLine(_ownerCharacter.Movement.Position, target.Position, Color.yellow, .1f);
                state = NodeState.FAILURE;
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
            w.WriteAttributeString("maxAngle", _maxAngle.ToString());
            w.WriteEndElement();
        }
    }
}