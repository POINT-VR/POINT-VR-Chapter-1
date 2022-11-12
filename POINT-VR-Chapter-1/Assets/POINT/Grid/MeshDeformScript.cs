using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
public class MeshDeformScript : MonoBehaviour
{
    // <summary>
    // Generic MeshDeformScipt. Should work on any Grid Model.
    // </summary>

    // <summary>
    // We will get the masses from the Rigidbody components
    // </summary>
    public Rigidbody[] rigidbodiesToDeformAround;
    [Header("Other Constants")]
    // <summary>
    // Strength of the mesh deformation
    // </summary>
    // public float power;
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
        displacedVertices = (Vector3[]) deformingMesh.vertices.Clone();
        originalVertices  = (Vector3[]) deformingMesh.vertices.Clone();
    }

    private void FixedUpdate()
    {
        Vector3[] massPositions = new Vector3[rigidbodiesToDeformAround.Length];
        for (int j = 0; j < rigidbodiesToDeformAround.Length; j++) //Puts the mass positions on the stack ahead of time
        {
            massPositions[j] = rigidbodiesToDeformAround[j].transform.position;
        }
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            Vector3 totalDisplacement = new Vector3(0f, 0f, 0f);
            for (int j = 0; j < rigidbodiesToDeformAround.Length; j++)
            {
                Vector3 direction = originalVertices[i] - massPositions[j];
                float distance = 1f;
                if (2*rigidbodiesToDeformAround[j].mass < direction.magnitude) //Displacement would not yield a complex number: deform at damped power
                {
                    distance = (1f - Mathf.Sqrt(1f - 2*rigidbodiesToDeformAround[j].mass / direction.magnitude));
                }
                totalDisplacement += distance * direction / rigidbodiesToDeformAround.Length; //Displacement from each mass is calculated independently, but combined by vector addition
            }
            displacedVertices[i] = originalVertices[i] - totalDisplacement; //Store the final displacement calculation for this vertex
        }
        deformingMesh.vertices = displacedVertices; //This is where the grid actually applies all of the calculations
        deformingMesh.RecalculateNormals();
    }
}
