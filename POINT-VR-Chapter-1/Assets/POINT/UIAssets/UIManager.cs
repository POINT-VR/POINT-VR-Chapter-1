using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Constants
    private const float ACTIVE_BUTTON_FONT_SIZE = 64.0f;
    private const float INACTIVE_BUTTON_FONT_SIZE = 48.0f;
    private Color32 ACTIVE_BUTTON_COLOR = new Color32(255, 255, 255, 255);
    private Color32 INACTIVE_BUTTON_COLOR = new Color32(123, 231, 255, 127);

    [Header("Sprites")]
    [SerializeField] public Sprite toggleSelected = null;
    [SerializeField] public Sprite toggleUnselected = null;
    [Header("Volume Adjustments")]
    [SerializeField] private List<AudioSource> functionalAudio = null;
    [SerializeField] private List<AudioSource> aestheticAudio = null;
    [Header("Language Toggle Parent")]
    [SerializeField] Transform languageParent = null;
    [Header("Subtitles")]
    [SerializeField] NarrationManager narrationManager = null;
    [SerializeField] private RectTransform subtitlesContainer = null;
    [Tooltip("The range of y-positions for the subtitles; centered at value in Prefab")]
    [SerializeField] private float subtitlesHeightRange = 10.0f;
    [Tooltip("The range of font sizes for the subtitles; centered at value in Prefab")]
    [SerializeField] private float subtitlesSizeRange = 5.0f;
    [Tooltip("The range of widths for the subtitles container; centered at value in Prefab")]
    [SerializeField] private float subtitlesWidthRange = 10.0f;
    [Header("Subtitles Toggle Parent")]
    [SerializeField] Transform subtitleParent = null;
    [Header("Floor Toggle Parent")]
    [SerializeField] GameObject floorToggles;
    [Header("Current Objective")]
    [SerializeField] TMP_Text currentObjectiveTMP;

    private LocalizedString currentObjective = null;
    private string objectiveText;
    private float defaultSubtitlesHeight = float.NegativeInfinity;
    private float defaultSubtitlesSize = float.NegativeInfinity;
    private float defaultSubtitlesWidth = float.NegativeInfinity;

    private void Awake()
    {
        if (subtitlesContainer != null)
        {
            defaultSubtitlesHeight = subtitlesContainer.anchoredPosition.y;
            defaultSubtitlesSize = subtitlesContainer.GetComponentInChildren<TMP_Text>(true).fontSize;
            defaultSubtitlesWidth = subtitlesContainer.sizeDelta.x;

            if (narrationManager != null)
            {
                narrationManager.MaxSubtitleWidth = defaultSubtitlesWidth;
            }
        }
    }

    public void UpdateCurrentObjective(string newObjective)
    {
        currentObjectiveTMP.text = newObjective;
    }

    public void UpdateCurrentObjective(LocalizedString newObjective)
    {
        if (currentObjective != null)
        {
            currentObjective.StringChanged -= UpdateObjectiveLocalized;
        }
        currentObjective = newObjective;
        currentObjective.StringChanged += UpdateObjectiveLocalized;
        currentObjective.RefreshString();
    }

    private void UpdateObjectiveLocalized(string s)
    {
        objectiveText = s;
        RefreshCurrentObjectiveLocalized();
    }

    private void RefreshCurrentObjectiveLocalized()
    {
        if (currentObjectiveTMP)
        {
            currentObjectiveTMP.text = objectiveText;
        }
    }

    public void AddToFunctionalAudio(AudioSource audioSource)
    {
        functionalAudio.Add(audioSource);
    }

    public void AddToAestheticAudio(AudioSource audioSource)
    {
        aestheticAudio.Add(audioSource);
    }

    /// <summary>
    /// Activates corresponding menu and automatically deactivates all other menus
    /// </summary>
    /// <param name="menu"></param>
    public void ActivateMenu(GameObject menu)
    {
        Transform parent = menu.transform.parent;
        if (parent != null)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                parent.GetChild(i).gameObject.SetActive(i == menu.transform.GetSiblingIndex());
            }
        }
    }

    /// <summary>
    /// Changes corresponding button to have selected state styling, and reverts other buttons to
    /// inactive state styling
    /// </summary>
    /// <param name="button"></param>
    public void ActivateButton(GameObject button)
    {
        Transform parent = button.transform.parent;
        if (parent != null)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform buttonTransform = parent.GetChild(i).GetChild(0);
                if (buttonTransform != null)
                {
                    TextMeshProUGUI textComponent = buttonTransform.GetComponent<TextMeshProUGUI>();
                    if (textComponent != null)
                    {
                        if (i == button.transform.GetSiblingIndex())
                        {
                            textComponent.fontSizeMax = ACTIVE_BUTTON_FONT_SIZE;
                            textComponent.color = ACTIVE_BUTTON_COLOR;
                        }
                        else
                        {
                            textComponent.fontSizeMax = INACTIVE_BUTTON_FONT_SIZE;
                            textComponent.color = INACTIVE_BUTTON_COLOR;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Adjusts all functional audio (labelled as "Narration") volume to new value according to the slider that calls this method
    /// </summary>
    /// <param name="newVolume"></param>
    public void AdjustFunctionalAudioVolume(float newVolume)
    {
        foreach (AudioSource audioSource in functionalAudio)
        {
            audioSource.volume = newVolume;
        }
    }

    /// <summary>
    /// Adjusts all aesthetic audio (labelled as "Background") volume to new value according to the slider that calls this method
    /// </summary>
    /// <param name="newVolume"></param>
    public void AdjustAestheticAudioVolume(float newVolume)
    {
        foreach (AudioSource audioSource in aestheticAudio)
        {
            audioSource.volume = newVolume;
        }
    }

    private GameManager.Language language;
    public int Language
    {
        set
        {
            language = (GameManager.Language)value;
            if (subtitleLanguage != 0)
            {
                subtitleLanguage = language;
                narrationManager.SubtitlesLanguage = language;
            }

            if (value - 1 >= 0 && value - 1 < LocalizationSettings.AvailableLocales.Locales.Count)
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[value - 1];
            }
            
            for (int i = 0; i < languageParent.childCount; i++)
            {
                Image imageComponent = languageParent.GetChild(i).GetComponentInChildren<Image>();
                if (imageComponent != null)
                {
                    if (i == value - 1) // selected toggle; offset due to the lack of "Disabled" option
                    {
                        imageComponent.sprite = toggleSelected;
                    }
                    else
                    {
                        imageComponent.sprite = toggleUnselected;
                    }
                }
            }
        }
        get
        {
            return (int)language;
        }
    }

    private GameManager.Language subtitleLanguage;
    public int SubtitleLanguage
    {
        set
        {
            if (value == 0)
            {
                subtitleLanguage = (GameManager.Language)value;
                narrationManager.SubtitlesLanguage = subtitleLanguage;
            }
            else
            {
                subtitleLanguage = language;
                narrationManager.SubtitlesLanguage = language;
            }


            for (int i = 0; i < subtitleParent.childCount; i++)
            {
                Image imageComponent = subtitleParent.GetChild(i).GetComponentInChildren<Image>();
                if (imageComponent != null)
                {
                    if (i == value) // selected toggle
                    {
                        imageComponent.sprite = toggleSelected;
                    }
                    else
                    {
                        imageComponent.sprite = toggleUnselected;
                    }
                }
            }
        }
        get
        {
            return (int)subtitleLanguage;
        }
    }

    /// <summary>
    /// The value of the slider controlling the subtitle height; between 0 and 1 (inclusive)
    /// </summary>
    private float subtitleHeightValue = 0.5f;
    public float SubtitleHeightValue
    {
        get
        {
            return subtitleHeightValue;
        }

        set
        {
            if (float.IsNegativeInfinity(defaultSubtitlesHeight))
            {
                defaultSubtitlesHeight = subtitlesContainer.anchoredPosition.y;
            }
            subtitleHeightValue = value;
            subtitlesContainer.anchoredPosition = new Vector2(subtitlesContainer.anchoredPosition.x,
                            defaultSubtitlesHeight + ((value - 0.5f) * subtitlesHeightRange));
        }
    }

    /// <summary>
    /// The value of the slider controlling the subtitle font size; between 0 and 1 (inclusive)
    /// </summary>
    private float subtitleSizeValue = 0.5f;
    public float SubtitleSizeValue
    {
        get
        {
            return subtitleSizeValue;
        }

        set
        {
            if (float.IsNegativeInfinity(defaultSubtitlesSize))
            {
                defaultSubtitlesSize = subtitlesContainer.GetComponentInChildren<TMP_Text>(true).fontSize;
            }
            subtitleSizeValue = value;
            if (subtitlesContainer.GetComponentInChildren<TMP_Text>(true) != null)
            {
                subtitlesContainer.GetComponentInChildren<TMP_Text>(true).fontSize =
                    defaultSubtitlesSize + ((value - 0.5f) * subtitlesSizeRange);
            }
        }
    }

    /// <summary>
    /// The value of the slider controlling the subtitle width; between 0 and 1 (inclusive)
    /// </summary>
    private float subtitleWidthValue = 0.5f;
    public float SubtitleWidthValue
    {
        get
        {
            return subtitleWidthValue;
        }

        set
        {
            if (float.IsNegativeInfinity(defaultSubtitlesWidth))
            {
                defaultSubtitlesWidth = subtitlesContainer.sizeDelta.x;
            }
            subtitleWidthValue = value;

            if (narrationManager != null)
            {
                narrationManager.MaxSubtitleWidth = defaultSubtitlesWidth + ((value - 0.5f) * subtitlesWidthRange);
                subtitlesContainer.sizeDelta = new Vector2(
                    defaultSubtitlesWidth + ((value - 0.5f) * subtitlesWidthRange),
                    subtitlesContainer.sizeDelta.y);
            }
        }
    }

    /// <summary>
    /// Toggles whether the floor is visible (translucent) or invisible (default)
    /// </summary>
    /// <param name="enabled"></param>
    public void ActivateFloorToggle(bool enabled)
    {
        (Resources.FindObjectsOfTypeAll(typeof(MeshCollider))[0] as MeshCollider).gameObject.GetComponent<MeshRenderer>().enabled = enabled;
        // only known method to find Floor after it is inactive; would be preferable to use Layer or Tag to isolate, but this does not seem to be possible if the floor is inactive
    }

    public void ResetColliders()
    {
        // re-adjust collider positions since they do not automatically follow UI object on scroll
        Physics.SyncTransforms();
    }
}