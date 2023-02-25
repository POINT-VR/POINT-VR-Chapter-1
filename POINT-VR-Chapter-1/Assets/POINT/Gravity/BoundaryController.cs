using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class BoundaryController : MonoBehaviour
{
    [SerializeField] float height;
    [SerializeField] Transform floor;
    [SerializeField] float squaredMaxDistance;
    readonly int unityDefaultPlaneSize = 5;
    readonly float tol = 1e-3f;
    private void Update()
    {
        float x, y, z;
        float maxX = floor.position.x + unityDefaultPlaneSize * floor.localScale.x;
        float minX = floor.position.x - unityDefaultPlaneSize * floor.localScale.x;
        if (Mathf.Abs(Mathf.Clamp(transform.position.x, minX, maxX) - transform.position.x) < tol)
        {
            x = 0;
        }
        else
        {
            x = Mathf.Min(Mathf.Abs(maxX - transform.position.x), Mathf.Abs(minX - transform.position.x));
        }
        float maxY = floor.position.y + height;
        float minY = floor.position.y;
        if (Mathf.Abs(Mathf.Clamp(transform.position.y, minY, maxY) - transform.position.y) < tol)
        {
            y = 0;
        }
        else
        {
            y = Mathf.Min(Mathf.Abs(maxY - transform.position.y), Mathf.Abs(minY - transform.position.y));
        }
        float maxZ = floor.position.z + unityDefaultPlaneSize * floor.localScale.z;
        float minZ = floor.position.z - unityDefaultPlaneSize * floor.localScale.z;
        if (Mathf.Abs(Mathf.Clamp(transform.position.z, minZ, maxZ) - transform.position.z) < tol)
        {
            z = 0;
        }
        else
        {
            z = Mathf.Min(Mathf.Abs(maxZ - transform.position.z), Mathf.Abs(minZ - transform.position.z));
        }
        if (Vector3.SqrMagnitude(new Vector3(x, y, z)) > squaredMaxDistance)
        {
            transform.position = floor.up + floor.position;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
