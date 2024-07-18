using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesiredCoordinates : MonoBehaviour
{
    /// <summary>
    /// Stores the camera which the coordinates attaches itself too
    /// </summary>
    private Camera cameraObject;
    // Start is called before the first frame update
    void Start()
    {
        cameraObject = Camera.allCameras[0]; 
    }

    // Update is called once per frame
    void Update()
    {
       this.transform.LookAt(cameraObject.transform); 
       transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
