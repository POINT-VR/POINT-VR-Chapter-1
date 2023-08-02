using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObjectives : MonoBehaviour
{
    private Camera cameraObject;
    // Start is called before the first frame update
    void Start()
    {
        cameraObject = Camera.allCameras[0];
        this.transform.SetParent(cameraObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
