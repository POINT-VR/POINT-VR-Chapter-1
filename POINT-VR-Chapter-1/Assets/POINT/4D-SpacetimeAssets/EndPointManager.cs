using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

/// <summary>
/// Accessed by Scene1Manager to spawn a set of endpoints arround a mass, allowing for snap motion along a lattice grid
/// </summary>
public class EndPointManager : MonoBehaviour
{

    [SerializeField]
    private EndPoint endPointPrefab;

    [SerializeField]
    private DynamicAxis dynamicAxisPrefab;

    /// <summary>
    /// The mass object which snaps to endpoints
    /// </summary>
    private GameObject massObject;

    /// <summary>
    /// Contains all the EndPoints around the mass.
    /// </summary>
    private List<EndPoint> endPoints = new List<EndPoint>();

    private List<Vector3> endPath = new List<Vector3>();
    private List<Vector3> comparePoints = new List<Vector3>();
    /// <summary>
    /// Saves the refrence to the axis object.
    /// </summary>
    private DynamicAxis dynamicAxis;

    /// <summary>
    /// The distance between any two adjacent endpoints.
    /// </summary>
    private float dist = 1.0f;

    private bool isActive = false;

    private bool pathDone = false;

    void Update() //checks if any of the endpoints are triggered and respawns them in the new location
    {
        if (!isActive)
        {
            return;
        }
        bool triggered = false;
        bool samePath = false;
        foreach (var endPoint in endPoints)
        {
            if(endPoint.WasTriggered())
            {
                ///
                var pos = massObject.transform.position;
                if (pos != new Vector3(0,0,0))
                {
                    endPath.Add(pos);
                }
                if (comparePoints.Count >= 1 && endPath.Count >= 1) 
                {
                    samePath = true;
                    for (int i = 0; i < endPath.Count && i < comparePoints.Count; i++)
                    {
                        Debug.Log(endPath[i]);
                        Debug.Log(comparePoints[i]);
                        if (endPath[i] != comparePoints[i])
                        {
                            samePath = false;
                            break;
                        }
                    }
                }
                Debug.Log(samePath);
                if (samePath == false) 
                {
                    triggered = true;
                    endPoint.ResetTrigger();
                    break;
                }
                endPath.RemoveAt(endPath.Count - 1); 
                ///
            }
        }
        if (triggered)
        {
            Destroy();
            Spawn();
        } else if (samePath == true) {
            if (endPath.Count > 0) 
            {
                massObject.transform.position = endPath[endPath.Count - 1];
            } else {
                massObject.transform.position = new Vector3(0,0,0);
            }
        }
    }
    void Spawn()
    {
        var pos = massObject.transform.position;
        // Checking if the target location has been reached (3, 1, 2)
        if (pos == new Vector3(3,1,2) * dist)
        {
            pathDone = true;
            Deactivate();
            return;
        }
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
                newEndPoint.Activate();
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

    //Public member functions
    public void Activate()
    {
        endPath = new List<Vector3>();
        Spawn();
        isActive = true;
    }

    public void Deactivate()
    {
        Destroy();
        isActive = false;
    }

    public void SetMass(GameObject obj)
    {
        Destroy();
        massObject = obj;
    }

    public bool Status()
    {
        return isActive;
    }

    public List<Vector3> GetPath() {
        return endPath;
    }

    public bool PathStatus()
    {
        return pathDone;
    }

    public void setComparisonPath(List<Vector3> l) 
    {
        comparePoints = l;
        Debug.Log(comparePoints.Count);
    }
}
