using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CoordinateDisplayer : MonoBehaviour
{
    private bool showTime = false;

    private float startTime;

    [SerializeField]
    TMP_Text coordinateText;

    Vector3 origin;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 outputCoordinates = transform.position - origin;
        if (showTime)
        {
            coordinateText.text = $"({outputCoordinates.x}, {outputCoordinates.y}, {outputCoordinates.z}, {Math.Floor(Time.time - startTime)})";
        }
        else
        {
            coordinateText.text = $"({outputCoordinates.x}, {outputCoordinates.y}, {outputCoordinates.z})";
        }
    }
    public void ShowTime()
    {
        showTime = true;
    }
    public void HideTime()
    {
        showTime = false;
    }

    public void SetTime(float offset) 
    {
        startTime = Time.time - offset; //if we want t=x in the present then we make the startTime x seconds before the present
    }
}
