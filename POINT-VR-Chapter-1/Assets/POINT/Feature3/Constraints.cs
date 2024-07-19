using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Constraints : MonoBehaviour
{
    List<Rigidbody> spheres = new List<Rigidbody>();
    public GridScript deformScript;

    private void OnTriggerEnter(Collider other)
    {
        if (!spheres.Contains(other.GetComponent<Rigidbody>()))
        {
            spheres.Add(other.GetComponent<Rigidbody>());
            UpdateDeforms();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (spheres.Contains(other.GetComponent<Rigidbody>()))
        {
            spheres.Remove(other.GetComponent<Rigidbody>());
            UpdateDeforms();
        }
    }

    void UpdateDeforms()
    {
        // if > 1, warn the user that the script is not designed to handle more than 1 sphere
        // if 0, do nothing
        if (spheres.Count == 0)
        {
            deformScript.rigidbodiesToDeformAround = new Rigidbody[0];
        }
        if (spheres.Count == 1)
        {
            deformScript.rigidbodiesToDeformAround = spheres.ToArray();
        }

        if (spheres.Count > 1)
        {
            deformScript.rigidbodiesToDeformAround = Array.Empty<Rigidbody>();
            Debug.LogWarning("The script is not designed to handle more than 1 sphere");
        }
    }
}
