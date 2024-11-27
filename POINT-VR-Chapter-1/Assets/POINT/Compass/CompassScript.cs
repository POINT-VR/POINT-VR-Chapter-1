using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CompassScript : MonoBehaviour
{
    /// <summary>
    /// Stores the camera which the floating objectives attaches itself too
    /// </summary>
    private Camera cameraObject;

    /// <summary>
    /// Stores tags, gameobjects, and angles
    /// </summary>
    private List<double> angles = new List<double>();

    /// <summary>
    /// Stores all ticks
    /// </summary>
    private List<RectTransform> ticks = new List<RectTransform>();

    [SerializeField]
    private Vector3 position;

    /// <summary>
    /// Initial list which takes gameobjects to be attached
    /// </summary>
    [SerializeField]
    private List<GameObject> gameObjects;

    /// <summary>
    /// Initial list which takes tages to be attached
    /// </summary>
    [SerializeField]
    private List<string> gameLabels;

    /// <summary>
    /// Tick Prefab
    /// </summary>
    [SerializeField]
    private GameObject tick;

    // Start is called before the first frame update
    void Start()
    {
        cameraObject = Camera.allCameras[0];
        this.transform.SetParent(cameraObject.transform);
        this.transform.localPosition = position;
        foreach (var obj in gameObjects)
        {
            angles.Add(0.0);
            CreateTick();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        UpdateTicks();
    }

    void UpdatePosition()
    {
        int i = 0;
        foreach (var obj in gameObjects)
        {
            Vector3 direction = obj.transform.position - cameraObject.transform.position;
            Vector3 orientation = cameraObject.transform.forward;
            double theta1 = Math.Atan2(direction.x, direction.z) * 180 / Math.PI;
            double theta2 = Math.Atan2(orientation.x, orientation.z) * 180 / Math.PI;
            double difference = theta1 - theta2;
            if (difference > 180)
            {
                difference -= 360;
            } else if (difference < -180)
            {
                difference += 360;
            }
            angles[i] = (difference);
            i++;
        }
    }

    void UpdateTicks()
    {
        int i = 0;
        foreach (var Tick in ticks)
        {
            Tick.anchoredPosition3D = new Vector3((float)angles[i]*10/9, 500, 0);
            i++;
        }
    }

    void CreateTick()
    {
        var Tick = Instantiate(tick,this.transform);
        Tick.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 500, 0);
        ticks.Add(Tick.GetComponent<RectTransform>());
    }
}
