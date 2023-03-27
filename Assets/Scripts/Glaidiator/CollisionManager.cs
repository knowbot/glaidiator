using System.Collections.Generic;
using Glaidiator.Model;
using UnityEngine;

namespace Glaidiator
{
    public class CollisionManager : MonoBehaviour
    {

        public static CollisionManager instance;

        // arena dimensions
        public static double width;
        public static double depth;

        public static Character player;
        public static Character boss;
    
        private Dictionary<Character, Vector2> positions;
        private Dictionary<Vector2, Character> characters;

        private List<ColBox> cols;


        private void Awake()
        {
            instance = this;
            positions = new Dictionary<Character, Vector2>();
            characters = new Dictionary<Vector2, Character>();
            cols = new List<ColBox>();

            player.onMove += OnMove;
            boss.onMove += OnMove;
            player.onMove += OnAttack;
            boss.onMove += OnAttack;
        }

        private void OnMove()
        {
        
        }

        private void OnAttack()
        {
        
        }
    
        void Start()
        {
            if (player != null) cols.Add(new ColBox(player, ColBox.ColType.Body));
            if (boss != null) cols.Add(new ColBox(boss, ColBox.ColType.Body));
        }

        void Update()
        {
            // TODO: include the deltaTime tick scaling
            
            // check collisions
            foreach(ColBox col in cols)
            {
                col.Update(Time.deltaTime);
            }

            foreach (ColBox col in cols)
            {
                foreach (ColBox other in cols)
                {
                    if (col.Intersects(other))
                    {
                        if (col.GetColType() == ColBox.ColType.Attack && other.GetColType() == ColBox.ColType.Body)
                        {
                            // call 'other' get hit
                        }
                    }
                }
            }
        }
        
        //private bool Intersects
    
        public static bool IsIntersectOther(Model.Character agent1)
        {
            Model.Character agent2;
            if (agent1.Equals(player))
            {
                agent2 = boss;
            }
            else
            {
                agent2 = player;
            }

            Vector2 origin = new Vector2(player.Movement.Position.x,
                player.Movement.Position.z);
        
            
            return false;
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

        public void Update(float dTime)
        {
            _duration -= dTime;
            if (_colType != ColType.Body && _duration < 0f)
            {
                // remove box
            }
            
            _origin = new Vector2(_owner.Movement.Position.x, _owner.Movement.Position.z);
            corner1 = _origin - offset2d;
            corner2 = _origin + offset2d;
        }

        public bool Intersects(ColBox other)
        {
            if (_owner == other._owner) return false;
            
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
    }
}
