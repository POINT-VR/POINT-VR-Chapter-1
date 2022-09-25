using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class TurnController : MonoBehaviour
{
    /// <summary>
    /// Input reference for turning the player
    /// </summary>
    [SerializeField] InputActionReference turnReference;
    /// <summary>
    /// How quickly the player turns
    /// </summary>
    [SerializeField] float increment;
    private bool actuatedLastFrame;
    void OnEnable()
    {
        turnReference.action.Enable();
        actuatedLastFrame = false;
    }
    void OnDisable()
    {
        turnReference.action.Disable();
    }
    void Update()
    {
        int direction = Math.Sign(turnReference.action.ReadValue<Vector2>().x);
        if (direction != 0)
        {
            if (actuatedLastFrame)
            {
                return;
            }
            transform.Rotate(Vector3.up, direction * increment);
            actuatedLastFrame = true;
        }
        else
        {
            actuatedLastFrame = false;
        }
    }
}
