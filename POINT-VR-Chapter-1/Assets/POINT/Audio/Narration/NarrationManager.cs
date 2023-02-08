using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NarrationManager : MonoBehaviour
{
    // Constants
    private const string DEFAULT_SUBTITLE_LANGUAGE = "English";
    private static readonly string[] ACCEPTED_COLORS = { "black", "blue", "green", "orange", "purple", "red", "white", "yellow" };

    /// <summary>
    /// The GameObject with a TextMeshPro component to display the subtitles
    /// </summary>
    [SerializeField] private GameObject subtitleObject = null;

    /// <summary>
    /// Volume multiplier for narration volume
    /// </summary>
    [SerializeField] private float volumeScale = 3.0f;

    [Header("Font Assets")]
    [SerializeField] private TMP_FontAsset latinFont;
    [SerializeField] private TMP_FontAsset arabicFont;
    [SerializeField] private TMP_FontAsset bengaliFont;
    [SerializeField] private TMP_FontAsset hindiFont;
    [SerializeField] private TMP_FontAsset japaneseFont;
    [SerializeField] private TMP_FontAsset koreanFont;
    [SerializeField] private TMP_FontAsset chineseFont;
    [SerializeField] private TMP_FontAsset russianFont;
    [SerializeField] private TMP_FontAsset teluguFont;

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
    private TMP_FontAsset currentFont = null;
    private Coroutine coroutine = null;

    private void OnEnable()
    {
        subtitlesLanguage = DEFAULT_SUBTITLE_LANGUAGE;
        currentFont = latinFont;
    }

    /// <summary>
    /// This function plays an audio clip (whose name WITHOUT the file type is the parameter) and activates
    /// the corresponding subtitles in a .txt file (but with .vtt formatting) with the SAME name (minus file extension).
    /// </summary>
    /// <param name="audioClipName"></param>
    public void PlayClipWithSubtitles(string audioClipName)
    {
        // Reset
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            isSubtitlePlaying = false;
        }
        this.GetComponent<AudioSource>().Stop();

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
                coroutine = StartCoroutine(GenerateSubtitles(playTime));
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
            string output = ParseStyleTags(subtitleLines[currentLine + 1]);
            // Update UI
            subtitleText.font = currentFont;
            subtitleObject.SetActive(true);
            subtitleText.text = output;
            subtitleText.ForceMeshUpdate();
            subtitleBackground.rectTransform.sizeDelta = new Vector2(subtitleText.renderedWidth, subtitleText.renderedHeight);
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

            for (int i = 0; i < subtitleLines.Length; i++)
            {
                if (subtitleLines[i].Contains("-->")) // line contains timestamp
                {
                    currentLine = i;
                    string[] timestamps = subtitleLines[i].Split(new string[] { "-->" }, StringSplitOptions.None);
                    float startTime = TimestampToSeconds(timestamps[0].Trim().Split(' ')[0]);
                    float endTime = TimestampToSeconds(timestamps[1].Trim().Split(' ')[0]); // further split to remove possible coordinates

                    if (Time.time < playTime + startTime)
                    {
                        // Hide subtitles if no current subtitles
                        subtitleObject.SetActive(false);
                    }

                    yield return new WaitUntil(() => Time.time >= playTime + startTime);
                    UpdateSubtitleUI();
                    yield return new WaitForSeconds(endTime - startTime);

                    i++; // next line should be skipped since it is a subtitle line
                }
                else
                {
                    continue;
                }
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
            case "English":
                currentFont = latinFont;
                return "en";
            case "Español":
                currentFont = latinFont;
                return "es";
            case "Français":
                currentFont = latinFont;
                return "fr";
            case "中文":
                currentFont = chineseFont;
                return "zh";
            case "日本語":
                currentFont = japaneseFont;
                return "ja";
            case "हिन्दी":
                currentFont = hindiFont;
                return "hi";
            case "العربية":
                currentFont = arabicFont;
                return "ar";
            case "বাংলা":
                currentFont = bengaliFont;
                return "bn";
            case "русский язык":
                currentFont = russianFont;
                return "ru";
            case "Português":
                currentFont = latinFont;
                return "pt";
            case "Bahasa Melayu":
                currentFont = latinFont;
                return "ms";
            case "اردو":
                currentFont = arabicFont;
                return "ur";
            case "తెలుగు":
                currentFont = teluguFont;
                return "te";
            case "한국어":
                currentFont = koreanFont;
                return "ko";
            default:
                currentFont = latinFont;
                return "en";
        }
    }

    private float TimestampToSeconds(string input)
    {
        float seconds = 0.0f;
        input = input.Replace(",", "."); // change decimal indication from comma to period

        string[] timestamps = input.Split(':');
        for (int i = timestamps.Length - 1; i >= 0; i--)
        {
            seconds += float.Parse(timestamps[i]) * (float)(Math.Pow(60.0f, timestamps.Length - 1 - i));
        }

        return seconds;
    }

    private string ParseStyleTags(string input)
    {
        if (!input.Contains("<font "))
        {
            return input; // no work to be done if no font tags to be parsed
        }

        string output = "";
        int substrStart = 0;
        int substrEnd = 0;
        Stack tagStack = new Stack();

        input = input.Replace("</font>", "</>");

        while (substrEnd < input.Length - 1 && input.IndexOf('<', substrEnd) != -1)
        {
            substrEnd = input.IndexOf('<', substrEnd);
            output += input.Substring(substrStart, substrEnd - substrStart);

            if (substrEnd < input.Length - 4 && input.Substring(substrEnd + 1, 4).Equals("font")) // opening font tag
            {
                substrEnd += 6;
                string tagType = input.Substring(substrEnd, input.IndexOf('=', substrEnd) - substrEnd);
                tagStack.Push(tagType);
                output += "<" + tagType;

                if (tagType.Equals("color")) // tag value has to be parsed as well because of different formatting for colors e.g. hex vs name
                {
                    int valueStartIndex = input.IndexOf('\"', substrEnd) + 1;
                    int valueEndIndex = input.IndexOf('\"', valueStartIndex + 1) - 1;
                    string value = input.Substring(valueStartIndex, valueEndIndex - valueStartIndex + 1);
                    bool isHex = true;

                    for (int i = 0; i < ACCEPTED_COLORS.Length; i++)
                    {
                        if (ACCEPTED_COLORS[i].Equals(value))
                        {
                            isHex = false;
                        }
                    }

                    if (isHex)
                    {
                        output += input[valueStartIndex].Equals('#') ? ("=" + value) : ("=#" + value);
                        substrStart = input.IndexOf('>', valueEndIndex);
                    }
                    else
                    {
                        substrStart = input.IndexOf('=', substrEnd);
                    }
                }
                else
                {
                    substrStart = input.IndexOf('=', substrEnd);
                }
            }
            else if (substrEnd < input.Length - 1 && input.Substring(substrEnd + 1, 2).Equals("/>") && tagStack.Count > 0) // closing tag
            {
                string tagType = (string)tagStack.Pop();
                output += "</" + tagType + ">";
                substrStart = input.IndexOf('>', substrEnd) + 1;
                substrEnd += 3;
            }
            else // not a tag
            {
                substrStart = substrEnd;
                substrEnd++;
            }
        }

        output += input.Substring(substrStart, input.Length - substrStart);
        return output;
    }
}