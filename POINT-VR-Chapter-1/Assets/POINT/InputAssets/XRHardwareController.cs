﻿using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
/// <summary>
/// This script should be attached to the GameObject that is meant to represent a physical hardware device in-game.
/// </summary>
public class XRHardwareController : MonoBehaviour
{
    /// <summary>
    /// The three types of hardware in VR development
    /// </summary>
    [SerializeField] enum Hardware {Headset, LeftHand, RightHand};
    /// <summary>
    /// The type of hardware that this specific component takes the role of
    /// </summary>
    [SerializeField] Hardware hardwareType;
    /// <summary>
    /// The input reference reading this hardware's position in the real world
    /// </summary>
    [Header("References")]
    [SerializeField] InputActionReference positionReference;
    /// <summary>
    /// The input reference reading this hardware's rotation in the real world
    /// </summary>
    [SerializeField] InputActionReference rotationReference;
    /// <summary>
    /// The height of the player in the game world. This is instantiated according to the height of the camera.
    /// </summary>
    private float playerHeight;
    // This represents a controller that can receive haptic feedback
    private XRControllerWithRumble inputDevice;
    // The reference used to get the device's velocity
    private UnityEngine.XR.InputDevice velocityDevice;
    /// <summary>
    /// Reads the hardware device's velocity
    /// </summary>
    public Vector3 Velocity { get { velocityDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceVelocity, out Vector3 velocity); return velocity; } }
    // This is the distance the headset has travelled from where the player began playing the simulation.
    // In the intended/most common use case, this should be equal to Vector3.zero
    private static Vector3 offset;
    /// <summary>
    /// Flag that decides whether the controller haptics are enabled or not. Initialized in editor.
    /// </summary>
    [SerializeField] private bool hapticsEnabled;
    /// <summary>
    /// Public property that exposes flag for haptics to UI events.
    /// </summary>
    public bool HapticsEnabled { get { return hapticsEnabled; } set { hapticsEnabled = value; } }
    /// <summary>
    /// Assigns input actions and locates the connected hardware device associated with this instance
    /// </summary>
    private void OnEnable()
    {
        playerHeight = transform.position.y;
        positionReference.action.Enable();
        rotationReference.action.Enable();
        inputDevice = null;
        foreach (UnityEngine.InputSystem.InputDevice device in InputSystem.devices) //Iterate over all connected devices
        { 
            if ((device.usages.Contains(UnityEngine.InputSystem.CommonUsages.LeftHand) && hardwareType == Hardware.LeftHand) || (device.usages.Contains(UnityEngine.InputSystem.CommonUsages.RightHand) && hardwareType == Hardware.RightHand))
            { //The device's common usage (controller hand) matches the hardware this represents: store the input device.
                inputDevice = (XRControllerWithRumble) device;
            }
        }
        switch (hardwareType)
        {
            case Hardware.Headset:
                velocityDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
                break;
            case Hardware.LeftHand:
                velocityDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                break;
            case Hardware.RightHand:
                velocityDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                break;
        }
    }
    /// <summary>
    /// Sends a pre-configured (not parameterized) haptic signal back to the hardware device.
    /// </summary>
    public void VibrateHand()
    {
#if UNITY_EDITOR //Do not vibrate hand in the editor or else this floods the console with errors
        return;
#endif
#pragma warning disable CS0162 // Unreachable code detected
        if (HapticsEnabled) {
            inputDevice.SendImpulse(0.15f, 0.05f);
        }
#pragma warning restore CS0162 // Unreachable code detected
    }
    /// <summary>
    /// Disables the actions as they are no longer used
    /// </summary>
    private void OnDisable()
    {
        positionReference.action.Disable();
        rotationReference.action.Disable();
    }
    /// <summary>
    /// Manages the movement of the controllers on a frame-by-frame basis
    /// </summary>
    void Update()
    {
        if (hardwareType == Hardware.Headset) //The headset will not update in Update()
        {
            return;
        }
        UpdateHardware();
    }
    /// <summary>
    /// Manages the movement of the headset on a frame-by-frame basis
    /// </summary>
    void LateUpdate()
    {
        if (hardwareType == Hardware.Headset) //Only the headset will update in LateUpdate()
        {
            UpdateHardware();
        }
    }
    /// <summary>
    /// Syncs up the hardware gameobjects in the simulation with the hardware in the real world
    /// </summary>
    private void UpdateHardware()
    {
        Vector3 pos = positionReference.action.ReadValue<Vector3>();
        if (hardwareType == Hardware.Headset && (pos-offset).sqrMagnitude > playerHeight * playerHeight) //Player has walked away: recenters the simulation
        {
            offset = pos;
        }
        transform.localPosition = pos - offset + Vector3.up * playerHeight;
        transform.localRotation = rotationReference.action.ReadValue<Quaternion>();
    }
}
