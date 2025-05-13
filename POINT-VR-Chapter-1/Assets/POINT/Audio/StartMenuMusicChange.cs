using UnityEngine;

public class StartMenuMusicChange : MonoBehaviour
{
    public AudioClip newClip;         // Assign in Inspector
    public float changeAfterSeconds = 0.0f;  // Time before changing

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        
        // Get the AudioSource from the Main Camera
        // GameObject mainCam = Camera.main.gameObject;
        // audioSource = mainCam.GetComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on Main Camera!");
            return;
        }

        // Start the timer to change the clip
        Invoke("ChangeAudioClip", changeAfterSeconds);
    }

    void ChangeAudioClip()
    {
        if (newClip != null)
        {
            // audioSource.Pause();
            audioSource.clip = newClip;
            audioSource.loop = true;
            audioSource.Play(); // Optional: play new clip immediately
        }
        else
        {
            Debug.LogWarning("New audio clip not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
