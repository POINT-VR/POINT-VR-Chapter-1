using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeshDeformScript : MonoBehaviour
{
    // Start is called before the first frame update
    float power = 5f;
    Vector3 pullingPosition;
    Mesh deformingMesh;
    Vector3[] originalVertices;
    Vector3[] displacedVertices;
    public InputActionReference deformReference = null;

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

    private void Awake()
    {
        deformReference.action.started += Deform;
    }

    private void Deform(InputAction.CallbackContext ctx)
    {
        pullingPosition = (Vector3) ctx.ReadValueAsObject();
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            UpdateVertex(i);
        }
        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();
    }

    private void OnDestroy()
    {
        deformReference.action.started -= Deform;
    }

    void UpdateVertex(int i)
    {
        Vector3 direction = originalVertices[i] - pullingPosition;
        float distance = (power * direction.sqrMagnitude) / (1f + (direction.sqrMagnitude)*(direction.sqrMagnitude));
        displacedVertices[i] = direction.normalized * distance;
    }
}
