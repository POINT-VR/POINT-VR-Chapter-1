using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private GameObject MassSphere;
    private bool isActive = true;
    private float snapDistance;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            SnapPosition();
        }
    }

    private void SnapPosition()
    {
        if ((MassSphere.transform.position - transform.position).magnitude < 1)
        {
            MassSphere.transform.position = transform.position;
        }
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }

    public void setSnapDistance(float distance)
    {
        snapDistance = distance;
    }
    public void setMass(GameObject obj)
    {
        MassSphere = obj;
    }
}
