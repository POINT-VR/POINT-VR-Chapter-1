using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Constraints : MonoBehaviour
{
    List<Rigidbody> spheres = new List<Rigidbody>();
    public GridScript deformScript;

    public bool twoAtOnce = false;

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
        // if > 2 (or 1, depending on user settings), warn the user that the script is not designed to handle more than 1 sphere
        // if 0, do nothing
        if (spheres.Count == 0)
        {
            deformScript.rigidbodiesToDeformAround = new Rigidbody[0];
        }
        if (spheres.Count == 1 || (spheres.Count == 2 && twoAtOnce))
        {
            deformScript.rigidbodiesToDeformAround = spheres.ToArray();
        }
        else
        {
            deformScript.rigidbodiesToDeformAround = Array.Empty<Rigidbody>();
            Debug.LogWarning("The grid has not been configured to allow this number of spheres");
        }
    }

    private void AllowTwoSpheres()
    {
        twoAtOnce = true;
        UpdateDeforms();
    }

    private void RestrictTwoSpheres()
    {
        twoAtOnce = false;
        UpdateDeforms();
    }
}
