using System;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Glaidiator.Model.Utils
{
    public static class MathUtils
    {
        public static float GetSignedAngle(Quaternion a, Quaternion b, Vector3 axis) {
            (b*Quaternion.Inverse(a)).ToAngleAxis(out float angle, out Vector3 angleAxis);
            if(Vector3.Angle(axis, angleAxis) > 90f) {
                angle = -angle;
            }
            return Mathf.DeltaAngle(0f, angle);
        }
        
        public static Vector3 Get8DDirection(float x, float y) {
            float absX = Math.Abs(x);
            float absY = Math.Abs(y);
            if (absX < 0.1 && absY < 0.1) {
                // close to center
                return Vector3.zero;
            }
            if (absX > absY) {
                // vertical side
                float half = absX * 0.4142f;
                if (x < 0) {
                    // left side
                    if (y > half) return new Vector3(-1, 0, -1).normalized;
                    if (y < -half) return new Vector3(-1, 0, 1).normalized;
                    return new Vector3(-1,0, 0).normalized;
                } else {
                    // right side
                    if (y > half) return new Vector3(1, 0, -1).normalized;
                    if (y < -half) return new Vector3(1, 0, 1).normalized;
                    return new Vector3(1, 0,0).normalized;
                }
            } else {
                // horizontal side
                float half = absY * 0.4142f;
                if (y < 0) {
                    // bottom
                    if (x > half) return new Vector3(1, 0, -1).normalized;
                    if (x < -half) return new Vector3(-1, 0, -1).normalized;
                    return new Vector3(0, 0,-1).normalized;
                } else {
                    // top
                    if (x > half) return new Vector3(1, 0, 1).normalized;
                    if (x < -half) return new Vector3(-1, 0, 1).normalized;
                    return new Vector3(0, 0, 1).normalized;
                }
            }
        }

        public static float GetSignedDistance(Vector3 origin, Vector3 target)
        {
            //Vector3 N = (target - origin).normalized;
            float dist = (target.x - origin.x) + (target.z - origin.z);
            return dist;
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