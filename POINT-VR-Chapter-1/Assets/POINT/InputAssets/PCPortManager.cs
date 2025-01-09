using UnityEngine;

public class PCPortManager : MonoBehaviour
{
    [Header("PC Port-Specific Values")]
    /// <summary>
    /// Scale of UI Canvas in the PC port
    /// </summary>
    [SerializeField] private float uiScale = 1.0f;
    /// <summary>
    /// Position offset of UI Canvas in the PC port
    /// </summary>
    [SerializeField] private Vector3 uiOffset = Vector3.zero;
    /// <summary>
    /// Mouse sensitivity for camera rotation
    /// </summary>
    [SerializeField] private float cameraSens = 5.0f;
    public float CameraSens
    {
        get
        {
            return cameraSens;
        }
    }
    [Header("References")]
    /// <summary>
    /// All UI Canvases to be scaled and offset
    /// </summary>
    [SerializeField] private Canvas[] uiCanvas;
    /// <summary>
    /// Player camera
    /// </summary>
    [SerializeField] private Camera mainCamera;
    /// <summary>
    /// Left hand controller
    /// </summary>
    [SerializeField] private XRHardwareController leftHandController;
    /// <summary>
    /// Right hand controller
    /// </summary>
    [SerializeField] private XRHardwareController rightHandController;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    private void Start()
    {
        // UI adjustments
        foreach (Canvas canvas in uiCanvas)
        {
            canvas.transform.localScale = uiScale * Vector3.one;
            (canvas.transform as RectTransform).position += uiOffset;
        }

        // Hand controller adjustments
        if (leftHandController)
        {
            leftHandController.gameObject.SetActive(false);
        }
        if (rightHandController)
        {
            rightHandController.enabled = false;
            rightHandController.GetComponent<SkinnedMeshRenderer>().enabled = false;
        }
    }
#endif
}
