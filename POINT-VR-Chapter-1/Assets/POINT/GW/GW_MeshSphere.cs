using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GW_MeshSphere : MonoBehaviour
{
    [SerializeField] private GameObject SphereMeshObject;
    [SerializeField] private GW_GravityScript gravityScript;

    [SerializeField] private float PercentOfPlusMode;
    [SerializeField] private float PercentOfCrossMode;
    [SerializeField] private float PercentOfBreathingMode;
    [SerializeField] private float PercentOfLongitudinalMode;
    [SerializeField] private float PercentOfXMode;
    [SerializeField] private float PercentOfYMode;

    private Mesh sphereMesh;
    private Vector3[] vertices;
    private Vector3 center;
    private List<Vector3> verticesCenters = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        center = transform.position;
        sphereMesh = SphereMeshObject.GetComponent<MeshFilter>().mesh;
        vertices = sphereMesh.vertices;
        

        for (var i = 0; i < vertices.Length; i++)
        {
            verticesCenters.Add(vertices[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[i] = gravityScript.CalculateOscillations(verticesCenters[i], center, PercentOfPlusMode, PercentOfCrossMode, PercentOfBreathingMode, PercentOfLongitudinalMode, PercentOfXMode, PercentOfYMode);
        }

        // assign the local vertices array into the vertices array of the Mesh.
        sphereMesh.vertices = vertices;
        sphereMesh.RecalculateBounds();
        sphereMesh.RecalculateNormals();
    }
}
