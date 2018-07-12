using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
public class Pie : MonoBehaviour {

    public float height = 0.4f;
    public float radius = 1f;
    public int details = 20;

    MeshFilter meshFilter;
    MeshCollider meshCollider;
    MeshRenderer meshRenderer;

    static float EPS = 0.01f;
    // 添加圆饼的一个切块
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
        if (a < end+EPS)
        {
            Vector3 v = new Vector3(radius * Mathf.Sin(end), 0, radius * Mathf.Cos(end));
            verts.Add(v);
            Vector3 v2 = new Vector3(radius * Mathf.Sin(end), -height, radius * Mathf.Cos(end));
            verts.Add(v2);
        }

        // 顶面顶点序号
        int n = verts.Count;
        for (int i = 2; i < n-2; i+=2)
        {
            tris.Add(i); tris.Add(i + 2); tris.Add(0);
        }

        // 侧面顶点序号
        for (int i = 2; i < n-2; i+=2)
        {
            tris.Add(i); tris.Add(i + 1); tris.Add(i+2);
            tris.Add(i+2); tris.Add(i+1); tris.Add(i+3);
        }

        // 封住两个直线边
        tris.Add(2); tris.Add(0); tris.Add(1);
        tris.Add(3); tris.Add(2); tris.Add(1);
        tris.Add(n -1); tris.Add(0); tris.Add(n-2);
        tris.Add(1); tris.Add(0); tris.Add(n -1);
    }

    public void Init()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // 参数：每个弧用两两个弧度（float）表示，每个饼可以有多个三角块，就和切披萨一样
    public void GeneratePie(List<float> arcs)
    {
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();

        List<Vector3> _verts = new List<Vector3>();
        List<Vector2> _uvs = new List<Vector2>();
        List<int> _tris = new List<int>();

        for (int i=0; i<arcs.Count; i+=2)
        {
            _verts.Clear();
            _uvs.Clear();
            _tris.Clear();

            _verts.Add(new Vector3(0, 0, 0));
            _verts.Add(new Vector3(0, -height, 0));

            AddArcMeshInfo(arcs[i], arcs[i+1], _verts, _uvs, _tris);

            foreach (int n in _tris)
            {
                tris.Add(n + verts.Count);
            }
            verts.AddRange(_verts);
            uvs.AddRange(_uvs);
        }
        
        Mesh mesh = new Mesh();
        // 填写mesh
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        //mesh.uv = uvs.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    public void GeneratePieByLevel(int level)
    {
        var arcs = new List<float>();
        // 按比例随机，0代表空的，1代表整圆
        float r = Random.Range(0.1f, 0.9f);
        r *= 2 * Mathf.PI;

        arcs.Add(0);
        arcs.Add(r);

        GeneratePie(arcs);

        transform.Rotate(0, Random.Range(0,359), 0);
    }


     

    // for test
    public List<float> arcs;

    void Test()
    {
        Debug.Log("Test " + arcs.Count);
        GeneratePie(arcs);
        Invoke("Test", 2);
    }
}
