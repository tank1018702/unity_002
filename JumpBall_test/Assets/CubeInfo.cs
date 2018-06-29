using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInfo : MonoBehaviour {
    MeshFilter meshFilter;
    // Use this for initialization
    Mesh mesh;
    private void Awake()
    {
        meshFilter = transform.GetComponent<MeshFilter>();

         mesh= meshFilter.mesh;

        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
        foreach (var i in mesh.vertices)
        {
            Debug.Log("顶点:" + i);


        }

        foreach (var j in mesh.normals)
        {

            Debug.Log("法线:" + j);


        }
        foreach (var n in mesh.uv)
        {
            Debug.Log("uv:" + "("+n.x+n.y+")");


        }
        
        Debug.Log("顶点个数:" + mesh.vertices.Length);
        Debug.Log("法线个数:" + mesh.normals.Length);
        Debug.Log("uv坐标个数:" + mesh.uv.Length);
    }
    void Start ()
    {
        StartCoroutine(test());
    }
	
	// Update is called once per frame
	void Update ()
    {
       
	}
    private void OnDrawGizmos()
    {
       
    }
    IEnumerator test()
    {
        int j = 0;

        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {

            //Debug.Log(j);
            Debug.DrawLine(mesh.vertices[mesh.triangles[i]], mesh.vertices[mesh.triangles[i + 1]], Color.red, 2f);

            yield return new WaitForSeconds(Time.deltaTime);
            Debug.DrawLine(mesh.vertices[mesh.triangles[i + 1]], mesh.vertices[mesh.triangles[i + 2]], Color.yellow, 2f);

            yield return new WaitForSeconds(Time.deltaTime);
            Debug.DrawLine(mesh.vertices[mesh.triangles[i + 2]], mesh.vertices[mesh.triangles[i]], Color.blue, 2f);

            yield return new WaitForSeconds(Time.deltaTime);

            Debug.DrawRay(mesh.vertices[j], mesh.normals[j], new Color(0,0,0,j%10*1.0f), 1000f);
            j++;



        }
   

    }

  
}
