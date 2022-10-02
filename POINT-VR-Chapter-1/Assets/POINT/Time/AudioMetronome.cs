using UnityEngine;

public class AudioMetronome : MonoBehaviour
{
    /// <summary>
    /// The audio that will be played to represent a "tick" of the metronome
    /// </summary>
    [SerializeField] private AudioClip metronomeSound;
    /// <summary>
    /// Whether the metronome is being played. Other scripts can read and write to this.
    /// </summary>
    public bool playMetronome = true;
    /// <summary>
    /// The time between each metronome "tick". Other scripts can read and write to this.
    /// </summary>
    [Range(0.1f, 5.0f)]
    public float timeInterval = 1.0f;
    private AudioSource audioSource = null;
    private float lastTickTime = 0.0f;

    private void OnValidate()
    {
        if (audioSource == null)
        {
            audioSource = this.transform.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                this.gameObject.AddComponent<AudioSource>();
            }
        }
        audioSource.clip = metronomeSound;
    }
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = this.transform.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                this.gameObject.AddComponent<AudioSource>();
            }
        }
        audioSource.hideFlags = HideFlags.HideInInspector;
    }

    void Update()
    {
        if (playMetronome && Time.time >= lastTickTime + timeInterval)
        {
            audioSource.Play();
            lastTickTime = Time.time;
        }
    }
}