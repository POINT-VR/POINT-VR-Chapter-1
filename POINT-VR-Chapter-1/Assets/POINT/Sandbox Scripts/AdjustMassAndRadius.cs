using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustMassAndRadius : MonoBehaviour
{
    [SerializeField] private float initialMass;
    [SerializeField] private float initialScale;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().mass = initialMass;
        transform.localScale = new Vector3(initialScale, initialScale, initialScale);
    }

    public void SetMass(float newMass)
    {
        gameObject.GetComponent<Rigidbody>().mass = newMass;
    }

    public void SetRadius(float newRadius)
    {
        transform.localScale = new Vector3(newRadius, newRadius, newRadius);
    }
}
