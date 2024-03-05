using UnityEngine;
using UnityEngine.InputSystem;

public class HighlightManager : MonoBehaviour
{
    [Header("Input Action References")]
    [SerializeField] private InputActionReference thumbstickReference;
    [SerializeField] private InputActionReference secondaryBtnReference;
    [SerializeField] private InputActionReference primaryBtnReference;
    [SerializeField] private InputActionReference startBtnReference;
    [SerializeField] private InputActionReference triggerReference;
    [SerializeField] private InputActionReference gripReference;

    [Header("Image GameObjects")]
    [SerializeField] private GameObject thumbstick;
    [SerializeField] private GameObject secondaryBtn;
    [SerializeField] private GameObject primaryBtn;
    [SerializeField] private GameObject startBtn;
    [SerializeField] private GameObject trigger;
    [SerializeField] private GameObject grip;

    private bool isActive = true;
    public bool IsActive
    {
        set
        {
            isActive = value;
        }
    }
    private Transform cameraTransform;

    private void OnEnable()
    {
        thumbstickReference.action.Enable();
        secondaryBtnReference.action.Enable();
        primaryBtnReference.action.Enable();
        startBtnReference.action.Enable();
        triggerReference.action.Enable();
        gripReference.action.Enable();
        secondaryBtnReference.action.started += (ctx) => { secondaryBtn.SetActive(true); };
        secondaryBtnReference.action.canceled += (ctx) => { secondaryBtn.SetActive(false); };
        primaryBtnReference.action.started += (ctx) => { primaryBtn.SetActive(true); };
        primaryBtnReference.action.canceled += (ctx) => { primaryBtn.SetActive(false); };
        startBtnReference.action.started += (ctx) => { startBtn.SetActive(true); };
        startBtnReference.action.canceled += (ctx) => { startBtn.SetActive(false); };
        triggerReference.action.started += (ctx) => { trigger.SetActive(true); };
        triggerReference.action.canceled += (ctx) => { trigger.SetActive(false); };
        gripReference.action.started += (ctx) => { grip.SetActive(true); };
        gripReference.action.canceled += (ctx) => { grip.SetActive(false); };
    }
    private void OnDisable()
    {
        thumbstickReference.action.Disable();
        secondaryBtnReference.action.Disable();
        primaryBtnReference.action.Disable();
        startBtnReference.action.Disable();
        triggerReference.action.Disable();
        gripReference.action.Disable();
        secondaryBtnReference.action.started -= (ctx) => { secondaryBtn.SetActive(true); };
        secondaryBtnReference.action.canceled -= (ctx) => { secondaryBtn.SetActive(false); };
        primaryBtnReference.action.started -= (ctx) => { primaryBtn.SetActive(true); };
        primaryBtnReference.action.canceled -= (ctx) => { primaryBtn.SetActive(false); };
        startBtnReference.action.started -= (ctx) => { startBtn.SetActive(true); };
        startBtnReference.action.canceled -= (ctx) => { startBtn.SetActive(false); };
        triggerReference.action.started -= (ctx) => { trigger.SetActive(true); };
        triggerReference.action.canceled -= (ctx) => { trigger.SetActive(false); };
        gripReference.action.started -= (ctx) => { grip.SetActive(true); };
        gripReference.action.canceled -= (ctx) => { grip.SetActive(false); };
    }

    private void Update()
    {
        if (cameraTransform != null)
        {
            this.transform.LookAt(cameraTransform);
        } else if (Camera.current != null)
        {
            cameraTransform = Camera.current.transform;
        }

        // Note: thumbstick input has to be read manually due to thumbstickTouched and thumbstickClicked
        // in the Input Manager either ignoring deadzones or constantly being triggered
        if (thumbstickReference.action.ReadValue<Vector2>().magnitude > 0.0f) thumbstick.SetActive(true);
        else thumbstick.SetActive(false);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (isActive)
        {
            this.GetComponent<Canvas>().enabled = hasFocus;
        }
    }
}
