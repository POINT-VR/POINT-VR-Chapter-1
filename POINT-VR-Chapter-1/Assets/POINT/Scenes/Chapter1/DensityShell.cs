using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DensityShell : MonoBehaviour
{
    // The Selected checkbox
    SelectObject selected;
    // Reference to the Renderer component of the GameObject
    private Renderer objectRenderer;
    // The materials to switch between
    public Material material_unselected;  // The unselected material
    public Material material_selected; // The selected material


    // Start is called before the first frame update
    void Start()
    {
        selected = GetComponent<SelectObject>();
        objectRenderer = GetComponent<Renderer>();
        UpdateMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMaterial();
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
}
