using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ch.sycoforge.Decal.Demo
{
    public static class BezierUtil
    {
        /// <summary>
        /// Approximates a given path with bezier curves.
        /// </summary>
        /// <param name="path">The path to approximate.</param>
        /// <param name="segments">The resolution.</param>
        /// <param name="radius">The max radius.</param>
        /// <param name="angleThreshold">The angular threshold specifying when to skip a segment.</param>
        /// <returns></returns>
        public static List<Vector3> InterpolatePath(List<Vector3> path, int segments, float radius, float angleThreshold)
        {
            if (path.Count >= 3)
            {
                List<Vector3> interpolatedPath = new List<Vector3>();
                int lastIndex = path.Count - 1;

                // Add first point
                interpolatedPath.Add(path[0]);

                int index = 0;
                for (int j = 2; j < path.Count; j += 1)
                {
                    Vector3 start = path[j - 2];
                    Vector3 center = path[j - 1];
                    Vector3 end = path[j];

                    // Direction vectors away from center
                    Vector3 sc = (center - start);
                    Vector3 ce = (end - center);

                    // Angle between segments
                    float angle = Mathf.Abs(Vector3.Angle(sc, ce));

                    // Skip segment if smaller the treshold
                    if (angle <= angleThreshold)
                    {
                        continue;
                    }


                    float scLen = sc.magnitude;
                    float ceLen = ce.magnitude;

                    sc.Normalize();
                    ce.Normalize();

                    scLen = Mathf.Min(scLen * 0.5f, radius);
                    ceLen = Mathf.Min(ceLen * 0.5f, radius);


                    // New control points
                    Vector3 p0 = center - sc * scLen;
                    Vector3 p1 = center;
                    Vector3 p2 = center + ce * ceLen;



                    float t;
                    float ti;
                    Vector3 position;

                    for (int i = 0; i < segments; i++)
                    {
                        t = i / (segments - 1.0f);
                        ti = 1.0f - t;

                        // Cubic interpolation in 3D space
                        position = (ti * ti * p0) + (2.0f * ti * t * p1) + (t * t * p2);

                        interpolatedPath.Add(position);
                    }

                    index = j;
                }

                if (index <= lastIndex)
                {
                    // Add last point
                    interpolatedPath.Add(path[lastIndex]);
                }

                return interpolatedPath;
            }
            else
            {
                return path;
            }
        }


        public static Vector3[] GetBezierApproximation(Vector3[] controlPoints, int outputSegmentCount)
        {
            Vector3[] points = new Vector3[outputSegmentCount + 1];
            for (int i = 0; i < outputSegmentCount; i++)
            {
                float t = (float)i / outputSegmentCount;
                points[i] = GetBezierPoint(t, controlPoints, 0, controlPoints.Length);
            }
            return points;
        }

        public static Vector3 GetBezierPoint(float t, Vector3[] controlPoints, int index, int count)
        {
            if (count == 1)
            {
                return controlPoints[index];
            }
            var p0 = GetBezierPoint(t, controlPoints, index - 1, count - 1);
            var p1 = GetBezierPoint(t, controlPoints, index, count - 1);
            var p2 = GetBezierPoint(t, controlPoints, index + 1, count - 1);


            Vector3 position = (1.0f - t) * (1.0f - t) * p0
               + 2.0f * (1.0f - t) * t * p1
               + t * t * p2;
            return position;
        }
    }
}
