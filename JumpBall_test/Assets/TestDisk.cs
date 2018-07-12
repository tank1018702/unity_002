using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TestDisk : MonoBehaviour
{
    MeshFilter MeshFilter;
    MeshRenderer MeshRenderer;

    public float height = 0.4f;
    public float radius = 1f;
    public int details = 20;

    static float EPS = 0.01f;

    void Start()
    {
        MeshFilter = transform.GetComponent<MeshFilter>();
        MeshRenderer = transform.GetComponent<MeshRenderer>();

    }
    //生成一个弧度随机的圆片
    [ContextMenu("GeneratePie")]
    public void GeneratePieByRadian()
    {
        var arcs = new List<float>();
        // 按比例随机，0代表空的，1代表整圆
        float r = Random.Range(0.9f, 1.0f);
        r *= 2 * Mathf.PI;

        arcs.Add(0);
        arcs.Add(r);

        GeneratePie(arcs);

        StartCoroutine(SequenceTest());
       
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

        for (int i = 0; i < arcs.Count; i += 2)
        {
            _verts.Clear();
            _uvs.Clear();
            _tris.Clear();

            //先把中心点添加进顶点List中
            _verts.Add(new Vector3(0, 0, 0));
            _verts.Add(new Vector3(0, -height, 0));

            AddArcMeshInfo(arcs[i], arcs[i + 1], _verts, _uvs, _tris);

            //把顶点序号填进三角形list里
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
        mesh.uv = uvs.ToArray();

        //根据顶点和三角形数据自动生成体积框,法线和切线
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        MeshFilter.mesh = mesh;

    }

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

    IEnumerator SequenceTest()
    {
        //for (int i = 0; i < MeshFilter.mesh.triangles.Length; i += 3)
        //{          
        //    Debug.DrawLine(MeshFilter.mesh.vertices[MeshFilter.mesh.triangles[i]], MeshFilter.mesh.vertices[MeshFilter.mesh.triangles[i + 1]], Color.red, 100f);

        //    yield return new WaitForSeconds(0.2f);
        //    Debug.DrawLine(MeshFilter.mesh.vertices[MeshFilter.mesh.triangles[i + 1]], MeshFilter.mesh.vertices[MeshFilter.mesh.triangles[i + 2]], Color.yellow, 100f);

        //    yield return new WaitForSeconds(0.2f);
        //    Debug.DrawLine(MeshFilter.mesh.vertices[MeshFilter.mesh.triangles[i + 2]], MeshFilter.mesh.vertices[MeshFilter.mesh.triangles[i]], Color.blue, 100f);

        //    yield return new WaitForSeconds(0.2f);

        //}
        for (int i = 0; i <MeshFilter. mesh.vertices.Length; i++)
        {
            Debug.DrawRay(MeshFilter.mesh. vertices[i],MeshFilter. mesh.normals[i], Color.black, 1000f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}