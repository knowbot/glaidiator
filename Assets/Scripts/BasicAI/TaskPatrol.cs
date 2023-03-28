using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace BasicAI
{
    public class TaskPatrol : Node
    {
        private Transform _transform; // curr pos of this agent
        //private Transform[] _waypoints;
        //private Animator _animator;

        private Vector2 _currPos;
        private List<Vector2> _waypoints;
        
        private int _currentWP;
        private float _waitTime = 1f;
        private float _waitCounter = 0f;
        private bool _isWaiting = false;

        //private float _speed = 2f;

        public TaskPatrol(BTree btree, Transform transform, List<Vector2> waypoints)
        {
            tree = btree;
            _transform = transform;
            _currPos = new Vector2(_transform.position.x, _transform.position.z);
            _waypoints = waypoints;
            //_animator = transform.GetComponent<Animator>();

            //_waypoints.Add(_currPos + new Vector3(0f, 0f, 0f));
            //_waypoints.Add(_currPos + new Vector3(2f, 0f, 0f));
            //_waypoints.Add(_currPos + new Vector3(2f, 0f, 2f));
            //_waypoints.Add(_currPos + new Vector3(0f, 0f, 2f));
            _waypoints.Add(_currPos + new Vector2(0f, 0f));
            _waypoints.Add(_currPos + new Vector2(2f, 0f));
            _waypoints.Add(_currPos + new Vector2(2f, 2f));
            _waypoints.Add(_currPos + new Vector2(0f, 2f));

        }

        public override NodeState Evaluate()
        {
            if (_isWaiting)
            {
                tree.Move = false;
                _waitCounter += Time.deltaTime;
                if (_waitCounter >= _waitTime)
                {
                    _isWaiting = false;
                    //_animator.SetBool("Walking", true);
                }
            }
            else
            {
                //var currPos = _transform.position;
                //Transform wp = _waypoints[_currentWP];
                var pos3d = tree.GetTransform().position;
                _currPos = new Vector2(pos3d.x, pos3d.z);
                Vector2 wp = _waypoints[_currentWP];

                if (Vector2.Distance(_currPos, wp) < 0.01f)
                {

                    _currPos = wp;
                    _waitCounter = 0f;
                    _isWaiting = true;

                    _currentWP = (_currentWP + 1) % _waypoints.Count;
                    Debug.Log("current WP is " + _currentWP);

                    //_animator.SetBool("Walking", false);
                }
                else
                {
                    tree.Move = true;
                    tree.Direction = (wp - _currPos).normalized;
                    /*
                    _transform.position = Vector3.MoveTowards(_transform.position,
                        wp,
                        GuardBT.speed * Time.deltaTime);
                    _transform.LookAt(wp);
                    */
                    Vector3 temp3d = new Vector3(_currPos.x, 0f, _currPos.y);
                    Vector3 wp3d = new Vector3(wp.x, 0f, wp.y);
                    Debug.DrawLine(temp3d, wp3d, Color.blue);
                }

            }

            
            state = NodeState.RUNNING;
            return state;
        }
    }

}



