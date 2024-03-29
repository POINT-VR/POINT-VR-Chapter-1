using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class GW_MeshSphere : MonoBehaviour
{
    [SerializeField] private GameObject SphereMeshObject;
    [SerializeField] private GW_GravityScript gravityScript;

    [SerializeField] public Toggle[] ModeToggles;
    private float PercentOfPlusMode;
    private float PercentOfCrossMode;
    private float PercentOfBreathingMode;
    private float PercentOfLongitudinalMode;
    private float PercentOfXMode;
    private float PercentOfYMode;

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

        PercentOfPlusMode = 0;
        PercentOfCrossMode = 0;
        PercentOfBreathingMode = 0;
        PercentOfLongitudinalMode = 0;
        PercentOfXMode = 0;
        PercentOfYMode = 0;
    }

    // Update is called once per frame
    void Update()
    {
        

        
        
    }

    void FixedUpdate()
    {
        /**Thread t = new Thread(() => DisplaceVertices());
        t.Start();
        DisplaceVertices();
        t.Join();**/
        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[i] = gravityScript.CalculateOscillations(verticesCenters[i], center, PercentOfPlusMode, PercentOfCrossMode, PercentOfBreathingMode, PercentOfLongitudinalMode, PercentOfXMode, PercentOfYMode);
        }
        // assign the local vertices array into the vertices array of the Mesh.
        sphereMesh.vertices = vertices;

        sphereMesh.RecalculateBounds();
        sphereMesh.RecalculateNormals();
    }

    void DisplaceVertices()
    {
        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[i] = gravityScript.CalculateOscillations(verticesCenters[i], center, PercentOfPlusMode, PercentOfCrossMode, PercentOfBreathingMode, PercentOfLongitudinalMode, PercentOfXMode, PercentOfYMode);
        }
        // assign the local vertices array into the vertices array of the Mesh.
        sphereMesh.vertices = vertices;
    }

    public void SetPlusMode(Toggle t) {
        if (PercentOfPlusMode == 0) {
            PercentOfPlusMode = 100;
            t.isOn = true;
        } else {
            PercentOfPlusMode = 0;
            t.isOn = false;
        }
    }

    public void SetCrossMode(Toggle t) {
        if (PercentOfCrossMode == 0) {
            PercentOfCrossMode = 100;
            t.isOn = true;
        } else {
            PercentOfCrossMode = 0;
            t.isOn = false;
        }
    }

    public void SetBreathingMode(Toggle t) {
        if (PercentOfBreathingMode == 0) {
            PercentOfBreathingMode = 100;
            t.isOn = true;
        } else {
            PercentOfBreathingMode = 0;
            t.isOn = false;
        }
    }

    public void SetLongitudinalMode(Toggle t) {
        if (PercentOfLongitudinalMode == 0) {
            PercentOfLongitudinalMode = 100;
            t.isOn = true;
        } else {
            PercentOfLongitudinalMode = 0;
            t.isOn = false;
        }
    }

    public void SetXMode(Toggle t) {
        if (PercentOfXMode == 0) {
            PercentOfXMode = 100;
            t.isOn = true;
        } else {
            PercentOfXMode = 0;
            t.isOn = false;
        }
    }

    public void SetYMode(Toggle t) {
        if (PercentOfYMode == 0) {
            PercentOfYMode = 100;
            t.isOn = true;
        } else {
            PercentOfYMode = 0;
            t.isOn = false;
        }
    }
}
