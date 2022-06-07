using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TeleportController : MonoBehaviour
{
    public GameObject controller;
    public GameObject teleporter;
    public InputActionReference teleportationReference;
    public UnityEvent teleportActivate;
    public UnityEvent teleportCancel;
    private void Start()
    {
        teleportationReference.action.performed += TeleportModeActivate;
        teleportationReference.action.canceled += TeleportModeDeactivate;
    }
    private void TeleportModeActivate(InputAction.CallbackContext ctx) => teleportActivate.Invoke();
    private void TeleportModeDeactivate(InputAction.CallbackContext ctx) => Invoke(nameof(Cancel), .1f);
    private void Cancel() => teleportCancel.Invoke();
}