using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NarrationManager : MonoBehaviour
{
    /// <summary>
    /// This is the path to the folder containing both the audio clip and the .vtt file.
    /// Path must be set here since OnCast() allows only for inputs of basic types e.g. string instead of AudioClip.
    /// </summary>
    [SerializeField]
    private string folderPath = "Assets/POINT/Audio/Narration/Resources/";
    /// <summary>
    /// The GameObject with a TextMeshPro component to display the subtitles
    /// </summary>
    [SerializeField]
    private GameObject subtitleObject = null;

    private void Start()
    {
        PlayClipWithSubtitles("Narration_test");
    }

    /// <summary>
    /// This function plays an audio clip (whose name WITHOUT the file type is the parameter) and activates
    /// the corresponding subtitles in a .vtt file with the SAME name (minus file extension).
    /// </summary>
    /// <param name="audioClipName"></param>
    public void PlayClipWithSubtitles (string audioClipName)
    {
        AudioClip audioClip = Resources.Load(audioClipName) as AudioClip;
        string[] vttLines = System.IO.File.ReadAllLines(folderPath + audioClipName + ".vtt");

        if (audioClip == null)
        {
            Debug.LogWarning("Audio clip could not be loaded. Ensure that the name has been entered without the file extension.");
        }
        else
        {
            this.GetComponent<AudioSource>().PlayOneShot(audioClip, 3.0f);
        }
        
        float playTime = Time.time;
        
        if (vttLines == null)
        {
            Debug.LogWarning(".vtt file could not be found. Please ensure that the path name has been entered correctly.");
        }
        else if (!vttLines[0].Equals("WEBVTT"))
        {
            Debug.LogWarning("File found is not a .vtt file. Subtitles will not be generated.");
        }
        else
        {
            StartCoroutine(GenerateSubtitles(vttLines, playTime));   
        }
    }

    IEnumerator GenerateSubtitles(string[] vttLines, float playTime)
    {
        TMP_Text subtitleText = subtitleObject.GetComponentInChildren<TMP_Text>();
        Image subtitleBackground = subtitleObject.GetComponent<Image>();

        if (subtitleText == null)
        {
            Debug.LogWarning("Subtitle text component could not be found.");
        }
        else
        {
            for (int i = 2; i < vttLines.Length; i += 3) // start from 2 since line 0 is WEBVTT, line 1 is empty; every set is 3 lines
            {
                string[] timestamps = vttLines[i].Split(new string[] { "-->" }, StringSplitOptions.None);
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
                subtitleText.text = vttLines[i + 1];
                subtitleBackground.rectTransform.sizeDelta = new Vector2(subtitleText.preferredWidth, subtitleText.preferredHeight);

                yield return new WaitForSeconds(endTime - startTime);
            }

            subtitleObject.SetActive(false);
        }
    }
}
