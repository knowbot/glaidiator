using UnityEngine;

namespace Glaidiator.Model
{
    public static class Arena
    {
        public static Vector2 Size = new(30f, 30f);
        public static Vector3 P1StartPos = new(5, 0, 5);
        public static Quaternion P1StartRot = Quaternion.Euler(0, 0, 0);
        public static Vector3 P2StartPos = new(25, 0, 25);
        public static Quaternion P2StartRot = Quaternion.Euler(0, 180, 0);
        public static float MaxSize = Mathf.Max(Size.x, Size.y);
        public static float Diagonal = Mathf.Sqrt(Mathf.Pow(Size.x, 2) + Mathf.Pow(Size.y, 2));
    }
}