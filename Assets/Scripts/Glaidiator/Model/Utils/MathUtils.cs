using UnityEngine;

namespace Glaidiator.Model.Utils
{
    public static class Math
    {
        public static float GetSignedAngle(Quaternion a, Quaternion b, Vector3 axis) {
            (b*Quaternion.Inverse(a)).ToAngleAxis(out float angle, out Vector3 angleAxis);
            if(Vector3.Angle(axis, angleAxis) > 90f) {
                angle = -angle;
            }
            return Mathf.DeltaAngle(0f, angle);
        }
        
        public static Vector2 GetDirection(double x, double y) {
            double absX = Math.Abs(x);
            double absY = Math.Abs(y);
            if (absX < 0.1 && absY < 0.1) {
                // close to center
                return Direction.None;
            }
            if (absX > absY) {
                // vertical side
                double half = absX * 0.4142;
                if (x > 0) {
                    // left side
                    if (y > half) return Direction.LeftDown;
                    if (y < -half) return Diretion.LeftUp;
                    return Direction.Left;
                } else {
                    // right side
                    if (y > half) return Direction.RightDown;
                    if (y < -half) return Direction.RightUp;
                    return Direction.Right;
                }
            } else {
                // horisontal side
                double half = absY * 0.4142;
                if (y > 0) {
                    // bottom
                    if (x > half) return Direction.RightDown;
                    if (x < -half) return Direction.LeftDown;
                    return Direction.Down;
                } else {
                    // top
                    if (x > half) return Direction.RightUp;
                    if (x < -half) return Direction.LeftUp;
                    return Direction.Up;
                }
            }
        }
    }
    
    public static class Vector2Extension {
     
        public static Vector2 Rotate(this Vector2 v, float degrees) {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
         
            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }
    }
    
}