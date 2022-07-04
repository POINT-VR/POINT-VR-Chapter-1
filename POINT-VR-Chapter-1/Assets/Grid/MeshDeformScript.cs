using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeshDeformScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float power  = 5f; // strength of the mesh deformation
    public float cutoff = 1f; // radius of region affected by mesh deformation
    [SerializeField]
    Vector3 pullingPosition;
    Mesh deformingMesh;
    Vector3[] originalVertices;
    Vector3[] displacedVertices;
    public InputActionReference positionReference = null; // Controller
    public GameObject sphereObject = null; // Sphere Mesh Deformation

    void Start()
    {
        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = deformingMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        for (int i = 0; i < originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i];
        }
    }

    private void FixedUpdate()
    {
        //pullingPosition = positionReference.action.ReadValue<Vector3>(); // Removed controller mesh deformation
        pullingPosition = sphereObject.transform.position;
        
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            Vector3 direction = originalVertices[i] - pullingPosition;

            // TEST: The below code should do the same as the code above
            //Vector3 direction;
            //direction[0] = originalVertices[i][0] - pullingPosition[0]; // x - horizontal
            //direction[3] = originalVertices[i][2] - pullingPosition[2]; // z - horizontal
            //direction[1] = originalVertices[i][1] - pullingPosition[1]; // vertical

            if (direction.sqrMagnitude < cutoff)
            {
                // UpdateVertex(i, direction); // What mesh deformation we want to do
                displacedVertices[i] = pullingPosition; // Test: Make the grid snap to the sphere location 
            }        
            else 
            {
                displacedVertices[i] = originalVertices[i]; // Reset Grid Position to the very inital grid
            }
        }
        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();
    }

    void UpdateVertex(int i, Vector3 direction)
    {
        // float distance = (float)direction.sqrMagnitude;
        direction[1] = -direction[1]; // flip the vertical direction
        //float distance = (power * direction.sqrMagnitude) / (1f + (direction.sqrMagnitude)*(direction.sqrMagnitude));
        displacedVertices[i] = direction.normalized * power;
    }
}
