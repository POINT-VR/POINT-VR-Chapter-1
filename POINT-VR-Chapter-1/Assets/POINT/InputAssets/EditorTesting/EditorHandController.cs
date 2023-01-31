using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(XRHardwareController))]
public class EditorHandController : MonoBehaviour
{
    /// <summary>
    /// The amount by which this hand moves per frame of action
    /// </summary>
    [SerializeField] float delta;
    /// <summary>
    /// Input reference for the arrow keys
    /// </summary>
    [SerializeField] InputActionReference motionReference;
#if UNITY_EDITOR
    private void OnEnable()
    {
        GetComponent<XRHardwareController>().enabled = false; //no conflict of movement between this and XRHardwareController
        motionReference.action.Enable();
        transform.parent.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false; //ensure the left hand stays above the right hand in the hierarchy
    }
    private void OnDisable()
    {
        GetComponent<XRHardwareController>().enabled = true;
        motionReference.action.Disable();
        transform.parent.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
    }
    private void Update()
    {
        Vector2 pos = motionReference.action.ReadValue<Vector2>();
        transform.localPosition = new Vector3(pos.x * delta + transform.localPosition.x, pos.y * delta + transform.localPosition.y, 0f);
    }
#endif
}
