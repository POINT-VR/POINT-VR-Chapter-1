using UnityEngine.InputSystem;
using UnityEngine;
using System;

/// <summary>
/// This script controls the motion of the player
/// </summary>
public class TurnControllerEmulator : MonoBehaviour
{
    /// <summary>
    /// Input refrence for right/left motion
    /// </summary>
    [Header("Input Action References")]
    [SerializeField] InputActionReference horizontalMotionRefrence;
    /// <summary>
    /// Input refrence for forward/back motion
    /// </summary>
    [SerializeField] InputActionReference forwardMotionRefrence;
    /// <summary>
    /// Input refrence for up/down motion
    /// </summary>
    [SerializeField] InputActionReference verticalMotionRefrence;
    /// <summary>
    /// Input refrence for rotation in the xz plane
    /// </summary>
    [SerializeField] InputActionReference horizontalTurnRefrence;
    /// <summary>
    /// Input refrence for rotation in the zy plane
    /// </summary>
    [SerializeField] InputActionReference verticalTurnRefrence;
    /// <summary>
    /// Input refrence for rotation reset
    /// </summary>
    [SerializeField] InputActionReference resetRefrence;
    /// <summary>
    /// Adjusts how quickly the player turns
    /// </summary>
    [Header("Increment Parameters")]
    [SerializeField] float turnIncrement;
    /// <summary>
    /// Adjusts how quickly the player moves
    /// </summary>
    [SerializeField] float motionIncrement;
    void OnEnable()
    {
        horizontalMotionRefrence.action.Enable();
        forwardMotionRefrence.action.Enable();
        verticalMotionRefrence.action.Enable();
        horizontalTurnRefrence.action.Enable();
        verticalTurnRefrence.action.Enable();
        resetRefrence.action.Enable();
    }
    void OnDisable()
    {
        horizontalMotionRefrence.action.Disable();
        forwardMotionRefrence.action.Disable();
        verticalMotionRefrence.action.Disable();
        horizontalTurnRefrence.action.Disable();
        verticalTurnRefrence.action.Disable();
        resetRefrence.action.Disable();
    }
    void Update()
    {
        Vector3 finalEulerAngles = transform.eulerAngles;
        Vector3 finalPosition = transform.position;
        if (resetRefrence.action.ReadValue<float>() == 1) //reset hotkey corrects sets the vertical angle of the player to be 0
        {
            finalEulerAngles.x = 0;
            transform.eulerAngles = finalEulerAngles;
            return;
        }

        //rotation inputs
        int horizontalTurn = Math.Sign(horizontalTurnRefrence.action.ReadValue<float>());
        finalEulerAngles += new Vector3(0, horizontalTurn * turnIncrement, 0);

        int verticalTurn = Math.Sign(verticalTurnRefrence.action.ReadValue<float>());
        finalEulerAngles += new Vector3(-verticalTurn * turnIncrement, 0, 0);

        //forward and side to side motion are dependent on the angle of the player
        int horizontalMotion = Math.Sign(horizontalMotionRefrence.action.ReadValue<float>()); 
        finalPosition += transform.right * horizontalMotion * motionIncrement;

        int forwardMotion = Math.Sign(forwardMotionRefrence.action.ReadValue<float>());
        finalPosition += transform.forward * forwardMotion * motionIncrement;

        //up and down motion is always relative to the xz plane
        int verticalMotion = Math.Sign(verticalMotionRefrence.action.ReadValue<float>());
        finalPosition += new Vector3(0, verticalMotion * motionIncrement, 0);

        //prevents the player from rotating too far up or down
        finalEulerAngles.x = (finalEulerAngles.x >= 90 && transform.eulerAngles.x <= 90) ? 89.9999f : finalEulerAngles.x; 
        finalEulerAngles.x = (finalEulerAngles.x <= 270 && transform.eulerAngles.x >= 270) ? 270.0001f : finalEulerAngles.x;

        //the 'tilt' rotation of the player should always be zero
        finalEulerAngles.z = 0;
        
        //updates rotation and position
        transform.eulerAngles = finalEulerAngles;
        transform.position = finalPosition;
    }
}
