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
    /// Power of the time dilation, strength of the dilation vs direction.magnitude. (0, 1.5] range for reccomended values.
    /// </summary>
    public float power = 1f;
    public float cutoff = 5f;

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
        float totalRotationSpeed = 0.0f;
        float nMass = rigidbodiesToDeformAround.Length;

        for (int j = 0; j < rigidbodiesToDeformAround.Length; j++) //Puts the mass positions on the stack ahead of time
        {
            massPositions[j] = rigidbodiesToDeformAround[j].transform.position;
            Vector3 direction = originalPosition - massPositions[j];
            float r = direction.magnitude;

            float rotation = 1.0f;

            if (!rigidbodiesToDeformAround[j].gameObject.activeSelf)
            {
                nMass -= 1;
                continue;
            }

            if (r > cutoff)
            {
                totalRotationSpeed += rotation; //Displacement from each mass is calculated
                continue;
            }

            float p = power*2*rigidbodiesToDeformAround[j].mass;
            if (p < r)
            {
                rotation = Mathf.Sqrt(1f - ( p / r ) );
            }
            else
            {
                rotation = 0f;
            }

            totalRotationSpeed += rotation; //Displacement from each mass is calculated
        }

        totalRotationSpeed = totalRotationSpeed / nMass;

        transform.LookAt(cameraObject);
        zAngle += totalRotationSpeed * rotationMultiplier * Time.deltaTime;
        transform.Rotate(0.0f, 0.0f, zAngle);
    }
}
