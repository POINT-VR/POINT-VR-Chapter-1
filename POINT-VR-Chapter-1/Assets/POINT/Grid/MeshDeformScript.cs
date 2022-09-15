using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
public class MeshDeformScript : MonoBehaviour
{
    // <summary>
    // We will derive the position from which to pull based on a transform present in the scene
    // </summary>
    public Transform transformToDeformAround;
    // <summary>
    // We will get the mass from the Rigidbody component.
    // </summary>
    public Rigidbody massToDeformAround;
    [Header("Other Constants")]
    // <summary>
    // Strength of the mesh deformation
    // </summary>
    public float power;
    // <summary>
    // Radius of region affected by mesh deformation
    // </summary>
    public float cutoff;
    // <summary>
    // Mass of the object doing deformation
    // </summary>
    float mass; 
    Mesh deformingMesh;
    Vector3[] originalVertices;
    Vector3[] displacedVertices;
    void Start()
    {
        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = deformingMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        mass = massToDeformAround.mass;
        for (int i = 0; i < originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i];
        }
    }

    private void FixedUpdate()
    {
        Vector3 pullingPosition = transformToDeformAround.position;
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            Vector3 direction = originalVertices[i] - pullingPosition;
            if (direction.sqrMagnitude < cutoff)
            {
                float distance = (power * mass * direction.sqrMagnitude) / (1f + (direction.sqrMagnitude) * (direction.sqrMagnitude));
                displacedVertices[i] = originalVertices[i] - (distance * direction); //The mesh deforms here
            }        
            else 
            {
                displacedVertices[i] = originalVertices[i]; // Reset Grid Position to the very inital grid
            }
        }
        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();
    }
}
