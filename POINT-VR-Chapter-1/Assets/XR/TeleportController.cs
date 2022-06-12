using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

public class TeleportController : MonoBehaviour
{
    public InputActionReference teleportationReference, teleportCancelReference;
    public UnityEvent teleporting;
    public bool inTeleportMode;
    private void OnEnable()
    {
        teleportationReference.action.Enable();
        teleportationReference.action.performed += TeleportModeActivate;
        teleportationReference.action.canceled += TeleportNow;
        teleportCancelReference.action.Enable();
        teleportCancelReference.action.started += TeleportModeCancel;
    }
    private void OnDisable()
    {
        teleportationReference.action.Disable();
        teleportationReference.action.performed -= TeleportModeActivate;
        teleportationReference.action.canceled -= TeleportNow;
        teleportCancelReference.action.Disable();
        teleportCancelReference.action.started -= TeleportModeCancel;
    }
    private void TeleportModeActivate(InputAction.CallbackContext ctx) => inTeleportMode = true;
    private void TeleportNow(InputAction.CallbackContext ctx) { 
        teleporting.Invoke();
        inTeleportMode = false;
    }
    private void TeleportModeCancel(InputAction.CallbackContext ctx) => inTeleportMode = false;
}