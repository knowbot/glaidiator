using UnityEngine;

namespace Glaidiator.Model
{
    public static class Arena
    {
        public static Vector2 Size = new Vector2(30f, 30f);
        public static Vector3 PlayerStartPos = new Vector3(5, 0, 5);
        public static Quaternion PlayerStartRot = Quaternion.Euler(0, 45, 0);
        public static Vector3 BossStartPos = new Vector3(25, 0, 25);
        public static Quaternion BossStartRot = Quaternion.Euler(0, 225, 0);
        public static float MaxSize = Mathf.Max(Size.x, Size.y);
        public static float Diagonal = Mathf.Sqrt(Mathf.Pow(Size.x, 2) + Mathf.Pow(Size.y, 2));
    }
}