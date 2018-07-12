using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Circle : MonoBehaviour
{
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    MeshCollider meshCollider;
    Rigidbody rig;
    Mesh mesh;

    public float OuterRadius = 1.0f;
    public float InnerRadius = 0.5f;
    public float Height = 0.2f;
    public int details = 20;

    List<Vector3> vertices;
    List<Vector2> uv;
    List<Vector3> normals;
    List<int> triangles;

    List<Vector3> temp_vertices;

    Vector3 MiddleLine;
    float CircleAngle;
    Transform pointer;

  



    public void Init()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
       



    }

    public void RigReset()
    {
        rig.isKinematic = true;
        transform.rotation = Quaternion.identity;
    }



    [ContextMenu("Generate")]
    public void GenerateCircleByLevel()
    {
        float r = Random.Range(0.1f, 0.6f);
        r *= 2 * Mathf.PI;
        GenerateCircle(r);

        float RandomAngle = Random.Range(0, 2 * Mathf.PI * Mathf.Rad2Deg);
        transform.Rotate(0, RandomAngle, 0);


    }
    [ContextMenu("PrintUV")]
    void PrintUV()
    {
        StartCoroutine(test());
    }

    public void GenerateCircle(float radian)
    {
        float EachAngle;
        EachAngle = 2 * Mathf.PI / details;

        vertices = new List<Vector3>();
        triangles = new List<int>();
        uv = new List<Vector2>();
        normals = new List<Vector3>();

        temp_vertices = new List<Vector3>();

        //计算外侧数据
        int trunc = Mathf.FloorToInt(radian / EachAngle);

        CircleAngle = trunc * EachAngle * Mathf.Rad2Deg;
        for (int i = 0; i <= trunc; i++)
        {
            Vector3 v1 = new Vector3(OuterRadius * Mathf.Sin(i * EachAngle), 0, OuterRadius * Mathf.Cos(i * EachAngle));
            Vector3 v2 = new Vector3(OuterRadius * Mathf.Sin(i * EachAngle), -Height, OuterRadius * Mathf.Cos(i * EachAngle));
            //外侧顶点
            temp_vertices.Add(v1); temp_vertices.Add(v2);
            //外侧顶点UV坐标
            uv.Add(new Vector2(((i * 1.0f) / trunc * 1.0f), 1)); uv.Add(new Vector2(((i * 1.0f) / trunc * 1.0f), 0));
            //外侧顶点法线
            normals.Add((v1 - Vector3.zero).normalized); normals.Add((v2 - new Vector3(0, -Height, 0)).normalized);
        }
        vertices.AddRange(temp_vertices);
        //外侧三角形
        for (int i = 0; i < temp_vertices.Count - 2; i += 2)
        {
            triangles.Add(i); triangles.Add(i + 1); triangles.Add(i + 3);
            triangles.Add(i); triangles.Add(i + 3); triangles.Add(i + 2);
        }
        temp_vertices.Clear();

        //计算内侧数据
        for (int i = 0; i <= trunc; i++)
        {
            Vector3 v1 = new Vector3(InnerRadius * Mathf.Sin(i * EachAngle), 0, InnerRadius * Mathf.Cos(i * EachAngle));
            Vector3 v2 = new Vector3(InnerRadius * Mathf.Sin(i * EachAngle), -Height, InnerRadius * Mathf.Cos(i * EachAngle));
            //内侧顶点
            temp_vertices.Add(v1); temp_vertices.Add(v2);
            //内侧顶点UV坐标
            uv.Add(new Vector2(((i * 1.0f) / trunc * 1.0f), 1)); uv.Add(new Vector2(((i * 1.0f) / trunc * 1.0f), 0));
            //内侧顶点法线
            normals.Add((Vector3.zero - v1).normalized); normals.Add((new Vector3(0, -Height, 0) - v2).normalized);
        }
        vertices.AddRange(temp_vertices);
        //内侧三角形
        for (int i = temp_vertices.Count; i < temp_vertices.Count * 2 - 2; i += 2)
        {
            triangles.Add(i); triangles.Add(i + 3); triangles.Add(i + 1);
            triangles.Add(i); triangles.Add(i + 2); triangles.Add(i + 3);
        }
        temp_vertices.Clear();

        //计算顶面数据
        for (int i = 0; i <= trunc; i++)
        {
            Vector3 v1 = new Vector3(InnerRadius * Mathf.Sin(i * EachAngle), 0, InnerRadius * Mathf.Cos(i * EachAngle));
            Vector3 v2 = new Vector3(OuterRadius * Mathf.Sin(i * EachAngle), 0, OuterRadius * Mathf.Cos(i * EachAngle));
            //顶面顶点
            temp_vertices.Add(v1); temp_vertices.Add(v2);
            //顶面顶点UV坐标
            uv.Add((new Vector2(0.5f * Mathf.Sin(i * EachAngle), 0.5f * Mathf.Cos(i * EachAngle)) + new Vector2(0.5f, 0.5f)));
            uv.Add((new Vector2(0.5f * (InnerRadius / OuterRadius) * Mathf.Sin(i * EachAngle), 0.5f * (InnerRadius / OuterRadius) * Mathf.Cos(i * EachAngle)) + new Vector2(0.5f, 0.5f)));
            //顶面顶点法线
            normals.Add(Vector3.up); normals.Add(Vector3.up);

        }
        vertices.AddRange(temp_vertices);
        for (int i = temp_vertices.Count * 2; i < temp_vertices.Count * 3 - 2; i += 2)
        {
            triangles.Add(i); triangles.Add(i + 1); triangles.Add(i + 3);
            triangles.Add(i); triangles.Add(i + 3); triangles.Add(i + 2);
        }
        temp_vertices.Clear();

        //底面数据
        for (int i = 0; i <= trunc; i++)
        {
            Vector3 v1 = new Vector3(InnerRadius * Mathf.Sin(i * EachAngle), -Height, InnerRadius * Mathf.Cos(i * EachAngle));
            Vector3 v2 = new Vector3(OuterRadius * Mathf.Sin(i * EachAngle), -Height, OuterRadius * Mathf.Cos(i * EachAngle));
            //底面顶点
            temp_vertices.Add(v1); temp_vertices.Add(v2);
            //底面顶点UV坐标
            uv.Add((new Vector2(0.5f * Mathf.Sin(i * EachAngle), 0.5f * Mathf.Cos(i * EachAngle)) + new Vector2(0.5f, 0.5f)));
            uv.Add((new Vector2(0.5f * (InnerRadius / OuterRadius) * Mathf.Sin(i * EachAngle), 0.5f * (InnerRadius / OuterRadius) * Mathf.Cos(i * EachAngle)) + new Vector2(0.5f, 0.5f)));
            //底面法线
            normals.Add(-Vector3.up); normals.Add(-Vector3.up);
        }
        vertices.AddRange(temp_vertices);
        for (int i = temp_vertices.Count * 3; i < temp_vertices.Count * 4 - 2; i += 2)
        {
            triangles.Add(i); triangles.Add(i + 3); triangles.Add(i + 1);
            triangles.Add(i); triangles.Add(i + 2); triangles.Add(i + 3);
        }
        temp_vertices.Clear();

        //封边

        //起点
        Vector3 vs1 = new Vector3(OuterRadius * Mathf.Sin(0 * EachAngle), 0, OuterRadius * Mathf.Cos(0 * EachAngle));
        Vector3 vs2 = new Vector3(OuterRadius * Mathf.Sin(0 * EachAngle), -Height, OuterRadius * Mathf.Cos(0 * EachAngle));
        Vector3 vs3 = new Vector3(InnerRadius * Mathf.Sin(0 * EachAngle), 0, InnerRadius * Mathf.Cos(0 * EachAngle));
        Vector3 vs4 = new Vector3(InnerRadius * Mathf.Sin(0 * EachAngle), -Height, InnerRadius * Mathf.Cos(0 * EachAngle));
        //顶点
        temp_vertices.Add(vs1); temp_vertices.Add(vs2); temp_vertices.Add(vs3); temp_vertices.Add(vs4);
        // UV
        uv.Add(new Vector2(0, 1)); uv.Add(new Vector2(0, 0)); uv.Add(new Vector2(1, 1)); uv.Add(new Vector2(1, 0));
        //法线
        normals.Add(Vector3.Cross((vs3 - vs1), (vs4 - vs1)).normalized);
        normals.Add(Vector3.Cross((vs3 - vs1), (vs4 - vs1)).normalized);
        normals.Add(Vector3.Cross((vs3 - vs1), (vs4 - vs1)).normalized);
        normals.Add(Vector3.Cross((vs3 - vs1), (vs4 - vs1)).normalized);
        //三角形

        triangles.Add(vertices.Count); triangles.Add(vertices.Count + 3); triangles.Add(vertices.Count + 1);
        triangles.Add(vertices.Count); triangles.Add(vertices.Count + 2); triangles.Add(vertices.Count + 3);
        vertices.AddRange(temp_vertices);
        temp_vertices.Clear();

        //终点
        Vector3 ve1 = new Vector3(OuterRadius * Mathf.Sin(trunc * EachAngle), 0, OuterRadius * Mathf.Cos(trunc * EachAngle));
        Vector3 ve2 = new Vector3(OuterRadius * Mathf.Sin(trunc * EachAngle), -Height, OuterRadius * Mathf.Cos(trunc * EachAngle));
        Vector3 ve3 = new Vector3(InnerRadius * Mathf.Sin(trunc * EachAngle), 0, InnerRadius * Mathf.Cos(trunc * EachAngle));
        Vector3 ve4 = new Vector3(InnerRadius * Mathf.Sin(trunc * EachAngle), -Height, InnerRadius * Mathf.Cos(trunc * EachAngle));
        //顶点
        temp_vertices.Add(ve1); temp_vertices.Add(ve2); temp_vertices.Add(ve3); temp_vertices.Add(ve4);
        // UV
        uv.Add(new Vector2(0, 1)); uv.Add(new Vector2(0, 0)); uv.Add(new Vector2(1, 1)); uv.Add(new Vector2(1, 0));
        //法线
        normals.Add(Vector3.Cross((ve4 - ve1), (ve3 - ve1)).normalized);
        normals.Add(Vector3.Cross((ve4 - ve1), (ve3 - ve1)).normalized);
        normals.Add(Vector3.Cross((ve4 - ve1), (ve3 - ve1)).normalized);
        normals.Add(Vector3.Cross((ve4 - ve1), (ve3 - ve1)).normalized);
        //三角形
        triangles.Add(vertices.Count); triangles.Add(vertices.Count + 1); triangles.Add(vertices.Count + 3);
        triangles.Add(vertices.Count); triangles.Add(vertices.Count + 3); triangles.Add(vertices.Count + 2);
        vertices.AddRange(temp_vertices);


        mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uv.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateTangents();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

    }
    [ContextMenu("focetest")]
    public void test2()
    {
        StartCoroutine(PlayDropAnim());
    }
    IEnumerator PlayDropAnim()
    {
        rig.isKinematic = false;
        rig.AddForce(MiddleLine * 10, ForceMode.Impulse);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);

    }

    IEnumerator test()
    {
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Debug.DrawLine(mesh.uv[mesh.triangles[i]] * 2, mesh.uv[mesh.triangles[i + 1]] * 2, Color.red, 100f);

            yield return new WaitForSeconds(Time.deltaTime);
            Debug.DrawLine(mesh.uv[mesh.triangles[i + 1]] * 2, mesh.uv[mesh.triangles[i + 2]] * 2, Color.yellow, 100f);

            yield return new WaitForSeconds(Time.deltaTime);
            Debug.DrawLine(mesh.uv[mesh.triangles[i + 2]] * 2, mesh.uv[mesh.triangles[i]] * 2, Color.blue, 100f);

            yield return new WaitForSeconds(Time.deltaTime);
        }
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            Debug.DrawRay(mesh.vertices[i], mesh.normals[i], Color.black, 1000f);
            //Debug.DrawRay(mesh.vertices[i], mesh.tangents[i], Color.white, 1000f);
            //Debug.DrawLine(mesh.vertices[40], mesh.vertices[i], Color.red, 1000f);
            Debug.Log(i + "：" + mesh.vertices[i]);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }



}
