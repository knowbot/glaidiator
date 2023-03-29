using System.Collections.Generic;
using BehaviorTree;
using Glaidiator.Model;
using Glaidiator.Presenter;
using UnityEngine;

namespace Glaidiator
{
    public class CollisionManager : MonoBehaviour
    {

        public static CollisionManager instance;

        // arena dimensions
        public static double width;
        public static double depth;

        public GameObject playerO;
        public GameObject bossO;
        public Character player;
        public Character boss;
    
        //private Dictionary<Character, Vector2> positions;
        //private Dictionary<Vector2, Character> characters;

        private List<ColBox> cols;


        private void Awake()
        {
            instance = this;
            //positions = new Dictionary<Character, Vector2>();
            //characters = new Dictionary<Vector2, Character>();
            cols = new List<ColBox>();

            //player.onMove += OnMove;
            //boss.onMove += OnMove;
            //player.onMove += OnAttack;
            //boss.onMove += OnAttack;
        }

        private void OnMove()
        {
        
        }

        private void OnAttack()
        {
        
        }
    
        void Start()
        {
            player = playerO.GetComponent<CharacterPresenter>().GetCharacter();
            boss = bossO.GetComponent<CharacterPresenter>().GetCharacter();
            //boss = GetTestBoss(bossO);
            //var temp = bossO.gameObject.GetComponent<AIContainer>().transform;
            //boss.Movement = new Movement(temp);
            
            
            if (player != null)
            {
                cols.Add(new ColBox(player, ColBox.ColType.Body));
                Debug.Log("added PLAYER colbox");
            }

            if (boss != null)
            {
                cols.Add(new ColBox(boss, ColBox.ColType.Body));
                Debug.Log("added BOSS colbox");
            }
        }

        void Update()
        {
            // TODO: include the deltaTime tick scaling
            
            // check lifetime of colboxes
            foreach(ColBox col in cols)
            {
                if (col.Update(Time.deltaTime)) cols.Remove(col);
            }

            // check intersections
            foreach (ColBox col in cols)
            {
                foreach (ColBox other in cols)
                {
                    if (col.Intersects(other))
                    {
                        Debug.Log(col.GetColType() + " Collided with " + other.GetColType());
                        
                        if (col.GetColType() == ColBox.ColType.Attack && other.GetColType() == ColBox.ColType.Body)
                        {
                            // call 'other' get hit
                        }

                        //col.GetOwner().GetHit()
                    }
                }
            }
        }

        private Character GetTestBoss(GameObject boss)
        {
            Character character = new Character(boss.transform);
            return character;
        }

    }

    
    public class ColBox
    {
        private Character _owner;
        private ColType _colType;
        private float _duration;

        private Vector2 _origin;

        public Vector2 corner1;
        public Vector2 corner2;

        private float offset = 0.75f; // radius of character footprint 2D topdown size
        private Vector2 offset2d = new Vector2(0.75f, 0.75f);

        public enum ColType
        {
            Body = 0,
            Attack = 1,
            //Defense = 2
        }
        
        public ColBox(Character owner, ColType colType)
        {
            _owner = owner;
            _colType = colType;
            _origin = new Vector2(_owner.Movement.Position.x, _owner.Movement.Position.z);
            corner1 = _origin - offset2d;
            corner2 = _origin + offset2d;
        }

        public ColBox(Character owner, ColType colType, float duration)
        {
            _owner = owner;
            _colType = colType;
            _duration = duration;
        }

        public bool Update(float dTime)
        {
            Debug.DrawLine(_owner.Movement.Position, _owner.Movement.Position + new Vector3(offset, 0f, offset), Color.magenta);
            Debug.DrawLine(_owner.Movement.Position, _owner.Movement.Position + new Vector3(-offset, 0f, -offset), Color.magenta);
            Debug.DrawLine(_owner.Movement.Position, _owner.Movement.Position + new Vector3(-offset, 0f, offset), Color.magenta);
            Debug.DrawLine(_owner.Movement.Position, _owner.Movement.Position + new Vector3(offset, 0f, -offset), Color.magenta);
            
            if (_duration >= 0f) _duration -= dTime;
            
            if (_colType != ColType.Body && _duration < 0f)
            {
                // remove box
                Debug.Log("removing ColBox of type " + _colType);
                return true;
            }
            
            _origin = new Vector2(_owner.Movement.Position.x, _owner.Movement.Position.z);
            corner1 = _origin - offset2d;
            corner2 = _origin + offset2d;
            return false;
        }

        public bool Intersects(ColBox other)
        {
            if (_owner.Equals(other._owner)) return false;

            if (other.corner1.x < (corner2.x) && //
                (other.corner2.x) > corner1.x &&
                other.corner1.y < (corner2.y) &&
                (other.corner2.y) > corner1.y)
            {
                return true;
            }
            
            return false;
        }

        public ColType GetColType()
        {
            return _colType;
        }

        public Character GetOwner()
        {
            return _owner;
        }
    }
}
