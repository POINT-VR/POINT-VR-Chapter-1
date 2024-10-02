using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;


public class SortHolder : MonoBehaviour
{
    public GameObject snapRingPrefab;
    [SerializeField] private Sprite correctSprite;
    [SerializeField] private Sprite wrongSprite;
    [SerializeField] private int snapRingCount = 5;
    [SerializeField] private float padding = 0.15f;
    [SerializeField] private GameObject NextTaskButton;

    private GameObject[] snapRings = null;
    private Camera currentCamera = null;
    private GameObject player = null;

    public void Start()
    {
        StartCoroutine(WaitForPlayerSpawn());

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

    IEnumerator WaitForPlayerSpawn()
    {
        yield return new WaitUntil(() => Camera.current != null);

        // Start menu initialization
        currentCamera = Camera.current;
        //this.GetComponent<Canvas>().worldCamera = currentCamera;
        player = currentCamera.transform.parent.gameObject;
        
        yield break;
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
            snapRings[i].GetComponentInChildren<Image>().enabled = true;
            if (masses[i] != snapRings[i].GetComponentInChildren<Rigidbody>().mass)
            {
                isCorrectOrder = false;
                snapRings[i].GetComponent<MeshRenderer>().material.color = Color.red;
                snapRings[i].GetComponentInChildren<Image>().sprite = wrongSprite;
                snapRings[i].GetComponentInChildren<Image>().color = Color.red;
            }
            else
            {
                snapRings[i].GetComponent<MeshRenderer>().material.color = Color.green;
                snapRings[i].GetComponentInChildren<Image>().sprite = correctSprite;
                snapRings[i].GetComponentInChildren<Image>().color = Color.green;
            }
        }

        return isCorrectOrder;
    }

    public void TestSortOrder()
    {
        if (CheckSortOrder())
        {
            // Nice Job
            StartCoroutine(MassesCorrect());
        }

        else
        {
            // Try Again
            StartCoroutine(MassesIncorrect());
        }
    }

    public IEnumerator MassesCorrect()
    {
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\6_put_masses_in_order_2");
        yield return new WaitForSecondsRealtime(6);
        NextTaskButton.SetActive(true);
        yield break;
    }

    public IEnumerator MassesIncorrect()
    {
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\6_put_masses_in_order_3");
        yield return new WaitForSecondsRealtime(2);
        yield break;
    }
}