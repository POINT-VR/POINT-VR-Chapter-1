using TMPro;
using UnityEngine;
/// <summary>
/// Manages the UI Containers attached to the clocks
/// </summary>
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
    /// The multiplier affecting this sprite's rotation speed
    /// </summary>
    [SerializeField] float rotationMultiplier;
    /// <summary>
    /// Power of the time dilation, strength of the dilation vs direction.magnitude. (0, 1.5] range for reccomended values.
    /// </summary>
    [SerializeField] float power;
    /// <summary>
    /// The maximum absolute magnitude at which a displacement will be calculated.
    /// </summary>
    [SerializeField] float cutoff;
    /// <summary>
    /// The TextMeshProObject to be updated
    /// </summary>
    [SerializeField] TextMeshProUGUI textMeshProObject;
    /// <summary>
    /// The should specific the monospace width if mspace is used.
    /// </summary>
    readonly private float charWidth = 0.7f;
    private Vector3 originalPosition;
    private float zAngle = 0.0f;
    void Start()
    {
        originalPosition = transform.position;
    }
    void Update()
    {
        float totalRotationSpeed = 0.0f;
        float nMass = rigidbodiesToDeformAround.Length;

        for (int j = 0; j < rigidbodiesToDeformAround.Length; j++) //Puts the mass positions on the stack ahead of time
        {
            float r = (originalPosition - rigidbodiesToDeformAround[j].transform.position).magnitude;
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
        totalRotationSpeed /= nMass;
        transform.LookAt(cameraObject);
        transform.Rotate(0.0f, 180.0f, 0.0f);
        zAngle += totalRotationSpeed * rotationMultiplier * Time.deltaTime;
        float counter = zAngle/360.0f;
        string counterStr = counter.ToString("N1"); // 1 decimal precision 
        textMeshProObject.SetText($"<mspace={charWidth}em>{counterStr}");
    }
}
