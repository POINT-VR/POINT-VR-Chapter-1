using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisScriptTwo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = this.transform.localScale + new Vector3(0, (float)0.05, 0);
        this.transform.localPosition = this.transform.localPosition + (float)0.05 * transform.up;


    }
}
