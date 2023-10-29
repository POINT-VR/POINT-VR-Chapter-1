using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class PlaneScript : MonoBehaviour
{
    /// <summary>
    /// The mesh object.
    /// </summary>
    Mesh mesh;

    /// <summary>
    /// Describe the ammount of 'cells' in the x/z directions.
    /// </summary>
    public int xSize, zSize;

    /// <summary>
    /// Describes the distance between gridmarks or the size of the 'cells'.
    /// </summary>
    public float spacing;
    private void Awake()
    {
        Generate();
    }
    private void Generate()
    {
        Vector3 offset = - new Vector3(xSize * spacing / 2, 0, zSize * spacing / 2); //offset ensures the plane is centered at the object

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Coordinate Plane";
        

        Vector3[] vertices = new Vector3[2*(xSize + 1 + zSize + 1)]; //n of vertices on all the edges (4 corners end up double counted)
        int i = 0;
        for (int x = 0; x < xSize + 1; x++)
        {
            vertices[i] = new Vector3(x * spacing, 0, 0) + offset; //defines vertices equal lengths apart along all the edges of the plane  
            vertices[i + 1] = new Vector3(x * spacing, 0, zSize * spacing) + offset;
            i += 2;
        }
        for (int z = 0; z < zSize + 1; z++)
        {
            vertices[i] = new Vector3(0, 0, z * spacing) + offset;
            vertices[i + 1] = new Vector3(xSize * spacing, 0, z * spacing) + offset;
            i += 2;
        }

        mesh.vertices = vertices;

        int[] lines = new int[vertices.Length];
        for (int k  = 0; k < vertices.Length; k++)
        {
            lines[k] = k; //connects each vertex to the one opposite to it, drawing a line through the grid
        }
        mesh.SetIndices(lines,MeshTopology.Lines,0);
    }

    public void HidePlane()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void ShowPlane()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }
}
