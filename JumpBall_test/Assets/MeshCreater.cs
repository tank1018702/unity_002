using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCreater : MonoBehaviour
{
    MeshFilter meshFilter;
    MeshCollider meshCollider;
    MeshRenderer meshRenderer;

    public float height = 0.4f;
    public float radius = 1f;
    public int details = 20;

    static float EPS = 0.01f;


    void Start ()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
	}
    void Update()
    {

    }
    [ContextMenu("AddMeshInfo")]
    void AddArcMeshInfo(float begin, float end, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        // begin和end是开始弧度、结束弧度
        // verts里面已经有了顶面中心点、底面中心点，下标分别是0和1

        float eachRad = 2 * Mathf.PI / details;

        // 顶点
        float a;
        for (a = begin; a <= end; a += eachRad)
        {
            Vector3 v = new Vector3(radius * Mathf.Sin(a), 0, radius * Mathf.Cos(a));
            verts.Add(v);
            Vector3 v2 = new Vector3(radius * Mathf.Sin(a), -height, radius * Mathf.Cos(a));
            verts.Add(v2);
        }
        if (a < end + EPS)
        {
            Vector3 v = new Vector3(radius * Mathf.Sin(end), 0, radius * Mathf.Cos(end));
            verts.Add(v);
            Vector3 v2 = new Vector3(radius * Mathf.Sin(end), -height, radius * Mathf.Cos(end));
            verts.Add(v2);
        }

        // 顶面顶点序号
        int n = verts.Count;
        for (int i = 2; i < n - 2; i += 2)
        {
            tris.Add(i); tris.Add(i + 2); tris.Add(0);
        }

        // 侧面顶点序号
        for (int i = 2; i < n - 2; i += 2)
        {
            tris.Add(i); tris.Add(i + 1); tris.Add(i + 2);
            tris.Add(i + 2); tris.Add(i + 1); tris.Add(i + 3);
        }

        // 封住两个直线边
        tris.Add(2); tris.Add(0); tris.Add(1);
        tris.Add(3); tris.Add(2); tris.Add(1);
        tris.Add(n - 1); tris.Add(0); tris.Add(n - 2);
        tris.Add(1); tris.Add(0); tris.Add(n - 1);
    }

   
}
