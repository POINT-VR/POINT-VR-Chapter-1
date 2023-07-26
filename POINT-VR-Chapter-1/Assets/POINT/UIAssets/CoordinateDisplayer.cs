using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CoordinateDisplayer : MonoBehaviour
{
    /// <summary>
    /// Decides whether or not to include the 4th time coordinate. 
    /// </summary>
    private bool showTime;

    /// <summary>
    /// Stores the origin for time, t=0.
    /// </summary>
    private float startTime;

    /// <summary>
    /// Stores refrence to TMP object which will display the coordinates.
    /// </summary>
    [SerializeField]
    TMP_Text coordinateText;

    /// <summary>
    /// The origin spacial coordinates.
    /// </summary>
    [SerializeField]
    Vector3 origin;

    /// <summary>
    /// Decides whether or not time is enabled upon start
    /// </summary>
    [SerializeField]
    bool showTimeOnStart;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        showTime = showTimeOnStart;
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
    //Public member functions to be accessed by other scripts
    public void ShowTime()
    {
        showTime = true;
    }
    public void HideTime()
    {
        showTime = false;
    }
    public void SetTime(float offset) //sets the time coordinate to any value
    {
        startTime = Time.time - offset; //if we want t=x in the present then we make the startTime x seconds before the present
    }
}
