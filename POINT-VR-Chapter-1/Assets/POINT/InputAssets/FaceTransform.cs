using UnityEngine;

public class FaceTransform : MonoBehaviour
{
    /// <summary>
    /// The Transform that this will be oriented towards.
    /// </summary>
    [SerializeField] Transform lookAt;
    /// <summary>
    /// Every update, this faces and transforms the UI.
    /// </summary>
    void Update()
    {
        transform.LookAt(lookAt, Vector3.up);
    }
}
