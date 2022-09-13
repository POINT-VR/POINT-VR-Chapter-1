using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private Camera cameraObject = null;
    private SpriteRenderer spriteComponent = null;
    private float rotationMultiplier = 100.0f;

    public Color32 color = new Color32(255, 255, 255, 255);
    public float rotationSpeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        spriteComponent = this.transform.GetComponent<SpriteRenderer>();
        spriteComponent.hideFlags = HideFlags.HideInInspector;
    }

    // Update is called once per frame
    void Update()
    {
        spriteComponent.color = color;
        this.transform.LookAt(cameraObject.transform);
        this.transform.Rotate(0.0f, 0.0f, Time.time * rotationMultiplier * rotationSpeed);
    }
}
