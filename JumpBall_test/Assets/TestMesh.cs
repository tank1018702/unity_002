using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class TestMesh : MonoBehaviour
{
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    MeshCollider meshCollider;

    Mesh mesh;

    List<Vector3> vertices;
    List<Vector3> normals;
    List<int> triangles;
    public int details = 360;

    public float OuterRadius = 1.0f;
    public float InnerRadius = 0.5f;

    public float Height = 0.2f;
    

    private void Awake()
    {

    }

    void Start()
    {
        meshFilter = transform.GetComponent<MeshFilter>();
        meshRenderer = transform.GetComponent<MeshRenderer>();
        meshCollider = transform.GetComponent<MeshCollider>();
        SingleOne();


    }


    void SingleOne()
    {
        float EachAngle;
        EachAngle = 2 * Mathf.PI / details;
        //每一块圆环的单元都是一个梯形立方体,需要八个顶点.
        //每两块之间合并时,有四个顶点是共用的.
        //所以假设组成整个圆环一共用了N个梯形,如果不是一个整圆则总共的顶点数就是(4*n+4)个,若首尾相连则是4*n个

        vertices = new List<Vector3>();

        for (float i = 0; i < 2 * Mathf.PI; i += EachAngle)
        {

            Vector3 v_up_inside = new Vector3(InnerRadius * Mathf.Sin(i), 0, InnerRadius * Mathf.Cos(i));
            Vector3 v_up_outside = new Vector3(OuterRadius * Mathf.Sin(i), 0, OuterRadius * Mathf.Cos(i));
            Vector3 v_down_inside = new Vector3(InnerRadius * Mathf.Sin(i), -Height, InnerRadius * Mathf.Cos(i));
            Vector3 v_down_outside = new Vector3(OuterRadius * Mathf.Sin(i), -Height, OuterRadius * Mathf.Cos(i));
            vertices.Add(v_up_inside);
            vertices.Add(v_up_outside);
            vertices.Add(v_down_inside);
            vertices.Add(v_down_outside);

            //Vector3 Test = new Vector3(InnerRadius * Mathf.Sin(i + EachAngle), 0, InnerRadius * Mathf.Cos(i + EachAngle));
            //Debug.Log(Vector3.Angle(v_up_inside,Test));

        }

        //每个单位梯形立方体有6个面,若首尾相连成一个圆环,则侧面不需要渲染,只剩里外上下四个面
        //四个面则是八个三角形
        //用三角形拼接成梯形立方体,每八个顶点为一组,因为相邻的两个梯形共用四个顶点,则每多画一个梯形立方体,下标递增4个
        //三个顶点顺时针连接的三角形式向着与Z轴相反的方向渲染.
        triangles = new List<int>();
        normals = new List<Vector3>();
        int n = vertices.Count;
        int v = 0;
        for (int i = 0; i < n - 4; i += 4)
        {
            //里
            triangles.Add(i); triangles.Add(i + 6); triangles.Add(i + 2);
            triangles.Add(i); triangles.Add(i + 4); triangles.Add(i + 6);
            //添加法线
            //normals.Add(Vector3.Cross((vertices[i + 6] - vertices[i]), (vertices[i + 2] - vertices[i])).normalized);
            //normals.Add(Vector3.Cross((vertices[i + 4] - vertices[i]), (vertices[i + 6] - vertices[i])).normalized);

            //外(法向量与里面正好相反,因此三角形的顶点顺序也相反)
            triangles.Add(i + 1); triangles.Add(i + 3); triangles.Add(i + 5);
            triangles.Add(i + 3); triangles.Add(i + 7); triangles.Add(i + 5);
            //normals.Add(Vector3.Cross((vertices[i + 3] - vertices[i+1]), (vertices[i + 5] - vertices[i+1])).normalized);
            //normals.Add(Vector3.Cross((vertices[i + 7] - vertices[i+3]), (vertices[i + 5] - vertices[i+3])).normalized);


            //上
            triangles.Add(i + 1); triangles.Add(i + 5); triangles.Add(i);
            triangles.Add(i + 5); triangles.Add(i + 4); triangles.Add(i);
            //normals.Add(Vector3.Cross((vertices[i + 5] - vertices[i+1]), (vertices[i ] - vertices[i+1])).normalized);
            //normals.Add(Vector3.Cross((vertices[i + 4] - vertices[i+5]), (vertices[i ] - vertices[i+5])).normalized);

            //下
            triangles.Add(i + 3); triangles.Add(i + 2); triangles.Add(i + 6);
            triangles.Add(i + 6); triangles.Add(i + 7); triangles.Add(i + 3);
            //normals.Add(Vector3.Cross((vertices[i + 2] - vertices[i+3]), (vertices[i + 6] - vertices[i+3])).normalized);
            //normals.Add(Vector3.Cross((vertices[i + 7] - vertices[i+6]), (vertices[i + 3] - vertices[i+6])).normalized);

            v = i;

        }


        //首尾相连封口
        //最口闭合时注意所有面的渲染顺序相反
        //小于于i+4即起点,否则为终点
        triangles.Add(0); triangles.Add(2); triangles.Add(v + 6);
        triangles.Add(0); triangles.Add(v + 6); triangles.Add(v + 4);
        //normals.Add(Vector3.Cross((vertices[2] - vertices[0]), (vertices[v + 6] - vertices[0])).normalized);
        //normals.Add(Vector3.Cross((vertices[v + 6] - vertices[0]), (vertices[v+4] - vertices[0])).normalized);

        triangles.Add(1); triangles.Add(v + 5); triangles.Add(3);
        triangles.Add(3); triangles.Add(v + 5); triangles.Add(v + 7);
        //normals.Add(Vector3.Cross((vertices[v + 5] - vertices[1]), (vertices[v + 3] - vertices[1])).normalized);
        //normals.Add(Vector3.Cross((vertices[v+5] - vertices[3]), (vertices[v + 7] - vertices[3])).normalized);

        triangles.Add(1); triangles.Add(0); triangles.Add(v + 5);
        triangles.Add(v + 5); triangles.Add(0); triangles.Add(v + 4);
        //normals.Add(Vector3.Cross((vertices[0] - vertices[1]), (vertices[v + 5] - vertices[1])).normalized);
        //normals.Add(Vector3.Cross((vertices[0] - vertices[v+5]), (vertices[v + 4] - vertices[v+5])).normalized);

        triangles.Add(3); triangles.Add(v + 6); triangles.Add(2);
        triangles.Add(v + 6); triangles.Add(3); triangles.Add(v + 7);
        //normals.Add(Vector3.Cross((vertices[v + 6] - vertices[3]), (vertices[ 2] - vertices[3])).normalized);
        //normals.Add(Vector3.Cross((vertices[3] - vertices[v+6]), (vertices[v + 7] - vertices[v+6])).normalized);

        for(int i=0;i<vertices.Count-4;i++)
        {
            int j = i % 4;
            switch(j)
            {
                case 0:
                    
                    normals.Add(Vector3.Cross((vertices[j + 2] - vertices[j]), (vertices[j+4] - vertices[j])).normalized);
                    break;
                case 1:
                    normals.Add(Vector3.Cross((vertices[j - 1] - vertices[j]), (vertices[j + 4] - vertices[j])).normalized);
                    break;
                case 2:
                    normals.Add(Vector3.Cross((vertices[j -2] - vertices[j]), (vertices[j + 4] - vertices[j])).normalized);
                    break;
                case 3:
                    normals.Add(Vector3.Cross((vertices[j + 1] - vertices[j]), (vertices[j + 4] - vertices[j])).normalized);
                    break;
            }
            
        }
        normals.Add(Vector3.up);
        normals.Add(Vector3.forward);
        normals.Add(-Vector3.up);
        normals.Add(-Vector3.forward);
        //填写mesh 
        mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
        mesh.RecalculateBounds();
        //mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;



    }


    void Generate()
    {

    }
    private void OnDrawGizmos()
    {
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            Gizmos.DrawRay(mesh.vertices[i], mesh.normals[i]);
        }
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.black;
    //    for (int i = 0; i < vertices.Count; i++)
    //    {
    //        Gizmos.DrawSphere(vertices[i], 0.01f);
    //    }
    //}


}
