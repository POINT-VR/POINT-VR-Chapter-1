using System;
using UnityEngine;


public class SortHolder : MonoBehaviour
{
    public GameObject snapRingPrefab;
    [SerializeField] private int snapRingCount = 5;
    [SerializeField] private float padding = 0.15f;
    [SerializeField] private XRHardwareController hardwareController;
    [SerializeField] private GameObject NextTaskButton;

    private GameObject[] snapRings = null;

    public void Start()
    {
        snapRings = new GameObject[snapRingCount];
        float snapRingX = snapRingPrefab.GetComponent<BoxCollider>().size.x;

        // Instantiate required number of snap rings
        for (int i = 0; i < snapRingCount; ++i)
        {
            GameObject snapRing = Instantiate(snapRingPrefab, this.transform);
            snapRing.transform.localPosition += new Vector3(i * (snapRingX + padding), 0, 0);
            snapRings[i] = snapRing;
        }
    }
 
    public bool CheckSortOrder()
    {
        float[] masses = new float[snapRingCount];
            
        // Store masses
        for (int i = 0; i < snapRingCount; ++i)
        {
            if (snapRings[i].GetComponentInChildren<Rigidbody>() == null) return false;
            masses[i] = snapRings[i].GetComponentInChildren<Rigidbody>().mass;
        }

        // Sort masses; masses now holds the "solution"
        Array.Sort(masses);
        bool isCorrectOrder = true;
        for (int i = 0; i < snapRingCount; ++i)
        {
            if (masses[i] != snapRings[i].GetComponentInChildren<Rigidbody>().mass)
            {
                isCorrectOrder = false;
                snapRings[i].GetComponent<MeshRenderer>().material.color = Color.red;
            }
            else
            {
                snapRings[i].GetComponent<MeshRenderer>().material.color = Color.green;
            }
        }

        return isCorrectOrder;
    }

    public void TestSortOrder()
    {
        if (CheckSortOrder())
        {
            NextTaskButton.SetActive(true);
        }
    }
}