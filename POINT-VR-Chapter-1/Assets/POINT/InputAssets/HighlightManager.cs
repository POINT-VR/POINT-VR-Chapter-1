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

    private void OnEnable()
    {
        thumbstickReference.action.Enable();
        secondaryBtnReference.action.Enable();
        primaryBtnReference.action.Enable();
        startBtnReference.action.Enable();
        triggerReference.action.Enable();
        gripReference.action.Enable();
        thumbstickReference.action.performed += (ctx) => { thumbstick.SetActive(true); } ;
        thumbstickReference.action.canceled += (ctx) => { thumbstick.SetActive(false); };
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
        thumbstickReference.action.performed -= (ctx) => { thumbstick.SetActive(true); };
        thumbstickReference.action.canceled -= (ctx) => { thumbstick.SetActive(false); };
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
}
