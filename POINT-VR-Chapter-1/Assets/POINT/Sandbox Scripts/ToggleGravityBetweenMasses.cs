using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGravityBetweenMasses : MonoBehaviour
{
    [SerializeField] private List<GameObject> masses;
    private List<GravityScript> gravityScripts = new List<GravityScript>(3);


    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject mass in masses)
        {
            GravityScript gs = mass.GetComponent<GravityScript>();
            gravityScripts.Add(gs);
            gs.enabled = false;
        }
    }

    public void GravityOn()
    {
        foreach (GravityScript item in gravityScripts)
        {
            item.enabled = true;
        }
    }

    public void GravityOff()
    {
        foreach (GravityScript item in gravityScripts)
        {
            item.enabled = false;
        }
    }
}
