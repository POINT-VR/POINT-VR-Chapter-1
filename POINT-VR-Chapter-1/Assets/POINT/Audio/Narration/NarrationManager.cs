using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NarrationManager : MonoBehaviour
{
    /// <summary>
    /// The GameObject with a TextMeshPro component to display the subtitles
    /// </summary>
    [SerializeField]
    private GameObject subtitleObject = null;

    /// <summary>
    /// Volume multiplier for narration volume
    /// </summary>
    [SerializeField]
    private float volumeScale = 3.0f;

    /// <summary>
    /// This function plays an audio clip (whose name WITHOUT the file type is the parameter) and activates
    /// the corresponding subtitles in a .vtt file with the SAME name (minus file extension).
    /// </summary>
    /// <param name="audioClipName"></param>
    public void PlayClipWithSubtitles (string audioClipName)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(audioClipName);
        TextAsset txtAsset = Resources.Load<TextAsset>(audioClipName);

        if (audioClip == null)
        {
            Debug.LogWarning("Audio clip could not be loaded. Ensure that the name has been entered without the file extension.");
        }
        else
        {
            this.GetComponent<AudioSource>().PlayOneShot(audioClip, volumeScale);
        }
        
        float playTime = Time.time;
        
        if (txtAsset == null)
        {
            Debug.LogWarning("Caption file could not be found. Please ensure that the file format is .txt.");
        }
        else
        {
            string[] subtitleLines = txtAsset.text.Split('\n');
            if (!subtitleLines[0].Trim().Equals("WEBVTT"))
            {
                Debug.LogWarning("File found is not a caption file. Subtitles will not be generated.");
            }
            else
            {
                StartCoroutine(GenerateSubtitles(subtitleLines, playTime));
            }
        }
    }

    IEnumerator GenerateSubtitles(string[] subtitleLines, float playTime)
    {
        TMP_Text subtitleText = subtitleObject.GetComponentInChildren<TMP_Text>();
        Image subtitleBackground = subtitleObject.GetComponent<Image>();

        if (subtitleText == null)
        {
            Debug.LogWarning("Subtitle text component could not be found.");
        }
        else
        {
            for (int i = 2; i < subtitleLines.Length - 2; i += 3) // start from 2 since line 0 is WEBVTT, line 1 is empty; end 2 elements before since last 2 lines are empty; every set is 3 lines
            {
                string[] timestamps = subtitleLines[i].Split(new string[] { "-->" }, StringSplitOptions.None);
                float startTime = float.Parse(timestamps[0].Split(':')[0]) * 60.0f + float.Parse(timestamps[0].Split(':')[1]);
                float endTime = float.Parse(timestamps[1].Split(':')[0]) * 60.0f + float.Parse(timestamps[1].Split(':')[1]);
                
                if (Time.time < playTime + startTime)
                {
                    // Hide subtitles if no current subtitles
                    subtitleObject.SetActive(false);
                }
                
                yield return new WaitUntil(() => Time.time >= playTime + startTime);

                // Update UI
                subtitleObject.SetActive(true);
                subtitleText.text = subtitleLines[i + 1];
                subtitleBackground.rectTransform.sizeDelta = new Vector2(subtitleText.preferredWidth, subtitleText.preferredHeight);

                yield return new WaitForSeconds(endTime - startTime);
            }

            subtitleObject.SetActive(false);
        }
    }
}
