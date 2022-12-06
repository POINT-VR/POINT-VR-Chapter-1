using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Constrains : MonoBehaviour
{
    List<Rigidbody> spheres = new List<Rigidbody>();
    public OptimalMeshDeformScript deformScript;

    private void OnTriggerEnter(Collider other)
    {
        // debug some stuff
        Debug.Log("OnTriggerEnter");
        spheres.Add(other.GetComponent<Rigidbody>());
        UpdateDeforms();
    }
  
    void UpdateDeforms()
    {
        if (spheres.Count == 0)
        {
            deformScript.rigidbodiesToDeformAround = new Rigidbody[0];
        }
        if (spheres.Count == 1)
        {
            deformScript.rigidbodiesToDeformAround = spheres.ToArray();
        }
        // if > 1, warn the user that the script is not designed to handle more than 1 sphere
        // if 0, do nothing

        if (spheres.Count>1)
        {
            deformScript.rigidbodiesToDeformAround = Array.Empty<Rigidbody>();
            Debug.LogWarning("The script is not designed to handle more than 1 sphere");
        }
    }

    private void OnTriggerExit(Collider other)
    {
            spheres.Remove(other.GetComponent<Rigidbody>());
            UpdateDeforms();
    }
}
