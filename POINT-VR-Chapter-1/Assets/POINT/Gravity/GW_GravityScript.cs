using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GW_GravityScript : MonoBehaviour
{

    private float amplitude;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 MovePlusMode(GameObject mesh, Vector3 pos, float angle)
    {

        Vector3 unitVector;

        unitVector = Quaternion.AngleAxis(angle, Vector3.up) * pos;

        Vector3 output;

        output.x = pos.x + unitVector.x * amplitude * Mathf.Cos(Time.deltaTime * speed);
        output.y = pos.y - unitVector.y * amplitude * Mathf.Cos(Time.deltaTime * speed);
        output.z = pos.z;

        return output;
    }

    Vector3 MoveCrossMode(GameObject mesh, Vector3 pos, float angle)
    {

        Vector3 unitVector;

        unitVector = Quaternion.AngleAxis(angle, Vector3.up) * pos;

        Vector3 output;

        output.x = pos.x + unitVector.x * amplitude * Mathf.Sin(Time.deltaTime * speed);
        output.y = pos.y - unitVector.y * amplitude * Mathf.Sin(Time.deltaTime * speed);
        output.z = pos.z;

        return output;
    }
}
