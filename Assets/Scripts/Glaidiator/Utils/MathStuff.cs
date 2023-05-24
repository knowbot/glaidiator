using System;
using System.Threading;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Glaidiator.Utils
{
    public static class MathStuff
    {
        private static int _tracker = 0;
        public static Random Rand => new((uint)(Guid.NewGuid().GetHashCode() + Interlocked.Increment(ref _tracker)));

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

        public static float GetSignedDistance(Vector3 origin, Vector3 target, Vector3 dir)
        {
            //Vector3 N = (target - origin).normalized;
            //float dist = (target.x - origin.x) + (target.z - origin.z);
            //float dotAB = Vector3.Dot(origin, target);
            //dist = math.sqrt(dist);

            Vector2 o2d = new Vector2(origin.x, origin.z);
            Vector2 t2d = new Vector2(target.x, target.z);
            Vector2 dir2d = new Vector2(dir.x, dir.z);
            
            // dir
            //Vector2 dir = (new Vector2(target.x - origin.x, target.z - origin.z)).normalized;
            float absDist = Vector2.Distance(o2d, t2d);

            //float dist = (dir.x * absDist) + (dir.z * absDist);

            float dist = absDist; // wip

            //float theta = math.atan2(t2d.y - o2d.y, t2d.x - o2d.x);
            //float dist = ((t2d.x - o2d.x) * math.cos(theta)) + ((t2d.y - o2d.y) * math.sin(theta));
            
            return dist;
        }

        public static float GetPercentDifference(float a, float b)
        {
            return 100f * (a - b) / ((a + b) / 2);
        }
    }
}