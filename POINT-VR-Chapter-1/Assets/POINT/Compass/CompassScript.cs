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

    [SerializeField]
    private Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        cameraObject = Camera.allCameras[0];
        this.transform.SetParent(cameraObject.transform);
        this.transform.localPosition = position;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject gameObject in gameObjects) 
        {
            Vector3 direction = gameObject.transform.position - cameraObject.transform.position;
            double theta = Math.Atan2(direction.x, direction.z)*180/Math.PI;
            transform.eulerAngles = new Vector3(0, (float)theta, 0);
        }
    }
}
