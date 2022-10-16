using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AudioMetronome : MonoBehaviour
{
    /// <summary>
    /// The time between each metronome "tick". Other scripts can read and write to this.
    /// </summary>
    public float TimeInterval
    {
        get { return timeInterval; }
        set { timeInterval = Mathf.Clamp(value, 0.1f, 5.0f); }
    }
    /// <summary>
    /// The time between each metronome "tick". Editor only.
    /// </summary>
    [Range(0.1f, 5.0f)]
    [SerializeField] float timeInterval;
    /// <summary>
    /// Whether the metronome is being played. Other scripts can read and write to this.
    /// </summary>
    public bool IsPlayingMetronome
    {
        get { return audioSource.enabled; }
        set { audioSource.enabled = value; }
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
        if (Time.time >= lastTickTime + timeInterval)
        {
            audioSource.Play();
            lastTickTime = Time.time;
        }
    }
}