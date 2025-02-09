using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attached to the Density Shell objects in the DensityExploration Scene
/// Handles material switching (will be replaced with animation), and collider updating to allow for both grabbing and selection
/// Snapping behavior and mass behavior is handled in DensityExploration.cs (can be moved here if necessary)
/// </summary>
public class DensityShell : MonoBehaviour
{
    // The Selected checkbox (when selected the shell is "open")
    SelectObject selected;
    // Reference to the Renderer component of the gameObject
    private Renderer objectRenderer;
    // Reference to the Collider component of the gameObject
    private Collider objectCollider;
    // Rigid body of the gameObject
     private Rigidbody rb;
    // The materials to switch between
    public Material material_unselected;  // The unselected material
    public Material material_selected; // The selected material


    void Start()
    {
        selected = GetComponent<SelectObject>();
        objectRenderer = GetComponent<Renderer>();
        objectCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        UpdateMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMaterial();
        UpdateCollider();
    }
    private void UpdateMaterial()
    {
        bool selectval = selected.GetSelected();
        if (objectRenderer != null)
        {
            // Change the material based on the Selected state
            objectRenderer.material = selectval ? material_selected : material_unselected;
        }
    }
    private void UpdateCollider()
    {
        bool selectval = selected.GetSelected();
        if (selectval)
        {
            if (!objectCollider.isTrigger)
            {
                objectCollider.isTrigger = true;
            }
            // change layer to make it selectable
            gameObject.layer = 12;
        }
        else
        {
            if (objectCollider.isTrigger)
            {
                objectCollider.isTrigger = false;
            }
            // change layer to make it grabbable
            gameObject.layer = 9;
        }
    }
}
