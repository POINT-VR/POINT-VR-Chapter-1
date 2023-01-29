using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NarrationManager : MonoBehaviour
{
    // Constants
    private const string DEFAULT_SUBTITLE_LANGUAGE = "Disabled"; // "Disabled" being no subtitles by default

    /// <summary>
    /// The GameObject with a TextMeshPro component to display the subtitles
    /// </summary>
    [SerializeField] private GameObject subtitleObject = null;

    /// <summary>
    /// Volume multiplier for narration volume
    /// </summary>
    [SerializeField] private float volumeScale = 3.0f;

    private string subtitlesLanguage;
    public string SubtitlesLanguage
    {
        set
        {
            subtitlesLanguage = value;
            DisplaySubtitles();
        }
    }

    // Cache
    private TMP_Text subtitleText = null;
    private Image subtitleBackground = null;
    private string[] subtitleLines = null;
    private bool isSubtitlePlaying = false;
    private string audioName = null;
    private int currentLine = 0;

    private void OnEnable()
    {
        subtitlesLanguage = DEFAULT_SUBTITLE_LANGUAGE;
    }

    /// <summary>
    /// This function plays an audio clip (whose name WITHOUT the file type is the parameter) and activates
    /// the corresponding subtitles in a .txt file (but with .vtt formatting) with the SAME name (minus file extension).
    /// </summary>
    /// <param name="audioClipName"></param>
    public void PlayClipWithSubtitles (string audioClipName)
    {
        audioName = audioClipName;
        AudioClip audioClip = Resources.Load<AudioClip>(audioName);

        // Play narration audio
        if (audioClip == null)
        {
            Debug.LogWarning("Audio clip could not be loaded. Ensure that the name has been entered without the file extension.");
        }
        else
        {
            this.GetComponent<AudioSource>().PlayOneShot(audioClip, volumeScale);
        }

        DisplaySubtitles();
    }

    private void DisplaySubtitles()
    {
        TextAsset txtAsset = Resources.Load<TextAsset>(audioName + "_" + LanguageCode());
        if (txtAsset == null)
        {
            Debug.LogWarning("Caption file could not be found. Please ensure that the file format is .txt and that it includes '_<639-1_language_code>' as a suffix.");
        }
        else
        {
            subtitleLines = txtAsset.text.Split('\n');

            if (!isSubtitlePlaying)
            {
                float playTime = Time.time;
                StartCoroutine(GenerateSubtitles(playTime));
            }
            else
            {
                UpdateSubtitleUI(); // For subititle update midway through a line
            }
        }
    }

    private void UpdateSubtitleUI()
    {
        if (!subtitlesLanguage.Equals("Disabled"))
        {
            subtitleObject.SetActive(true);
            subtitleText.text = subtitleLines[currentLine + 1];
            subtitleBackground.rectTransform.sizeDelta = new Vector2(subtitleText.preferredWidth, subtitleText.preferredHeight);
        }
        else
        {
            subtitleObject.SetActive(false);
        }
    }

    IEnumerator GenerateSubtitles(float playTime)
    {
        subtitleText = subtitleObject.GetComponentInChildren<TMP_Text>();
        subtitleBackground = subtitleObject.GetComponent<Image>();

        if (subtitleText == null)
        {
            Debug.LogWarning("Subtitle text component could not be found.");
        }
        else
        {
            isSubtitlePlaying = true;

            for (int i = 2; i < subtitleLines.Length - 2; i += 3) // start from 2 since line 0 is WEBVTT, line 1 is empty; end 2 elements before since last 2 lines are empty; every set is 3 lines
            {
                currentLine = i;
                string[] timestamps = subtitleLines[i].Split(new string[] { "-->" }, StringSplitOptions.None);
                float startTime = float.Parse(timestamps[0].Split(':')[0]) * 60.0f + float.Parse(timestamps[0].Split(':')[1]);
                float endTime = float.Parse(timestamps[1].Split(':')[0]) * 60.0f + float.Parse(timestamps[1].Split(':')[1]);

                if (Time.time < playTime + startTime)
                {
                    // Hide subtitles if no current subtitles
                    subtitleObject.SetActive(false);
                }

                yield return new WaitUntil(() => Time.time >= playTime + startTime);
                UpdateSubtitleUI();
                yield return new WaitForSeconds(endTime - startTime);
            }

            subtitleObject.SetActive(false);
            isSubtitlePlaying = false;
        }
    }

    /// <summary>
    /// Returns the ISO 639-1 language code, found at https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes
    /// </summary>
    /// <returns></returns>
    private string LanguageCode()
    {
        switch (subtitlesLanguage) // more languages to be added here as required
        {
            case "English": return "en";
            default: return "en";
        }
    }
}
