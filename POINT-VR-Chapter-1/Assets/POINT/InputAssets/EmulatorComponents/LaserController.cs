using UnityEngine.InputSystem;
using UnityEngine;

/// <summary>
/// This script controls the motion of the player's hand
/// </summary>
public class LaserController : MonoBehaviour
{
    /// <summary>
    /// Input reference for rotation
    /// </summary>
    [SerializeField] InputActionReference turnRefrence;
    /// <summary>
    /// Adjusts how quickly the hand turns
    /// </summary>
    [SerializeField] float turnIncrement;
    void Start()
    {
        Cursor.visible = false; //hides the cursor on start to make the laser input feel more natural
    }
    void OnEnable()
    {
        turnRefrence.action.Enable();
        turnRefrence.action.performed += Turn;
    }
    void OnDisable()
    {
        turnRefrence.action.Disable();
        turnRefrence.action.performed -= Turn;
    }
    /// <summary>
    /// Called whenever mouse delta is detected; adjusts the rotation of the hand
    /// </summary>
    void Turn(InputAction.CallbackContext obj)
    {
        Vector3 finalEulerAngles = transform.localEulerAngles;
        
        float verticalDelta = obj.action.ReadValue<Vector2>().y;
        float horizontalDelta = obj.action.ReadValue<Vector2>().x;

        finalEulerAngles.x -= verticalDelta * turnIncrement;
        finalEulerAngles.y += horizontalDelta * turnIncrement;

        //Makes sure the pointer doesn't rotate too far out of frame
        finalEulerAngles.x = (finalEulerAngles.x >= 70 && transform.localEulerAngles.x <= 70) ? 69.9999f : finalEulerAngles.x;
        finalEulerAngles.x = (finalEulerAngles.x <= 290 && transform.localEulerAngles.x >= 290) ? 290.0001f : finalEulerAngles.x;

        finalEulerAngles.y = (finalEulerAngles.y >= 90 && transform.localEulerAngles.y <= 90) ? 89.9999f : finalEulerAngles.y;
        finalEulerAngles.y = (finalEulerAngles.y <= 270 && transform.localEulerAngles.y >= 270) ? 270.0001f : finalEulerAngles.y;

        transform.localEulerAngles = finalEulerAngles;
    }
}
