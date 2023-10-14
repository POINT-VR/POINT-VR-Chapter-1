using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EventSystems;
using UnityEngine;

public class EndPoint : MonoBehaviour //This script is built so that an endpoint can be reused and be fully controlled by another script
{
    private GameObject MassSphere;
    private bool isActive = true;
    private bool triggered = false;
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
        if ((MassSphere.transform.position - transform.position).magnitude < triggerDistance)
        {
            triggered = true;
            Deactivate();
        }
    }

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

    public bool WasTriggered() //Allows other scripts to tell if the endpoint was triggered
    {
        return triggered;
    }
}
