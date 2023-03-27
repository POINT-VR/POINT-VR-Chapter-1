using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisMaster : MonoBehaviour
{
    /**
     * Connect AxisMaster to the 3Daxis component
     * THIS SCRIPT CONTROLS THE MOVEMENT OF THE AXES
     * Change scaleFinal to the desired values (Vector3, not below 0), and use method scaleAxes to change the axes scale
    **/


    public Vector3 scale = new Vector3(1,1,1);
    private AxisScript axisScript;
    private Vector3 scaleInitial;
    public Vector3 scaleFinal;
    private float t = 0.0f;

    public float speed = 0.05f;
    private float duration = 10f;
    // Start is called before the first frame update
    void Start()
    {
        //Initializes values
        axisScript = GetComponent<AxisScript>();
        scaleInitial = axisScript.scale;
        
    }

    public void scaleAxes()
   {
    //resets time constant, setting the loop up again
    //    scaleFinal = scale;
        t = 0.0f;
   }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Loop lerps scale from initial value from AxisScript to scaleFinal
        if (t <= 1.0f)
        {
            axisScript.scale = new Vector3(Mathf.Lerp(scaleInitial.x, scaleFinal.x, t / duration), Mathf.Lerp(scaleInitial.y, scaleFinal.y, t / duration),
                Mathf.Lerp(scaleInitial.z, scaleFinal.z, t / duration));
            t = t + speed;
        }
    }
}
