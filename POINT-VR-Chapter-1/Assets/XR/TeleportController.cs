using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TeleportController : MonoBehaviour
{
    public GameObject controller;
    public GameObject teleporter;
    public InputActionReference teleportationReference, cancellationReference;
    public UnityEvent teleportActivate;
    public UnityEvent teleportCancel;
    private void OnEnable()
    {
        teleportationReference.action.performed += TeleportModeActivate;
        teleportationReference.action.canceled += TeleportModeDeactivate;
        cancellationReference.action.canceled += CancelNow;
    }
    private void OnDisable()
    {
        teleportationReference.action.performed -= TeleportModeActivate;
        teleportationReference.action.canceled -= TeleportModeDeactivate;
        cancellationReference.action.canceled -= CancelNow;
    }
    private void TeleportModeActivate(InputAction.CallbackContext ctx) => teleportActivate.Invoke();
    private void TeleportModeDeactivate(InputAction.CallbackContext ctx) => Invoke(nameof(Cancel), .1f);
    private void CancelNow(InputAction.CallbackContext ctx) => teleportCancel.Invoke();
    void Cancel() => teleportCancel.Invoke();
}