using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherePositionUpdate : MonoBehaviour
{
    // This script is attached to the grid and holds both mass spheres. It updates the grid shader with their positions.
    public GameObject ysphere, bsphere;
    Vector4 pos;

    // Update is called once per frame
    void Update()
    {
        // Important note: Unity might be implementing zombie shaders here, i.e. copying shaders for parameter changes instead of recomputing.
        // This can cause severe performance issues, so a methodology change might be necessary if we see significant lag or power usage.

        Vector4 avg = (ysphere.transform.position + bsphere.transform.position) / 2;

        if (pos != avg) {
            pos = avg;
            gameObject.GetComponent<Renderer>().sharedMaterial.SetVector("_CenterPoint", pos);
        }
    }
}
