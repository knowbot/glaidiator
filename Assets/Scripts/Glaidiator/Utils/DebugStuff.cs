using UnityEngine;

namespace Glaidiator.Utils
{
    public static class DebugStuff
    {
        public static void DrawRect(Vector3 position, Quaternion orientation, Vector2 extent, Color color)
        {
            Vector3 rightOffset = Vector3.right * extent.x * 0.5f;
            Vector3 upOffset = Vector3.forward * extent.y * 0.5f;
 
            Vector3 offsetA = orientation * (rightOffset + upOffset);
            Vector3 offsetB = orientation * (-rightOffset + upOffset);
            Vector3 offsetC = orientation * (-rightOffset - upOffset);
            Vector3 offsetD = orientation * (rightOffset - upOffset);
 
            Debug.DrawLine(position + offsetA, position + offsetB, Color.blue);
            Debug.DrawLine(position + offsetB, position + offsetC, Color.red);
            Debug.DrawLine(position + offsetC, position + offsetD, Color.green);
            Debug.DrawLine(position + offsetD, position + offsetA, Color.yellow);
        }
        
        public static void DrawCircle(Vector3 position, float radius, int segments, Color color)
        {
            // If either radius or number of segments are less or equal to 0, skip drawing
            if (radius <= 0.0f || segments <= 0)
            {
                return;
            }
 
            // Single segment of the circle covers (360 / number of segments) degrees
            float angleStep = (360.0f / segments);
 
            // Result is multiplied by Mathf.Deg2Rad constant which transforms degrees to radians
            // which are required by Unity's Mathf class trigonometry methods
 
            angleStep *= Mathf.Deg2Rad;
 
            // lineStart and lineEnd variables are declared outside of the following for loop
            Vector3 lineStart = Vector3.zero;
            Vector3 lineEnd = Vector3.zero;
 
            for (int i = 0; i < segments; i++)
            {
                // Line start is defined as starting angle of the current segment (i)
                lineStart.x = Mathf.Cos(angleStep * i) ;
                lineStart.z = Mathf.Sin(angleStep * i);
 
                // Line end is defined by the angle of the next segment (i+1)
                lineEnd.x = Mathf.Cos(angleStep * (i + 1));
                lineEnd.z = Mathf.Sin(angleStep * (i + 1));
 
                // Results are multiplied so they match the desired radius
                lineStart *= radius;
                lineEnd *= radius;
 
                // Results are offset by the desired position/origin 
                lineStart += position;
                lineEnd += position;
 
                // Points are connected using DrawLine method and using the passed color
                Debug.DrawLine(lineStart, lineEnd, color);
            }
        }
    }

}