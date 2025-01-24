using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DensityShell : MonoBehaviour
{
    // The Selected checkbox
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


    // Start is called before the first frame update
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
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object has a Rigidbody and a mass
        Rigidbody otherRb = other.GetComponent<Rigidbody>();
        if (otherRb != null)
        {
            // Increase the mass of the current object's Rigidbody by the mass of the other object's Rigidbody
            rb.mass += otherRb.mass;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Rigidbody otherRb = other.GetComponent<Rigidbody>();
        if (otherRb != null)
        {
            // Decrease the mass of the current object by the mass of the other object
            rb.mass -= otherRb.mass;
        }
    }
}
