using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointManager : MonoBehaviour
{

    [SerializeField]
    private EndPoint prefab;

    private GameObject massObject;

    /// <summary>
    /// Contains all the EndPoints around the mass.
    /// </summary>
    private List<EndPoint> endPoints = new List<EndPoint>();

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
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    if (i == 0 && j == 0 && k == 0)
                    {
                        continue;
                    }
                    var newPos = pos + new Vector3(i, j, k)*dist;
                    var newEndPoint = Instantiate(prefab,newPos, Quaternion.identity, this.transform);
                    newEndPoint.Deactivate();
                    newEndPoint.SetMass(massObject.gameObject);
                    newEndPoint.SetTriggerDistance(dist/2); //To prevent a mass object snapping to multiple Endpoints
                    endPoints.Add(newEndPoint);
                }
            }
        }
    }

    void Destroy()
    {
        foreach (var endPoint in endPoints)
        {
            Destroy(endPoint.gameObject);
        }
        endPoints.Clear();
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
