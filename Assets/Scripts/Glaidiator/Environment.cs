using System.Collections.Generic;
using UnityEngine;

namespace Glaidiator
{
    public class Environment : MonoBehaviour
    {

        public static Environment instance;

        // arena dimensions
        public static double width;
        public static double depth;

        public static Model.Character player;
        public static Model.Character boss;
    
        private Dictionary<Model.Character, Vector2> positions;
        private Dictionary<Vector2, Model.Character> characters;
    
    

        private void Awake()
        {
            instance = this;
            positions = new Dictionary<Model.Character, Vector2>();
            characters = new Dictionary<Vector2, Model.Character>();

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
        
        }

    
        void Update()
        {
        
        }

    
        //public static Vector2 ConvertToModel(Vector3 )

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
}
