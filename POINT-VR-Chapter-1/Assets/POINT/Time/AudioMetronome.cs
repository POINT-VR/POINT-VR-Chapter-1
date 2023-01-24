using UnityEngine;
/// <summary>
/// This script manages the variable-rate audio of the metronome.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioMetronome : MonoBehaviour
{
    /// <summary>
    /// We will get the masses from the Rigidbody components
    /// </summary>
    public Rigidbody[] rigidbodiesToDeformAround;
    /// <summary>
    /// The multiplier affecting this sprite's timeInterval speed
    /// </summary>
    [SerializeField] float timeIntervalMultiplier;
    /// <summary>
    /// Power of the time dilation, strength of the dilation vs direction.magnitude. (0, 1.5] range for reccomended values.
    /// </summary>
    [SerializeField] float power;
    /// <summary>
    /// The maximum absolute magnitude at which a displacement will be calculated.
    /// </summary>
    [SerializeField] float cutoff;
    /// <summary>
    /// Whether or not the metronome is being played. Other scripts can read and write to this.
    /// </summary>
    public bool IsPlayingMetronome
    {
        get { return audioSource.enabled; }
        set { audioSource.enabled = value; }
    }
    private AudioSource audioSource;
    private float lastTickTime;
    private Vector3 originalPosition;
    void Start()
    {
        originalPosition = transform.position;
    }
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lastTickTime = 0f;
    }
    void Update()
    {
        float totalTimeInterval = 0.0f;
        float nMass = rigidbodiesToDeformAround.Length;
        for (int j = 0; j < rigidbodiesToDeformAround.Length; j++)
        {
            float r = (originalPosition - rigidbodiesToDeformAround[j].transform.position).magnitude;
            float timeInterval = 100000.0f;
            if (!rigidbodiesToDeformAround[j].gameObject.activeSelf)
            {
                nMass -= 1;
                continue;
            }
            if (r > cutoff)
            {
                totalTimeInterval += 1.0f;
                continue;
            }
            float p = power*2*rigidbodiesToDeformAround[j].mass;
            if (p < r)
            {
                timeInterval = 1.0f / Mathf.Sqrt(1f - ( p / r ) );
            }
            totalTimeInterval += timeInterval; //Displacement from each mass is calculated
        }   
        totalTimeInterval /= nMass;
        if (Time.time >= lastTickTime + totalTimeInterval*timeIntervalMultiplier)
        {
            audioSource.Play();
            lastTickTime = Time.time;
        }
    }
}