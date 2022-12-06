using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    // test on trigger enter and exit
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter");
    }
    
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exit");
    }
}
