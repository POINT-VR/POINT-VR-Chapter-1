using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Clock : MonoBehaviour
{
    /// <summary>
    /// We will get the masses from the Rigidbody components
    /// </summary>
    public Rigidbody[] rigidbodiesToDeformAround;

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
    //public float rotationSpeed;

    private Vector3 originalPosition;

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

    void Start()
    {
        originalPosition  = GetComponent<Transform>().transform.position;
    }

    private float zAngle = 0.0f;
    void Update()
    {
        Vector3[] massPositions = new Vector3[rigidbodiesToDeformAround.Length];
        for (int j = 0; j < rigidbodiesToDeformAround.Length; j++) //Puts the mass positions on the stack ahead of time
        {
            massPositions[j] = rigidbodiesToDeformAround[j].transform.position;
        }

        Vector3 direction = originalPosition - massPositions[0];

        float rotationSpeed = 0.0f;
        
        if (2*rigidbodiesToDeformAround[0].mass < direction.magnitude)
        {
            rotationSpeed = Mathf.Sqrt(1f - 2*rigidbodiesToDeformAround[0].mass / direction.magnitude);
        }

        transform.LookAt(cameraObject);
        zAngle += rotationSpeed * rotationMultiplier * Time.deltaTime; //Can (rotationSpeed * rotationMultiplier) be combined into a single argument?
        transform.Rotate(0.0f, 0.0f, zAngle);
    }
}
