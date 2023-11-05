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
        if ((MassSphere.transform.position - transform.position).magnitude < triggerDistance && !MassSphere.GetComponentInParent<HandController>()) //Need to make it check that the sphere is not being grabbed
        {
            MassSphere.transform.position = transform.position;
            triggered = true;
            Deactivate();
        }
    }

    //Public Member Function
    public void Activate()
    {
        isActive = true;
        triggered = false;
        GetComponent<MeshRenderer>().enabled = true;
    }

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

    //Other scripts can reset the endpoint when the information that it was triggered is no longer necessary 
    public void Reset()
    { 
        triggered = false;
    }

}
