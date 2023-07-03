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
        /**
         * Motion of a particle based on Plus Mode polarization of a gravitational wave
         * Works on the principle of x = x_0 + (direction of normal to the mesh) * cos(omega * t)
         * And y = y_0 - (direction of normal to the mesh) * cos(omega * t)
         * (Gives out both x and y oscillations, given propagation on z axis)
         * **/

        //Create normal vector of mesh; mesh is translated along this direction
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
        /**
         * Motion of a particle based on Cross Mode polarization of a gravitational wave
         * Works on the principle of x = x_0 - (direction of tangent to the mesh) * sin(omega * t)
         * And y = y_0 + (direction of tangent to the mesh) * sin(omega * t)
         * (Gives out both x and y oscillations, given propagation on z axis)
         * **/

        //Create tangent vector of mesh; mesh is translated along this direction
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

    public Vector3 MovePlusMode(Vector3 pos, Vector3 center, float angle, float phase)
    {
        /**
         * -----------Additional Method of the Function with Phase of the Oscillations-----------
         * Motion of a particle based on Plus Mode polarization of a gravitational wave
         * Works on the principle of x = x_0 + (direction of normal to the mesh) * cos(omega * t + phi)
         * And y = y_0 - (direction of normal to the mesh) * cos(omega * t + phi)
         * (Gives out both x and y oscillations, given propagation on z axis)
         * **/

        //Create normal vector of mesh; mesh is translated along this direction
        Vector3 unitVector = (pos - center).normalized;



        Vector3 output;
        output.x = pos.x
            + unitVector.x * amplitude * Mathf.Cos(Time.time * speed * Mathf.Deg2Rad / Mathf.PI + phase);
        output.y = pos.y
            - unitVector.y * amplitude * Mathf.Cos(Time.time * speed * Mathf.Deg2Rad / Mathf.PI + phase);
        output.z = pos.z;

        return output;
    }

    public Vector3 MoveCrossMode(Vector3 pos, Vector3 center, float angle, float phase)
    {
        /**
         *  -----------Additional Method of the Function with Phase of the Oscillations-----------
         * Motion of a particle based on Cross Mode polarization of a gravitational wave
         * Works on the principle of x = x_0 - (direction of tangent to the mesh) * sin(omega * t + phi)
         * And y = y_0 + (direction of tangent to the mesh) * sin(omega * t + phi)
         * (Gives out both x and y oscillations, given propagation on z axis)
         * **/

        //Create tangent vector of mesh; mesh is translated along this direction
        Vector3 unitVectory = (pos - center).normalized;
        Vector3 unitVector = new Vector3(-unitVectory.y, unitVectory.x, 0);


        Vector3 output;
        output.x = pos.x
        - unitVector.x * amplitude * Mathf.Sin(Time.time * speed * Mathf.Deg2Rad / Mathf.PI + phase);
        output.y = pos.y
            + unitVector.y * amplitude * Mathf.Sin(Time.time * speed * Mathf.Deg2Rad / Mathf.PI + phase);
        output.z = pos.z;

        return output;
    }
}
