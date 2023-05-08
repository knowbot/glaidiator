﻿using UnityEngine;

namespace Glaidiator.Model
{
    public static class Arena
    {
        public static Vector2 Size = new Vector2(30f, 30f);
        public static Vector3 PlayerStartPos = new Vector3(1, 0, 1);
        public static Quaternion PlayerStartRot = Quaternion.Euler(0, 45, 0);
        public static Vector3 EnemyStartPos = new Vector3(29, 0, 29);
        public static Quaternion EnemyStartRot = Quaternion.Euler(0, 225, 0);
        public static float MaxSize = Mathf.Max(Size.x, Size.y);
    }
}