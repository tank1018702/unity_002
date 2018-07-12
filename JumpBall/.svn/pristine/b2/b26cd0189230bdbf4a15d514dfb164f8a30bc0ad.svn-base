using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ch.sycoforge.Decal.Demo
{
    public static class LineUtil
    {
        public static void DrawPath(float thickness, Material material, List<Vector3> path)
        {
            if (path == null || (path != null && path.Count < 2)) { return; }

            if (thickness <= Mathf.Epsilon)
            {
                GL.Begin(GL.LINES);
            }
            else
            {
                GL.Begin(GL.QUADS);
            }

            material.SetPass(0);
            GL.Color(Color.blue);

            Vector3 lastCorner = path[0];

            for (int i = 1; i < path.Count; i++)
            {
                Vector3 corner = path[i];
                DrawLine(thickness, lastCorner, corner);

                lastCorner = corner;
            }


            GL.End();
        }

        private static void DrawLine(float thickness, Vector3 start, Vector3 end)
        {
            if (thickness <= Mathf.Epsilon)
            {
                GL.Vertex(start);
                GL.Vertex(end);
            }
            else
            {
                Camera c = Camera.main;

                Vector3 startToEnd = (end - start).normalized;
                Vector3 camToStart = (start - c.transform.position).normalized;

                Vector3 perpendicular = Vector3.Cross(camToStart, startToEnd) * (thickness / 2);

                GL.Vertex(start - perpendicular);
                GL.Vertex(start + perpendicular);
                GL.Vertex(end + perpendicular);
                GL.Vertex(end - perpendicular);
            }
        }
    }
}
