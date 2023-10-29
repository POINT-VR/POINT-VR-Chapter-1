using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

//Displays spacial and an optional time coordinate above an object. Public member functions allow for the 4d origin to be manipulated and the text to be shown and hid from other scripts. 
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
        coordinateText.transform.position = transform.position + dummyCamera.up / 2;

        coordinateText.transform.LookAt(2 * coordinateText.transform.position - dummyCamera.transform.position); //makes the text angle to face the camera.
        Destroy(dummyCamera.gameObject);
    }

    private void updateText()
    {
        Vector3 outputCoordinates = transform.position - (Vector3)origin; //calculates position relative to origin
        if (showTime)
        {
            coordinateText.text = $"<color=grey>( <color=blue>{Math.Floor(10 * outputCoordinates.x) / 10}</color> , <color=green>{Math.Floor(10 * outputCoordinates.y) / 10}</color> , <color=red>{Math.Floor(10 * outputCoordinates.z) / 10}</color> , {Math.Floor(Time.time - tNaught)} )</color>"; //calculates time relative to origin
        }
        else
        {
            coordinateText.text = $"<color=grey>( <color=blue>{Math.Floor(10 * outputCoordinates.x) / 10}</color> , <color=green>{Math.Floor(10 * outputCoordinates.y) / 10}</color> , <color=red>{Math.Floor(10 * outputCoordinates.z) / 10}</color> )</color>";
        }
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
        transform.parent.gameObject.SetActive(false);
    }
    public void HideMass()
    {
        transform.parent.gameObject.SetActive(true);
    }
    
}
