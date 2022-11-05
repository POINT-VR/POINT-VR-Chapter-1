using UnityEngine;
using System.Collections;
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
        displacedVertices = (Vector3[]) deformingMesh.vertices.Clone();
        originalVertices  = (Vector3[]) deformingMesh.vertices.Clone();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < displacedVertices.Length; i ++)
        {
            Vector3 currentPosition = originalVertices[i];
            for (int j = 0; j < rigidbodiesToDeformAround.Length; j++)
            {
                Vector3 direction = currentPosition - rigidbodiesToDeformAround[j].transform.position;

                float distance = power * 1f;

                if ( ((1f*rigidbodiesToDeformAround[j].mass) <= direction.magnitude) ) // If the sqrt above is negative, this gets called.
                {
                    distance = power * (1f - Mathf.Sqrt( 1f  - (1f*rigidbodiesToDeformAround[j].mass)/direction.magnitude  ) );
                }

                currentPosition -= distance * direction;
            }
            displacedVertices[i] = currentPosition;

        }
        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();
    }
}
