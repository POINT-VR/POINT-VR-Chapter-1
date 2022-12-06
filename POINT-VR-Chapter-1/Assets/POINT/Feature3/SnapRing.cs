using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// When player release a object, if the object is inside the trigger, it will be put into correct position, only allow one object in the trigger
/// if there is already an object in the trigger, the new object will be ignored
/// </summary>
[RequireComponent(typeof(Collider))]
public class SnapRing : MonoBehaviour
{
    // hold current colliding objects inside the trigger
    private Rigidbody collidingObject = null;
    // a snap position
    private Transform snapPosition;
    [SerializeField] private InputActionReference rightGrabReference;
    [SerializeField] private InputActionReference leftGrabReference;
    
    private void Start()
    {
        // Get the snap position
        snapPosition = transform.GetChild(0);
        rightGrabReference.action.canceled += OnPlayerRelease;
        leftGrabReference.action.canceled += OnPlayerRelease;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        // If the object is not a rigidbody, ignore it
        if (other.attachedRigidbody == null)
            return;
        // If there is already an object in the trigger, ignore the new object
        if (collidingObject != null)
            return;
        // Add the object to the variable
        collidingObject = other.attachedRigidbody;
    }
    
    // onPlayerRelease is called when player release the object
    
    public void OnPlayerRelease(InputAction.CallbackContext ctx)
    {
        // If there is no object in the trigger, ignore
        if (collidingObject == null)
            return;
        // Set the object's position to the snap position
        collidingObject.transform.position = snapPosition.position;
    }
    
    private void OnTriggerExit(Collider other)
    {
        // If the object is not a rigidbody, ignore it
        if (other.attachedRigidbody == null)
            return;
        // If the object is not the same as the one in the variable, ignore it
        if (collidingObject != other.attachedRigidbody)
            return;
        // Remove the object from the variable
        collidingObject = null;
    }
    
    

}
