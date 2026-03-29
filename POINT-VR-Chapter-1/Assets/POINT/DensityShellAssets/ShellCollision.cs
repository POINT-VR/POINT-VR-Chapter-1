using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attached to "Sphere" GameObject (only the big one) in DensityShell scene in order to handle interactions between the two shells
/// Works by manually finding objects with DensityShell script (aka the other shell) in a certain radius and moving them away from this shell
/// </summary>
public class ShellCollision : MonoBehaviour
{
    public float detectionRadius = 0.5f;
    void Start() {
        Vector3 scale = transform.localScale;
        detectionRadius = 0.5f * scale.x;  // Radius of the shell
    }

    void Update()
    {
        // Perform an overlap check around this object to see if any colliders are within the detection radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (var collider in colliders)
        {
            // Check if the other object has the "DensityShell" script
            DensityShell shellScript = collider.GetComponent<DensityShell>();

            if (shellScript != null)  // If the script is found (not null)
            {
                // Get the direction to move the object away from the shell
                Vector3 direction = collider.transform.position - transform.position;
                direction.Normalize();  // Normalize direction to avoid uneven pushing

                // Move the object away slightly
                collider.transform.position += direction * 0.1f;

                // Stop the object's movement
                Rigidbody sphereRigidbody = collider.GetComponent<Rigidbody>();
                if (sphereRigidbody != null)
                {
                    sphereRigidbody.velocity = Vector3.zero;
                }
            }
        }
    }
}
