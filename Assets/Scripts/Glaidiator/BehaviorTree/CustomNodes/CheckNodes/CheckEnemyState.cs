using System;
using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;

namespace Glaidiator.BehaviorTree.CustomNodes.CheckNodes
{
    public class CheckEnemyState : Check
    {
        private string _enemyState;
        
        public CheckEnemyState(string enemyState)
        {
            _enemyState = enemyState;
        }
        
        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            if (tree == null) throw new NullReferenceException();

            string currEnemyState = ((Character)GetData("enemy")).CurrentState.ToString();
            if (currEnemyState == null)
            {
                state = NodeState.FAILURE;
                return state;
            }

            state = _enemyState == currEnemyState ? NodeState.SUCCESS : NodeState.FAILURE;
            
            return state;
        }

        public override Node Clone()
        {
            return new CheckEnemyState(_enemyState);
        }

        public override void Mutate()
        {
            return;
        }

        public override Node Randomized()
        {
            return Clone();
        }

        public override void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(GetType().Name);
            w.WriteAttributeString("enemyState", _enemyState);
            w.WriteEndElement();
        }
    }
}