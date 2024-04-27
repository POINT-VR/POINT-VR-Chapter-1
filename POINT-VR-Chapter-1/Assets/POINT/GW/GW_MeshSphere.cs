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

    [Header("For Editor Usage - See Toggle Menus for percent value")]
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
    private List<Vector3> xyRingCenters = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        center = transform.position;
        sphereMesh = SphereMeshObject.GetComponent<MeshFilter>().mesh;
        vertices = sphereMesh.vertices;
        

        for (var i = 0; i < vertices.Length; i++)
        {
            verticesCenters.Add(vertices[i]);
            Vector3 ringCenter = new Vector3(0, 0, vertices[i].z);
            xyRingCenters.Add(ringCenter);
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
            //vertices[i] = gravityScript.CalculateOscillations(verticesCenters[i], center, PercentOfPlusMode, PercentOfCrossMode, PercentOfBreathingMode, PercentOfLongitudinalMode, PercentOfXMode, PercentOfYMode);
            //vertices[i] = gravityScript.CalculateOscillations(verticesCenters[i], xyRingCenters[i], 0.25f*verticesCenters[i].magnitude, 0.0f, 0.0f, PercentOfPlusMode, PercentOfCrossMode, PercentOfBreathingMode, PercentOfLongitudinalMode, PercentOfXMode, PercentOfYMode);
            vertices[i] = gravityScript.CalculateOscillations(verticesCenters[i], 0.0f, 0.0f, PercentOfPlusMode, PercentOfCrossMode, PercentOfBreathingMode, PercentOfLongitudinalMode, PercentOfXMode, PercentOfYMode);
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

    public void SetPlusMode(float percent) { PercentOfPlusMode = percent; }
    public void SetCrossMode(float percent) { PercentOfCrossMode = percent; }
    public void SetBreathingMode(float percent) { PercentOfBreathingMode = percent; }
    public void SetLongitudinalMode(float percent) { PercentOfLongitudinalMode = percent; }
    public void SetXMode(float percent) { PercentOfXMode = percent; }
    public void SetYMode(float percent) { PercentOfYMode = percent; }

    public float GetPlusMode() { return PercentOfPlusMode; }
    public float GetCrossMode() { return PercentOfCrossMode; }
    public float GetBreathingMode() { return PercentOfBreathingMode; }
    public float GetLongitudinalMode() { return PercentOfLongitudinalMode; }
    public float GetXMode() { return PercentOfXMode; }
    public float GetYMode() { return PercentOfYMode; }
}
