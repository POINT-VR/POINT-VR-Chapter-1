using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XR;

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
    private float playerHeight;
    private XRControllerWithRumble inputDevice;
    private void OnEnable()
    {
        playerHeight = transform.position.y;
        positionReference.action.Enable();
        rotationReference.action.Enable();
        inputDevice = null;
        
    }
    
    public void ReConnect()
    {
        foreach (InputDevice device in InputSystem.devices) //Iterate over all connected devices
        { 
            //print all devices
            print(device.name);
            if ((device.usages.Contains(CommonUsages.LeftHand) && hardwareType == Hardware.LeftHand) || (device.usages.Contains(CommonUsages.RightHand) && hardwareType == Hardware.RightHand))
            { //The device's common usage (controller hand) matches the hardware this represents: store the input device.
                inputDevice = (XRControllerWithRumble) device;
            }
        }
    }
    /// <summary>
    /// Sends a pre-configured (not parameterized) haptic signal back to the hardware device.
    /// </summary>
    public void VibrateHand()
    {
        // inputDevice.SendImpulse(0.15f, 0.05f);
    }
    private void OnDisable()
    {
        positionReference.action.Disable();
        rotationReference.action.Disable();
    }
    void Update()
    {
        // if (inputDevice==null)
        // {
        //     ReConnect();
        // }
        if (hardwareType == Hardware.Headset) //The headset will not update in Update()
        {
            return;
        }
        UpdateHardware();
    }
    void LateUpdate()
    {
        if (hardwareType == Hardware.Headset) //Only the headset will update in LateUpdate()
        {
            UpdateHardware();
        }
    }
    private void UpdateHardware()
    {
        //Syncs up the hardware gameobjects in the simulation with the hardware in the real world
        transform.localPosition = positionReference.action.ReadValue<Vector3>() + Vector3.up * playerHeight;
        transform.localRotation = rotationReference.action.ReadValue<Quaternion>();
    }
}
