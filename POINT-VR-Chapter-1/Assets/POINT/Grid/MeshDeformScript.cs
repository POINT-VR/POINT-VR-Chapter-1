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
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            Vector3 currentPosition = originalVertices[i];
            for (int j = 0; j < rigidbodiesToDeformAround.Length; j++)
            {
                Vector3 direction = currentPosition - rigidbodiesToDeformAround[j].transform.position;
                float distance = (power * rigidbodiesToDeformAround[j].mass * direction.sqrMagnitude) / (1f + (direction.sqrMagnitude) * (direction.sqrMagnitude));
                currentPosition -= distance * direction;
            }
            displacedVertices[i] = currentPosition; //The mesh deforms here
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
