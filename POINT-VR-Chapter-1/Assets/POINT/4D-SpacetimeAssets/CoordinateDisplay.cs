﻿using UnityEngine;
using TMPro;
using System;

//Displays spacial and an optional time coordinate above an object. This script is used at every point where we wish to access the mass object.
public class CoordinateDisplay : MonoBehaviour
{
    /// <summary>
    /// Decides whether or not to include the 4th time coordinate. 
    /// </summary>
    private bool showTime = true;

    /// <summary>
    /// Determines whether or not the text is being displayed.
    /// </summary>
    private bool showText = true;

    /// <summary>
    /// The origin for time, t=0.
    /// </summary>
    private float tNaught;

    /// <summary>
    /// Camera object which controls where the text is facing.
    /// </summary>
    private Camera cameraObject;

    /// <summary>
    /// Stores refrence to TMP object which will display the coordinates.
    /// </summary>
    [SerializeField]
    TMP_Text coordinateText;

    /// <summary>
    /// The origin for spacial coordinates.
    /// </summary>
    [SerializeField]
    Vector3 origin;

    void Start()
    {
        cameraObject = Camera.allCameras[0]; //sets player camera so that position can be calculated correctly
        tNaught = Time.time; //sets time origin to start time
        coordinateText.outlineWidth = 0.1f;
        coordinateText.outlineColor = new Color32(255, 220, 220, 220); //light grey

    }

    void Update()
    {
        if (!showText)
        {
            coordinateText.text = "";
            return; // no need to calculate position or update text when it is not showing
        }
        updatePosition();
        updateText();
    }


    //private update functions
    private void updatePosition()
    {
        Transform dummyCamera = new GameObject().transform; //uses a dummy transform to position the text 'above' the mass from the point of view of the camera
        dummyCamera.position = cameraObject.transform.position;
        dummyCamera.LookAt(transform.position);
        Vector3 shellVector = cameraObject.transform.position - coordinateText.transform.position; // vector pointing to camera from location of text
        shellVector.y = 0; // setting the y component equal to 0 so only the other 2 components get normalized
        Vector3 shellNorm = 0.3f * (shellVector).normalized;
        coordinateText.transform.position = transform.position + shellNorm + new Vector3(0,0.5f,0);

        coordinateText.transform.LookAt(2 * coordinateText.transform.position - dummyCamera.transform.position); //makes the text angle to face the camera.
        Destroy(dummyCamera.gameObject);
    }

    private void updateText()
    {
        Vector3 outputCoordinates = transform.position - (Vector3)origin; //calculates position relative to origin
        if (showTime)
        {
            coordinateText.text = $"<color=white>( <color=red>{(Math.Floor(10 * outputCoordinates.x + 0.01) / 10)}</color> , <color=blue>{(Math.Floor(10 * outputCoordinates.z + 0.01) / 10)}</color> , <color=green>{(Math.Floor(10 * outputCoordinates.y + 0.01) / 10)}</color> , {Math.Floor(Time.time - tNaught)} )</color>"; //calculates time relative to origin, +0.01 accounts for a rounding error
        }
        else
        {
            coordinateText.text = $"<color=white>( <color=red>{(Math.Floor(10 * outputCoordinates.x + 0.01) / 10)}</color> , <color=blue>{(Math.Floor(10 * outputCoordinates.z + 0.01) / 10)}</color> , <color=green>{(Math.Floor(10 * outputCoordinates.y + 0.01) / 10)}</color> )</color>";
        }
        RectTransform rt = coordinateText.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        Bounds textBounds = coordinateText.mesh.bounds;
        rt.sizeDelta = new Vector2(textBounds.max.x - textBounds.min.x + 40, 130);
    }


    //Public member functions to be accessed by other scripts
    public void SetTime(float t) //sets the time coordinate to any value
    {
        tNaught = Time.time - t; //if we want time=t in the present then we make the origin t0 seconds before the present
    }
    public void SetOrigin(Vector3 position)
    {
        origin = position;
    }

    public void ShowTime()
    {
        showTime = true;
    }
    public void HideTime()
    {
        showTime = false;
    }
    public void ShowText()
    {
        showText = true;
    }
    public void HideText()
    {
        showText = false;
    }
    public void ShowMass()
    {
        transform.gameObject.SetActive(true);
    }
    public void HideMass()
    {
        transform.gameObject.SetActive(false);
    }
    
}
