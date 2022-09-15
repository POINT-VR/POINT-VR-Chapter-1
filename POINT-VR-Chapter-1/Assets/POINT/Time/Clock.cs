using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Clock : MonoBehaviour
{
    /// <summary>
    /// The camera transform that this sprite will face
    /// </summary>
    [SerializeField] Transform cameraObject;
    /// <summary>
    /// The multiplier affecting this sprite's rotation speed
    /// </summary>
    [SerializeField] float rotationMultiplier;
    /// <summary>
    /// The speed at which this sprite rotates. Other scripts can read and write to this.
    /// </summary>
    public float rotationSpeed;
    /// <summary>
    /// The current color of this clock sprite. Other scripts can read and write to this.
    /// </summary>
    public Color32 Color { 
        get { 
            return GetComponent<SpriteRenderer>().color;
        }
        set { 
            GetComponent<SpriteRenderer>().color = value;
        }
    }
    private float zAngle = 0.0f;
    void Update()
    {
        transform.LookAt(cameraObject);
        zAngle += rotationSpeed * rotationMultiplier * Time.deltaTime; //Can (rotationSpeed * rotationMultiplier) be combined into a single argument?
        transform.Rotate(0.0f, 0.0f, zAngle);
    }
}
