﻿using System.Collections;
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

    public Vector3 CalculateOscillations(Vector3 pos, Vector3 center, float angle, 
        float PercentOfPlusMode = 0.0f, float PercentOfCrossMode = 0.0f, float PercentOfBreathingMode = 0.0f,
        float PercentOfLongitudinalMode = 0.0f,
        float PercentOfXMode = 0.0f, float PercentOfYMode = 0.0f)
    {
        Vector3 plus = new Vector3(PercentOfPlusMode / 100.0f * MovePlusMode(pos, center, angle).x, PercentOfPlusMode / 100.0f * MovePlusMode(pos, center, angle).y, MovePlusMode(pos, center, angle).z);
        Vector3 cross = new Vector3(PercentOfCrossMode / 100.0f * MoveCrossMode(pos, center, angle).x, PercentOfCrossMode / 100.0f * MoveCrossMode(pos, center, angle).y, MoveCrossMode(pos, center, angle).z);
        Vector3 breathing =  new Vector3(PercentOfBreathingMode / 100.0f * MoveBreathingMode(pos, center, angle).x, PercentOfBreathingMode / 100.0f * MoveBreathingMode(pos, center, angle).y, MoveBreathingMode(pos, center, angle).z);

        return plus + cross + breathing;
    }

    /**
     * FUNCTIONS DESCRIBE MOTIONS FOR
     * EACH POLARIZATION MODE OF GRAVITATIONAL WAVES
     * 
     * 
     * **/



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
        Vector3 unitVector = (pos-center).normalized;
        //Vector3 unitVector = new Vector3(-unitVectory.y, unitVectory.x, 0);


        Vector3 output;
        output.x = pos.x
        + unitVector.y * amplitude * Mathf.Sin(Time.time * speed * Mathf.Deg2Rad / Mathf.PI);
        output.y = pos.y
            + unitVector.x * amplitude * Mathf.Sin(Time.time * speed * Mathf.Deg2Rad / Mathf.PI);
        output.z = pos.z;

        return output;
    }

    public Vector3 MoveBreathingMode(Vector3 pos, Vector3 center, float angle)
    {
        /**
         * Motion of a particle based on Breathing Mode polarization of a gravitational wave
         * Works on the principle of x = x_0 + (direction of normal to the mesh) * cos(omega * t)
         * And y = y_0 - (direction of normal to the mesh) * sin(omega * t)
         * (Gives out both x and y oscillations, given propagation on z axis)
         * **/

        //Create normal vector of mesh; mesh is translated along this direction
        Vector3 unitVector = (pos - center).normalized;



        Vector3 output;
        output.x = pos.x
            + unitVector.x * amplitude * Mathf.Cos(Time.time * speed * Mathf.Deg2Rad / Mathf.PI);
        output.y = pos.y
            + unitVector.y * amplitude * Mathf.Cos(Time.time * speed * Mathf.Deg2Rad / Mathf.PI);
        output.z = pos.z;

        return output;
    }

    /**
     * OVERLOADS OF THE ABOVE 6 FUNCTIONS
     * WITH PHASE DIFFERENCE
     * 
     * **/

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

    float CalculateResponseFunction(int index, float theta, float phi, float psi)
    {
        /**
         * Calculates the Response Function for the polarization mode entered through index
         * 
         * Index:
         * 0:Plus Mode
         * 1:Cross Mode
         * 2:Breathing Mode
         * 3:Longitudinal Mode
         * 4:X Mode
         * 5:Y Mode
         * 
         * **/

        float output;

        switch (index)
        {
            case 0:
                output =  
                    0.5f * (1 + Mathf.Pow(Mathf.Cos(theta), 2.0f) * Mathf.Cos(2.0f * phi) * Mathf.Cos(2.0f * psi))
                    - Mathf.Cos(theta) * Mathf.Sin(2.0f * phi) * Mathf.Sin(2.0f * psi);
                break;

            case 1:
                output =
                    0.5f * (1 + Mathf.Pow(Mathf.Cos(theta), 2.0f) * Mathf.Cos(2.0f * phi) * Mathf.Cos(2.0f * psi))
                    + Mathf.Cos(theta) * Mathf.Sin(2.0f * phi) * Mathf.Sin(2.0f * psi);
                break;

            case 2:
                output = -0.5f * Mathf.Pow(Mathf.Sin(theta), 2.0f) * Mathf.Cos(2 * phi);
                break;

            case 3:
                output = 0.5f * Mathf.Pow(Mathf.Sin(theta), 2.0f) * Mathf.Cos(2 * phi);
                break;

            case 4:
                output =
                    -Mathf.Sin(theta) * 
                    (Mathf.Cos(theta) * Mathf.Cos(2 * phi) * Mathf.Cos(psi) - Mathf.Sin(2 * phi) * Mathf.Sin(psi));
                break;

            case 5:
                output =
                    -Mathf.Sin(theta) *
                    (Mathf.Cos(theta) * Mathf.Cos(2 * phi) * Mathf.Cos(psi) + Mathf.Sin(2 * phi) * Mathf.Sin(psi));
                break;

            default: output = 0; break;
        }

        return output;


    }
}
