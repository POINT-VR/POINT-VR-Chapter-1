using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    [Header("Subtitles Toggle Parent")]
    [SerializeField] Transform subtitleParent = null;
    [Header("Floor Toggle Parent")]
    [SerializeField] GameObject floorToggles;
    [Header("Current Objective")]
    [SerializeField] TMP_Text currentObjectiveTMP;

    public void updateCurrentObjective(string newObjective)
    {
        currentObjectiveTMP.text = newObjective;
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
            subtitleLanguage = (GameManager.Language)value;
            narrationManager.SubtitlesLanguage = subtitleLanguage;
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