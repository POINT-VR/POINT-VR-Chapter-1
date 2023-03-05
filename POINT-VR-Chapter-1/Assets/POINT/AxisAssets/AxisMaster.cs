using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisMaster : MonoBehaviour
{

    public Vector3 scale = new Vector3(1,1,1);
    private AxisScript axisScript;
    private Vector3 scaleInitial;
    public Vector3 scaleFinal = new Vector3(4.0f, 2.0f, 9.0f);
    private float t = 0.0f;

    public float speed = 0.05f;
    private float duration = 10f;
    // Start is called before the first frame update
    void Start()
    {
        axisScript = GetComponent<AxisScript>();
        scaleInitial = axisScript.scale;
        
    }

   // public void scaleAxes(Vector3 scale)
   // {
    //    scaleFinal = scale;
    //    t = 0.0f;
   // }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (t <= 1.0f)
        {
            axisScript.scale = new Vector3(Mathf.Lerp(scaleInitial.x, scaleFinal.x, t / duration), Mathf.Lerp(scaleInitial.y, scaleFinal.y, t / duration),
                Mathf.Lerp(scaleInitial.z, scaleFinal.z, t / duration));
            t = t + speed;
        }
    }
}
