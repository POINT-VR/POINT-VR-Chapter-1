using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trails : MonoBehaviour
{
    public GameObject[] tps;
    public GameObject[] x;
    [SerializeField]
    public float time = 1.0f;
    public bool trails_on = false;
    private bool is_empty = true;

    [SerializeField]
    private float startWidth = 0.5f;
    [SerializeField]
    private float endWidth = 0.25f;

    private Color startColor = Color.white;
    private Color endColor = Color.black;

    [SerializeField] private GameObject sphere;
    [SerializeField] private GameObject tube;
    [SerializeField] private Toggle t;

    void Update()
    {
        if (is_empty) // Gets called first frame
        {
            // not the best solution to get the particles of the sphere and 
            // tube in an array but works
            tps = GameObject.FindGameObjectsWithTag("tp"); 
            AddTrails();
            if (tps.Length != 0)
            {
                is_empty = false;
                tube.SetActive(false);  
            }
        }
        //print(tps.Length);
        foreach(GameObject p in tps)
        {
            
            TrailRenderer tr = p.GetComponent<TrailRenderer>();
            tr.time = time;
            tr.enabled = trails_on;
        }
    }

    void AddTrails()
    {
        foreach(GameObject p in tps)
        {
            p.AddComponent<TrailRenderer>();
            TrailRenderer tr = p.GetComponent<TrailRenderer>();
            tr.material = new Material(Shader.Find("Sprites/Default"));
            tr.startColor = startColor;
            tr.endColor = endColor;
            tr.startWidth = startWidth;
            tr.endWidth = endWidth;
        }
    }
    
    // Meant to be used in UI Collider's On Cast for the Trail Toggles
    public void TrailsOnChange()
    {
        trails_on = !trails_on;
        t.isOn = trails_on;
    }
}