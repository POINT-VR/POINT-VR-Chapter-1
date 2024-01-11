using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour //This script is built so that an endpoint can be reused and be fully controlled by another script
{
    /// <summary>
    /// Mass which snaps to the endpoint
    /// </summary>
    private GameObject MassSphere;

    private bool isActive = true;

    /// <summary>
    /// Remains true after the endpoint is triggered and until the endpoint is reset by another script
    /// </summary>
    private bool triggered = false;

    /// <summary>
    /// The maximum distance an object can be from the endpoint for it to trigger.
    /// </summary>
    private float triggerDistance;

    void Update()
    {
        if (isActive) //Checks trigger each frame
        {
            CheckTrigger();
        }
    }
    private void CheckTrigger() //Checks if the mass sphere is within the is within the snap distance, then deactivates the endpoint
    {
        if ((MassSphere.transform.position - transform.position).magnitude < triggerDistance && !MassSphere.GetComponentInParent<HandController>()) //Check that the sphere is not being grabbed (should be HandControllerEmulator for testing in emulator)
        {
            MassSphere.transform.position = transform.position;
            MassSphere.transform.SetParent(null);
            triggered = true;
            Deactivate();
        }
    }

    //Public Member Function

    /// <summary>
    /// Shows and activates each the endpoint
    /// </summary>
    public void Activate()
    {
        isActive = true;
        triggered = false;
        GetComponent<MeshRenderer>().enabled = true;
    }

    /// <summary>
    /// Hides and deactivates each the endpoint
    /// </summary>
    public void Deactivate()
    {
        isActive = false;
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void SetTriggerDistance(float distance)
    {
        triggerDistance = distance;
    }
    public void SetMass(GameObject obj) //Sets the mass object
    {
        MassSphere = obj;
    }

    //Allows other scripts to tell if the endpoint was triggered
    public bool WasTriggered() 
    {
        return triggered;
    }

    public void ResetTrigger()
    { 
        triggered = false;
    }

}
