using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CompassScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> gameObjects;

    /// <summary>
    /// Stores the camera which the floating objectives attaches itself too
    /// </summary>
    private Camera cameraObject;

    private List<double> angles = new List<double>();

    [SerializeField]
    private Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        cameraObject = Camera.allCameras[0];
        this.transform.SetParent(cameraObject.transform);
        this.transform.localPosition = position;
        foreach (GameObject gameObject in gameObjects)
            angles.Add(0);
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        foreach (GameObject gameObject in gameObjects) 
        {
            Vector3 direction = gameObject.transform.position - cameraObject.transform.position;
            Vector3 orientation = cameraObject.transform.forward;
            double theta1 = Math.Atan2(direction.x, direction.z)*180/Math.PI;
            double theta2 = Math.Atan2(orientation.x, orientation.z)*180/Math.PI;
            angles[i] = theta1-theta2;
            i++;
        }
        Debug.Log(angles[0]);
    }
}
