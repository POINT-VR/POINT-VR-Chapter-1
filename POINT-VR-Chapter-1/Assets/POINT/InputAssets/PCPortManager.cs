using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    public Vector3 UiOffset
    {
        get
        {
            return uiOffset;
        }
    }
    /// <summary>
    /// Anchored y-coordinate for subtitles (position from bottom of screen)
    /// </summary>
    [SerializeField] private float subtitlesYPosition = 250.0f;
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
    /// <summary>
    /// Movement speed of the player (using WASD)
    /// </summary>
    [SerializeField] private float movementSpeed = 2.0f;
    /// <summary>
    /// Crosshair sprite when focused
    /// </summary>
    [SerializeField] private Sprite crosshairFocused = null;
    /// <summary>
    /// Crosshair sprite when not focused (i.e. default)
    /// </summary>
    [SerializeField] private Sprite crosshairUnfocused = null;
    [Header("References")]
    /// <summary>
    /// UI Container (Contains pause menu)
    /// </summary>
    [SerializeField] private Canvas uiContainer;
    /// <summary>
    /// Overlay Container
    /// </summary>
    [SerializeField] private Canvas overlayContainer;
    /// <summary>
    /// Subtitles UI
    /// </summary>
    [SerializeField] private RectTransform subtitles;
    /// <summary>
    /// Highlighted controls (on button input); deactivated in PC Port
    /// </summary>
    [SerializeField] private HighlightManager highlightControls;
    /// <summary>
    /// Image containing crosshair; visible only in PC Port
    /// </summary>
    [SerializeField] private Image crosshair;
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
    /// <summary>
    /// InputActionReference for player movement in PC Port
    /// </summary>
    [SerializeField] private InputActionReference playerMoveReference;

    [Header("UI Menu Changes")]
    /// <summary>
    /// UI Image for the diagram in the Controls tab of the UI menu
    /// </summary>
    [SerializeField] private Image controlsDiagram;
    /// <summary>
    /// Sprite showing the default controls for the PC Port
    /// </summary>
    [SerializeField] private Sprite controlsSprite;
    /// <summary>
    /// UI setting for live controller diagrams; deactivated in PC Port
    /// </summary>
    [SerializeField] private GameObject controllerDiagramsToggle;
    /// <summary>
    /// UI setting for haptics; deactivated in PC Port
    /// </summary>
    [SerializeField] private GameObject hapticsToggle;

#if UNITY_STANDALONE || (UNITY_EDITOR && IS_NOT_USING_OCULUS_LINK) || UNITY_WEBGL
    private void Start()
    {
        // UI adjustments
        uiContainer.transform.localScale = uiScale * Vector3.one;
        (uiContainer.transform as RectTransform).position += uiOffset;
        if (controllerDiagramsToggle)
        {
            controllerDiagramsToggle.SetActive(false);
        }
        if (hapticsToggle)
        {
            hapticsToggle.SetActive(false);
        }
        if (controlsDiagram && controlsSprite)
        {
            controlsDiagram.sprite = controlsSprite;
        }

        overlayContainer.renderMode = RenderMode.ScreenSpaceOverlay;
        subtitles.anchorMin = 0.5f * Vector2.right;
        subtitles.anchorMax = 0.5f * Vector2.right;
        subtitles.anchoredPosition = subtitlesYPosition * Vector2.up;
        SetCrossHairVisible(true);
        if (highlightControls != null)
        {
            highlightControls.enabled = false;
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

    private void OnEnable()
    {
        playerMoveReference.action.Enable();
    }

    private void OnDisable()
    {
        playerMoveReference.action.Disable();
    }

    private void Update()
    {
        Vector2 xyMovement = playerMoveReference.action.ReadValue<Vector2>();
        this.transform.parent.position += movementSpeed * Time.deltaTime * ((xyMovement.x * this.transform.parent.right) + (xyMovement.y * this.transform.forward));
    }
#endif

    public void SetCrossHairColor(Color color)
    {
        crosshair.color = color;
    }

    public void SetCrossHairFocused(bool focused)
    {
        crosshair.sprite = focused ? crosshairFocused : crosshairUnfocused;
    }

    public void SetCrossHairVisible(bool visible)
    {
        crosshair.gameObject.SetActive(visible);
        Cursor.visible = !visible; // If crosshair is not visible, cursor shows up, and vice versa
    }
}
