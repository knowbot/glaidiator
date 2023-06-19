using System;
using UnityEngine;

namespace Glaidiator.Utils
{
    public static class Direction
    {
        private static readonly Vector3[] Dirs = {
            new Vector3( 0, 0,  1),
            new Vector3( 1, 0,  1).normalized,
            new Vector3( 1, 0,  0),
            new Vector3( 1, 0, -1).normalized,
            new Vector3( 0, 0, -1),
            new Vector3(-1, 0, -1).normalized,
            new Vector3(-1, 0,  0),
            new Vector3(-1, 0,  1).normalized,
        };

        public enum Name
        {
            Up,        
            UpRight,   
            Right,     
            DownRight,
            Down,      
            DownLeft,  
            Left,     
            UpLeft    
        }

        public static Vector3 Get(Name name)
        {
            return Dirs[(int)name];
        }
        
        public static Vector3 Get(int index)
        {
            return Dirs[index];
        }

        public static int GetIndex(Vector3 dir)
        {
            return Array.IndexOf(Dirs, dir);
        }
    }
}