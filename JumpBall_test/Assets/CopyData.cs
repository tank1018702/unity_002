using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyData : MonoBehaviour
{

    MeshFilter meshFilter;
	void Start ()
    {

        meshFilter = GetComponent<MeshFilter>();


	}
	
	public Mesh GetMeshData()
    {
        Mesh mesh = new Mesh();

        mesh = meshFilter.mesh;
        return mesh;

    }
	void Update ()
    {
		
	}
}
