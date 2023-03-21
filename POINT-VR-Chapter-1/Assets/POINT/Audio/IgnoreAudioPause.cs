using UnityEngine;

public class IgnoreAudioPause : MonoBehaviour
{
    private void OnEnable()
    {
        // If audio source should ignore pausing (e.g. background music), this script should be attached
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.ignoreListenerPause = true;
        }
    }
}
