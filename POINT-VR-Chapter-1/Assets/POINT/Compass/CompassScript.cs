using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CompassScript : MonoBehaviour
{
    /// <summary>
    /// Stores the camera which the compass objectives attaches itself too
    /// </summary>
    private Camera cameraObject;

    /// <summary>
    /// Stores all angles
    /// </summary>
    private List<double> angles = new List<double>();

    /// <summary>
    /// Stores all ticks
    /// </summary>
    private List<RectTransform> ticks = new List<RectTransform>();

    /// <summary>
    /// /// Stores all objects
    /// </summary>
    private List<GameObject> gameObjects = new List<GameObject>();

    /// <summary>
    /// Stores tags for objects
    /// </summary>
    private List<string> gameLabels = new List<string>();

    /// <summary>
    /// Tick Prefab
    /// </summary>
    [SerializeField]
    private GameObject tick;

    // Start is called before the first frame update
    void Start()
    {
        cameraObject = Camera.allCameras[0];
        transform.SetParent(cameraObject.transform);
        transform.localPosition = new Vector3(0, 0, 3);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAngles();
        UpdateTicks();
    }

    private void UpdateAngles()
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
            }
            else if (difference < -180)
            {
                difference += 360;
            }
            angles[i] = difference;
            i++;
        }
    }

    private void UpdateTicks()
    {
        int i = 0;
        foreach (var Tick in ticks)
        {
            Tick.anchoredPosition3D = new Vector3((float)angles[i] / 72, 0, 0); //10/9 takes 360 degrees to 400 pixels
            i++;
        }
    }

    private void CreateTick()
    {
        var Tick = Instantiate(tick, this.transform);
        Tick.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        ticks.Add(Tick.GetComponent<RectTransform>());
    }

    public void AddCompassObject(GameObject gameObject, string label = "")
    {
        gameObjects.Add(gameObject);
        gameLabels.Add(label);
        CreateTick();
        angles.Add(0.0);
    }

    public void RemoveCompassObject(GameObject gameObject)
    {
        int i = 0;
        foreach (GameObject obj in gameObjects)
        {
            if (obj == gameObject)
            {
                gameObjects.Remove(obj);
                gameLabels.RemoveRange(i, 1);
                angles.RemoveRange(i, 1);
                GameObject oldtick = ticks[i].gameObject;
                ticks.RemoveRange(i, 1);
                Destroy(oldtick);
                break;
            }
            i++;
        }
    }
}