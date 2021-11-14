using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MenuBehavior : MonoBehaviour
{
    public List<UnityEngine.XR.InputDevice> leftHandedControllers = new();
    public UnityEngine.XR.InputDevice left;
                               // Start is called before the first frame update
    void Start()
    {
        var desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);
        left = leftHandedControllers[0];
    }

    // Update is called once per frame
    void Update()
    {
        /* 
                 var board = Keyboard.current;
                if (board == null)
                {
                    return;
                }
                if (board.spaceKey.isPressed)
      */
        if (left.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool triggerValue) && triggerValue)
        {
            if (transform.position.y < 0)
            {
                transform.Translate(Vector3.up * 150);
            }
        }
        else
        {
            if (transform.position.y > -100)
            {
                transform.Translate(Vector3.up * -150);
            }
        }
    }
}