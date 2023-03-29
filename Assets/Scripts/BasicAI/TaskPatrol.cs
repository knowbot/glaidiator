using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Glaidiator.Model;

namespace BasicAI
{
    public class TaskPatrol : Node
    {
        //private Transform _transform; // curr pos of this agent
        private Movement _transform;
        private Vector2 _currPos;
        private List<Vector2> _waypoints;
        
        private int _currentWP;
        private float _waitTime = 1f;
        private float _waitCounter = 0f;
        private bool _isWaiting = false;


        public TaskPatrol(BTree btree, Character myCharacter)
        {
            tree = btree;
            _transform = myCharacter.Movement;
            _currPos = _transform.Position.xz();
            _waypoints = new List<Vector2>();
            _waypoints.Add(_currPos + new Vector2(0f, 0f));
            _waypoints.Add(_currPos + new Vector2(2f, 0f));
            _waypoints.Add(_currPos + new Vector2(2f, 2f));
            _waypoints.Add(_currPos + new Vector2(0f, 2f));
        }

        public override NodeState Evaluate()
        {
            if (_isWaiting)
            {
                tree.Direction = _transform.LastDir.xz();
                tree.Move = false;
                
                _waitCounter += Time.deltaTime;
                if (_waitCounter >= _waitTime)
                {
                    _isWaiting = false;
                }
            }
            else
            {
                //var currPos = _transform.position;
                //Transform wp = _waypoints[_currentWP];
                //var pos3d = tree.GetTransform().position;
                //var pos3d = _transform.Position;
                //_currPos = new Vector2(pos3d.x, pos3d.z);
                _currPos = _transform.Position.xz();
                Vector2 wp = _waypoints[_currentWP];

                if (Vector2.Distance(_currPos, wp) < 0.01f)
                {
                    tree.Direction = _transform.LastDir.xz();
                    tree.Move = false;
                    
                    _currPos = wp;
                    _waitCounter = 0f;
                    _isWaiting = true;

                    _currentWP = (_currentWP + 1) % _waypoints.Count;
                    Debug.Log("current WP is " + _currentWP);
                    
                }
                else
                {
                    tree.Direction = (wp - _currPos).normalized;
                    tree.Move = true;
                    
                    /*
                    _transform.position = Vector3.MoveTowards(_transform.position,
                        wp,
                        GuardBT.speed * Time.deltaTime);
                    _transform.LookAt(wp);
                    */
                    //Vector3 temp3d = new Vector3(_currPos.x, 0f, _currPos.y);
                    Vector3 temp3d = _currPos.x0y();
                    //Vector3 wp3d = new Vector3(wp.x, 0f, wp.y);
                    Vector3 wp3d = wp.x0y();
                    Debug.DrawLine(temp3d, wp3d, Color.blue);
                }

            }

            
            state = NodeState.RUNNING;
            return state;
        }
    }

}



