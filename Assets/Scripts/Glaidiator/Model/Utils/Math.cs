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