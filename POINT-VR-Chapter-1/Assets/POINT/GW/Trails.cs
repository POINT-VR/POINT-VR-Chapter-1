using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trails : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] tps;
    [SerializeField]
    public float time = 1.0f;
    private bool is_empty = true;

    [SerializeField]
    private float startWidth = 0.5f;
    [SerializeField]
    private float endWidth = 0.25f;

    private Color startColor = Color.white;
    private Color endColor = Color.black;
    void Update()
    {
        if (is_empty)
        {
            tps = GameObject.FindGameObjectsWithTag("tp");
            AddTrails();
            if (tps.Length != 0)
            {
                is_empty = false;
            }
        }
        //print(tps.Length);
        foreach(GameObject p in tps)
        {
            
            TrailRenderer tr = p.GetComponent<TrailRenderer>();
            tr.time = time;
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
    
}