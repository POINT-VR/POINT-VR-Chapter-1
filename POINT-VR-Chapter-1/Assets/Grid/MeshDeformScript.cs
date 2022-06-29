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
    public InputActionReference positionReference = null;

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
        pullingPosition = positionReference.action.ReadValue<Vector3>();
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            Vector3 direction = originalVertices[i] - pullingPosition;
            if (direction.sqrMagnitude < cutoff)
            {
                UpdateVertex(i, direction);
            }        
        }
        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();
    }

    void UpdateVertex(int i, Vector3 direction)
    {
        float distance = 5.0f ;
        //float distance = -(float)direction.sqrMagnitude + (float)1.0;
        //float distance = (power * direction.sqrMagnitude) / (1f + (direction.sqrMagnitude)*(direction.sqrMagnitude));
        displacedVertices[i] = direction.normalized * distance;
    }
}
