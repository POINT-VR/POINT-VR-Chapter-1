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
    void OnEnable()
    {
        turnReference.action.Enable();
    }
    void OnDisable()
    {
        turnReference.action.Disable();
    }
    void Update()
    {
       transform.Rotate(Vector3.up, Math.Sign(turnReference.action.ReadValue<Vector2>().x) * increment);
    }
}
