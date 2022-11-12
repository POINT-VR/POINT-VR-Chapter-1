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
        for (int j = 0; j < rigidbodiesToDeformAround.Length; j++) //Puts the mass positions on the stack ahead of time
        {
            massPositions[j] = rigidbodiesToDeformAround[j].transform.position;
        }

        Vector3 direction = originalPosition - massPositions[0];

        float timeInterval = 10000.0f;
        
        if (2*rigidbodiesToDeformAround[0].mass < direction.magnitude)
        {
            timeInterval = 1.0f/Mathf.Sqrt(1f - 2*rigidbodiesToDeformAround[0].mass / direction.magnitude);
        }

        if (Time.time >= lastTickTime + timeInterval*timeIntervalMultiplier)
        {
            audioSource.Play();
            lastTickTime = Time.time;
        }
    }
}