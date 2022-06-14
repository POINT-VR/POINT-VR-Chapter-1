using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Polling : MonoBehaviour
{
    public GameObject console;
    public InputActionReference refe;
    private void FixedUpdate()
    {
        Vector3 v = refe.action.ReadValue<Vector3>();
        ConsoleScript c = console.GetComponent<ConsoleScript>();
        c.Log(v.x + " " + v.y + " " + v.z);
    }
    
}
