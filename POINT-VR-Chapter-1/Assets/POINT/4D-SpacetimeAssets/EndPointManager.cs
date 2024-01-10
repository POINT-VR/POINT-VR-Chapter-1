using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointManager : MonoBehaviour
{

    [SerializeField]
    private EndPoint endPointPrefab;

    [SerializeField]
    private DynamicAxis dynamicAxisPrefab;

    private GameObject massObject;

    /// <summary>
    /// Contains all the EndPoints around the mass.
    /// </summary>
    private List<EndPoint> endPoints = new List<EndPoint>();

    private DynamicAxis dynamicAxis;

    /// <summary>
    /// The distance between any two adjacent endpoints.
    /// </summary>
    private float dist = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        bool triggered = false;
        foreach (var endPoint in endPoints)
        {
            if(endPoint.WasTriggered())
            {
                triggered = true;
            }
        }
        if (triggered)
        {
            Destroy();
            Spawn();
            Activate();
        }
    }
    void Spawn()
    {
        var pos = massObject.transform.position;

        for (int i = 0; i <= 2; i++)
        {
            for (int j = -1; j <= 1; j+=2)
            {
                var newPos = new Vector3();
                if (i == 0)
                {
                    newPos = pos + new Vector3(j, 0, 0) * dist;
                } else if (i == 1)
                {
                    newPos = pos + new Vector3(0, j, 0) * dist;
                } else
                {
                    newPos = pos + new Vector3(0, 0, j) * dist;
                }
                var newEndPoint = Instantiate(endPointPrefab,newPos, Quaternion.identity, this.transform);
                newEndPoint.Deactivate();
                newEndPoint.SetMass(massObject.gameObject);
                newEndPoint.SetTriggerDistance(dist/2); //To prevent a mass object snapping to multiple Endpoints
                endPoints.Add(newEndPoint);
                
            }
        }
        dynamicAxis = Instantiate(dynamicAxisPrefab, pos, Quaternion.identity, this.transform);
        dynamicAxis.AxisLength(0.8f);

    }

    void Destroy()
    {
        foreach (var endPoint in endPoints)
        {
            Destroy(endPoint.gameObject);
        }
        endPoints.Clear();
        if (dynamicAxis != null)
        {
            Destroy(dynamicAxis.gameObject);
        }
    }

    public void Activate()
    {
        foreach (var endPoint in endPoints)
        {
            endPoint.Activate();
        }
    }

    public void SetMass(GameObject obj)
    {
        Destroy();
        massObject = obj;
        Spawn();
    }
}
