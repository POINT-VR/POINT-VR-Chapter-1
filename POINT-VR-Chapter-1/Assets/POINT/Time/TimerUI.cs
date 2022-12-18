using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
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

    /// <summary>
    /// The multiplier affecting this sprite's rotation speed
    /// </summary>
    [SerializeField] float rotationMultiplier;

    private Vector3 originalPosition;
    private TextMeshProUGUI textMeshProObject;
    private float charWidth = 0.7f;

    void Start()
    {
        originalPosition  = GetComponent<Transform>().transform.position;

        textMeshProObject = GameObject.Find("Time Counter Text").GetComponent<TextMeshProUGUI>();
    }

    private float zAngle = 0.0f;
    void Update()
    {
        Vector3[] massPositions = new Vector3[rigidbodiesToDeformAround.Length];
        float totalRotationSpeed = 0.0f;

        for (int j = 0; j < rigidbodiesToDeformAround.Length; j++) //Puts the mass positions on the stack ahead of time
        {
            massPositions[j] = rigidbodiesToDeformAround[j].transform.position;
            Vector3 direction = originalPosition - massPositions[j];

            float rotation = 0.0f;

            if (2*rigidbodiesToDeformAround[j].mass < direction.magnitude)
            {
                rotation = Mathf.Sqrt(1f - 2*rigidbodiesToDeformAround[j].mass / direction.magnitude);
            }

            totalRotationSpeed += rotation; //Displacement from each mass is calculated
        }

        transform.LookAt(cameraObject);
        transform.Rotate(0.0f, 180.0f, 0.0f);

        zAngle += totalRotationSpeed * rotationMultiplier * Time.deltaTime; //Can (rotationSpeed * rotationMultiplier) be combined into a single argument?
        float counter = zAngle/360.0f;
        string counterStr = counter.ToString("N1"); // 2 decimal precision 
        
        //textMeshProObject.SetText($"<mspace={charWidth}em>{counterStr}");
        textMeshProObject.SetText($"{counterStr}");
    }
}
