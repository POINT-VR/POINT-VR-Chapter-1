using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GW_GravityScript : MonoBehaviour
{

   [SerializeField] private float amplitude;
   [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 MovePlusMode(Vector3 pos, Vector3 center, float angle)
    {

        Vector3 unitVector = (pos - center).normalized;



        Vector3 output;
        output.x = pos.x
            + unitVector.x * amplitude * Mathf.Cos(Time.time * speed * Mathf.Deg2Rad / Mathf.PI);
        output.y = pos.y
            - unitVector.y * amplitude * Mathf.Cos(Time.time * speed * Mathf.Deg2Rad / Mathf.PI);
        output.z = pos.z;

        return output;
    }

    public Vector3 MoveCrossMode(Vector3 pos, Vector3 center, float angle)
    {

        Vector3 unitVectory = (pos-center).normalized;
        Vector3 unitVector = new Vector3(-unitVectory.y, unitVectory.x, 0);


        Vector3 output;
        output.x = pos.x
        - unitVector.x * amplitude * Mathf.Sin(Time.time * speed * Mathf.Deg2Rad / Mathf.PI);
        output.y = pos.y
            + unitVector.y * amplitude * Mathf.Sin(Time.time * speed * Mathf.Deg2Rad / Mathf.PI);
        output.z = pos.z;

        return output;
    }
}
