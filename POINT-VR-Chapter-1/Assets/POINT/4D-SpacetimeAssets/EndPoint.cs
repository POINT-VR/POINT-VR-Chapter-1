using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EventSystems;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private GameObject MassSphere;
    private bool isActive = true;
    private bool triggered = false;
    private float triggerDistance;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            CheckTrigger();
        }
    }

    private void CheckTrigger()
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
    public void SetMass(GameObject obj)
    {
        MassSphere = obj;
    }

    public bool WasTriggered()
    {
        return triggered;
    }
}
