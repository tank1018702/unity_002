using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class TestMesh2 : MonoBehaviour
{
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    MeshCollider meshCollider;

    Mesh mesh;

    List<Vector3> vertices;

    List<Vector2> uv;

    List<Vector3> normals;
    List<int> triangles;
    public int details = 360;

    public float OuterRadius = 1.0f;
    public float InnerRadius = 0.5f;

    public float Height = 0.2f;

    void Start ()
    {
        meshFilter = transform.GetComponent<MeshFilter>();
        meshRenderer = transform.GetComponent<MeshRenderer>();
        meshCollider = transform.GetComponent<MeshCollider>();
        SingleOne();
        StartCoroutine(test());
    }
	
	
	void Update ()
    {
		
	}

    void SingleOne()
    {
        float EachAngle;
        EachAngle = 2 * Mathf.PI / details;

        //顶点计算
        List<Vector3> vertices_f = new List<Vector3>();
        List<Vector3> vertices_s = new List<Vector3>();

        List<Vector2> uv_f = new List<Vector2>();
        List<Vector2> uv_s = new List<Vector2>();

        for (float i = 0; i < 2 * Mathf.PI; i += EachAngle)
        {
            Vector3 up_inside = new Vector3(InnerRadius * Mathf.Sin(i), 0, InnerRadius * Mathf.Cos(i));
            Vector3 down_inside = new Vector3(InnerRadius * Mathf.Sin(i), -Height, InnerRadius * Mathf.Cos(i));
            Vector3 down_outside = new Vector3(OuterRadius * Mathf.Sin(i), -Height, OuterRadius * Mathf.Cos(i));
            Vector3 up_outside = new Vector3(OuterRadius * Mathf.Sin(i), 0, OuterRadius * Mathf.Cos(i));

            vertices_f.Add(up_inside); vertices_s.Add(up_inside);
            vertices_f.Add(down_inside); vertices_s.Add(down_inside);
            vertices_f.Add(down_outside); vertices_s.Add(down_outside);
            vertices_f.Add(up_outside); vertices_s.Add(up_outside);
            //uv坐标
            uv_f.Add(new Vector2(0, 1));uv_s.Add(new Vector2(1, 1));
            uv_f.Add(new Vector2(0, 0));uv_s.Add(new Vector2(1, 0));
            uv_f.Add(new Vector2(1, 0));uv_s.Add(new Vector2(0, 0));
            uv_f.Add(new Vector2(1, 1));uv_s.Add(new Vector2(0, 1));

        }

        //三角形计算

        List<int> triangles_f = new List<int>();
        List<int> triangles_s = new List<int>();
        //法线
        List<Vector3> normals_f = new List<Vector3>();
        List<Vector3> normals_s = new List<Vector3>();

       
        int n = vertices_f.Count;
        //每一个梯形立方体不考虑侧边有8个三角形
        for(int i=0;i<n;i+=4)
        {
            if(i<n-4)
            {  //0
                triangles_f.Add(i); triangles_f.Add(i+3); triangles_f.Add(i+4);
                triangles_s.Add(i); triangles_s.Add(i+4); triangles_s.Add(i+5);

                normals_f.Add(Vector3.Cross((vertices_f[i + 3] - vertices_f[i]), (vertices_f[i + 4] - vertices_f[i])).normalized);
                normals_s.Add(Vector3.Cross((vertices_s[i + 4] - vertices_s[i]), (vertices_s[i + 5] - vertices_s[i])).normalized);

                //1
                triangles_f.Add(i + 1); triangles_f.Add(i); triangles_f.Add(i + 5);
                triangles_s.Add(i + 1); triangles_s.Add(i + 5); triangles_s.Add(i + 6);

                normals_f.Add(Vector3.Cross((vertices_f[i ] - vertices_f[i+1]), (vertices_f[i + 5] - vertices_f[i+1])).normalized);
                normals_s.Add(Vector3.Cross((vertices_s[i + 5] - vertices_s[i+1]), (vertices_s[i + 6] - vertices_s[i+1])).normalized);

                //2
                triangles_f.Add(i + 2); triangles_f.Add(i+1); triangles_f.Add(i + 6);
                triangles_s.Add(i + 2); triangles_s.Add(i + 6); triangles_s.Add(i + 7);

                normals_f.Add(Vector3.Cross((vertices_f[i+1] - vertices_f[i + 2]), (vertices_f[i + 6] - vertices_f[i + 2])).normalized);
                normals_s.Add(Vector3.Cross((vertices_s[i + 6] - vertices_s[i + 2]), (vertices_s[i + 7] - vertices_s[i + 2])).normalized);


                //3
                triangles_f.Add(i + 3); triangles_f.Add(i+2); triangles_f.Add(i + 7);
                triangles_s.Add(i + 3); triangles_s.Add(i + 7); triangles_s.Add(i + 4);

                normals_f.Add(Vector3.Cross((vertices_f[i + 2] - vertices_f[i + 3]), (vertices_f[i + 7] - vertices_f[i + 3])).normalized);
                normals_s.Add(Vector3.Cross((vertices_s[i + 7] - vertices_s[i + 3]), (vertices_s[i + 4] - vertices_s[i + 3])).normalized);
            }
            else
            {
                //0
                triangles_f.Add(i); triangles_f.Add(i + 3); triangles_f.Add(0);
                triangles_s.Add(i); triangles_s.Add(0); triangles_s.Add(1);

                normals_f.Add(Vector3.Cross((vertices_f[i + 3] - vertices_f[i ]), (vertices_f[0] - vertices_f[i ])).normalized);
                normals_s.Add(Vector3.Cross((vertices_s[0] - vertices_s[i ]), (vertices_s[1] - vertices_s[i])).normalized);

                //1
                triangles_f.Add(i+1); triangles_f.Add(i); triangles_f.Add(1);
                triangles_s.Add(i+1); triangles_s.Add(1); triangles_s.Add(2);

                normals_f.Add(Vector3.Cross((vertices_f[i] - vertices_f[i+1]), (vertices_f[1] - vertices_f[i+1])).normalized);
                normals_s.Add(Vector3.Cross((vertices_s[1] - vertices_s[i+1]), (vertices_s[2] - vertices_s[i+1])).normalized);
                //2
                triangles_f.Add(i + 2); triangles_f.Add(i+1); triangles_f.Add(2);
                triangles_s.Add(i + 2); triangles_s.Add(2); triangles_s.Add(3);

                normals_f.Add(Vector3.Cross((vertices_f[i+1] - vertices_f[i + 2]), (vertices_f[2] - vertices_f[i + 2])).normalized);
                normals_s.Add(Vector3.Cross((vertices_s[2] - vertices_s[i + 2]), (vertices_s[3] - vertices_s[i + 2])).normalized);

                //3
                triangles_f.Add(i + 3); triangles_f.Add(i + 2); triangles_f.Add(3);
                triangles_s.Add(i + 3); triangles_s.Add(3); triangles_s.Add(0);

                normals_f.Add(Vector3.Cross((vertices_f[i + 2] - vertices_f[i + 3]), (vertices_f[3] - vertices_f[i + 3])).normalized);
                normals_s.Add(Vector3.Cross((vertices_s[3] - vertices_s[i + 3]), (vertices_s[0] - vertices_s[i + 3])).normalized);


            }
        }
        vertices = new List<Vector3>();
        triangles = new List<int>();
        normals = new List<Vector3>();
        uv = new List<Vector2>();

        vertices.AddRange(vertices_f);
        vertices.AddRange(vertices_s);
        triangles.AddRange(triangles_f);
        triangles.AddRange(triangles_s);
        normals.AddRange(normals_f);
        normals.AddRange(normals_s);
        uv.AddRange(uv_f);
        uv.AddRange(uv_s);

        mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uv.ToArray();

        mesh.RecalculateBounds();
        //mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;


    }
    IEnumerator test()
    {
        int j = 0;

        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {

            //Debug.Log(j);
            Debug.DrawLine(mesh.vertices[mesh.triangles[i]], mesh.vertices[mesh.triangles[i + 1]], Color.red, 100f);

            yield return new WaitForSeconds(Time.deltaTime);
            Debug.DrawLine(mesh.vertices[mesh.triangles[i + 1]], mesh.vertices[mesh.triangles[i + 2]], Color.yellow, 100f);

            yield return new WaitForSeconds(Time.deltaTime);
            Debug.DrawLine(mesh.vertices[mesh.triangles[i + 2]], mesh.vertices[mesh.triangles[i]], Color.blue, 100f);

            yield return new WaitForSeconds(Time.deltaTime);

            //Debug.DrawRay(mesh.vertices[j], mesh.normals[j], Color.black, 1000f);
            
            j++;
        }
    }


    // private void OnDrawGizmos()
    //{
    //    for (int i = 0; i < mesh.vertices.Length; i++)
    //    {
    //        Gizmos.DrawRay(mesh.vertices[i], mesh.normals[i]);
    //    }
    //}
}
