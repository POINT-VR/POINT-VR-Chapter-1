using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
public class MeshDeformScript : MonoBehaviour
{
    // <summary>
    // We will get the masses from the Rigidbody components
    // </summary>
    public Rigidbody[] rigidbodiesToDeformAround;
    [Header("Other Constants")]
    // <summary>
    // Strength of the mesh deformation
    // </summary>
    public float power;
    // <summary>
    // Radius of region affected by mesh deformation
    // </summary>
    // public float cutoff;
    Mesh deformingMesh;
    Vector3[] originalVertices;
    Vector3[] displacedVertices;
    readonly float thickness = 0.02f;
    void Start()
    {
        deformingMesh = GetComponent<MeshFilter>().mesh;
        displacedVertices = (Vector3[]) deformingMesh.vertices.Clone();
        originalVertices = new Vector3[displacedVertices.Length/8];
        for (int i = 0; i < originalVertices.Length; i++)
        {
            originalVertices[i] = displacedVertices[8*i + 1];
        }
    }

    private void FixedUpdate()
    {
        for (int i = 1; i < displacedVertices.Length; i += 8)
        {
            Vector3 currentPosition = originalVertices[(i-1)/8];
            for (int j = 0; j < rigidbodiesToDeformAround.Length; j++)
            {
                Vector3 direction = currentPosition - rigidbodiesToDeformAround[j].transform.position;
                float distance = (power * rigidbodiesToDeformAround[j].mass * direction.sqrMagnitude) / (1f + (direction.sqrMagnitude) * (direction.sqrMagnitude));
                currentPosition -= distance * direction;
            }
            displacedVertices[i - 1] = currentPosition - new Vector3(thickness, 0f, 0f);
            displacedVertices[i] = currentPosition;
            displacedVertices[i + 1] = currentPosition - new Vector3(thickness, thickness, 0f);
            displacedVertices[i + 2] = currentPosition - new Vector3(0f, thickness, 0f);
            displacedVertices[i + 3] = currentPosition - new Vector3(thickness, 0f, thickness);
            displacedVertices[i + 4] = currentPosition - new Vector3(0f, 0f, thickness);
            displacedVertices[i + 5] = currentPosition - new Vector3(thickness, thickness, thickness);
            displacedVertices[i + 6] = currentPosition - new Vector3(0f, thickness, thickness);
            // cutoff code is archived here for now. note that the cutoff variable has been commented out as well.
            //           if (direction.sqrMagnitude < cutoff)
            //           {
            //           }        
            //           else 
            //           {
            //               displacedVertices[i] = originalVertices[i]; // Reset Grid Position to the inital grid
            //           }
        }
        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();
    }
}
