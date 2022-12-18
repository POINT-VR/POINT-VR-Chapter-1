using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AudioMetronome : MonoBehaviour
{
    /// <summary>
    /// We will get the masses from the Rigidbody components
    /// </summary>
    public Rigidbody[] rigidbodiesToDeformAround;

    private Vector3 originalPosition;

    /// The multiplier affecting this sprite's timeInterval speed
    /// </summary>
    [SerializeField] float timeIntervalMultiplier;
    /// <summary>

    // Sonali: Not sure how to rework this bottom section so rewrote the variable for timeInterval.
    /// <summary>
    /// The time between each metronome "tick". Other scripts can read and write to this.
    /// </summary>
    //private float TimeInterval
    //{
    //    get { return timeInterval; }
    //    set { timeInterval = Mathf.Clamp(value, 0.1f, 5.0f); }
    //}
    /// <summary>
    /// The time between each metronome "tick". Editor only.
    /// </summary>
    //[Range(0.1f, 5.0f)]
    //[SerializeField] float timeInterval;

    /// <summary>
    /// Power of the time dilation, strength of the dilation vs direction.magnitude. (0, 1.5] range for reccomended values.
    /// </summary>
    public float power = 1f;
    public float cutoff = 5f;

    /// <summary>
    /// Whether the metronome is being played. Other scripts can read and write to this.
    /// </summary>
    public bool IsPlayingMetronome
    {
        get { return audioSource.enabled; }
        set { audioSource.enabled = value; }
    }

    void Start()
    {
        originalPosition  = GetComponent<Transform>().transform.position;
    }

    private AudioSource audioSource;
    private float lastTickTime;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lastTickTime = 0f;
    }
    void Update()
    {
        Vector3[] massPositions = new Vector3[rigidbodiesToDeformAround.Length];
        
        float totalTimeInterval = 0.0f;
        float nMass = rigidbodiesToDeformAround.Length;

        for (int j = 0; j < rigidbodiesToDeformAround.Length; j++) //Puts the mass positions on the stack ahead of time
        {
            massPositions[j] = rigidbodiesToDeformAround[j].transform.position;
            Vector3 direction = originalPosition - massPositions[j];
            float r = direction.magnitude;

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