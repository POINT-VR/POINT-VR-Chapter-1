﻿using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleBehavior : MonoBehaviour
{
    public InputActionReference toggleReference = null;
    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void Awake()
    {
        toggleReference.action.started += Toggle;
    }

    private void Toggle(InputAction.CallbackContext ctx)
    {
        bool isActive = !gameObject.activeSelf;
        gameObject.SetActive(isActive);
    }

    private void OnDestroy()
    {
        toggleReference.action.started -= Toggle;
    }

}
