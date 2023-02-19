using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Constants
    private const float ACTIVE_BUTTON_FONT_SIZE = 64.0f;
    private const float INACTIVE_BUTTON_FONT_SIZE = 48.0f;
    private Color32 ACTIVE_BUTTON_COLOR = new Color32(255, 255, 255, 255);
    private Color32 INACTIVE_BUTTON_COLOR = new Color32(123, 231, 255, 127);

    [SerializeField] private Sprite toggleSelected = null;
    [SerializeField] private Sprite toggleUnselected = null;
    [Header("Volume Adjustments")]
    [SerializeField] private AudioSource[] functionalAudio = null;
    [SerializeField] private AudioSource[] aestheticAudio = null;
    [Header("Subtitles")]
    [SerializeField] NarrationManager narrationManager = null;
    [Header("Floor")]
    [SerializeField] private MeshRenderer floorMeshRenderer = null;

    /// <summary>
    /// Activates corresponding menu and automatically deactivates all other menus
    /// </summary>
    /// <param name="menu"></param>
    public void ActivateMenu(GameObject menu)
    {
        Transform parent = menu.transform.parent?.transform;
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
        Transform parent = button.transform.parent?.transform;
        if (parent != null)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                TextMeshProUGUI textComponent = parent.GetChild(i).GetChild(0)?.GetComponent<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    if (i == button.transform.GetSiblingIndex())
                    {
                        textComponent.fontSize = ACTIVE_BUTTON_FONT_SIZE;
                        textComponent.color = ACTIVE_BUTTON_COLOR;
                    }
                    else
                    {
                        textComponent.fontSize = INACTIVE_BUTTON_FONT_SIZE;
                        textComponent.color = INACTIVE_BUTTON_COLOR;
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

    /// <summary>
    /// Toggles on selected toggle (i.e. radio button) and switches off everything else
    /// </summary>
    /// <param name="toggle"></param>
    public void ActivateLanguageToggle(GameObject toggle)
    {
        Transform parent = toggle.transform.parent?.transform;
        if (parent != null)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Image imageComponent = parent.GetChild(i).GetComponentInChildren<Image>();
                if (imageComponent != null)
                {
                    if (i == toggle.transform.GetSiblingIndex()) // selected toggle
                    {
                        imageComponent.sprite = toggleSelected;
                        narrationManager.SubtitlesLanguage = toggle.GetComponentInChildren<TMP_Text>().text;
                    }
                    else
                    {
                        imageComponent.sprite = toggleUnselected;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Toggles whether the floor is visible (translucent blue) or invisible (default)
    /// </summary>
    /// <param name="toggle"></param>
    public void ActivateFloorToggle(GameObject toggle)
    {
        floorMeshRenderer.enabled = toggle.GetComponentInChildren<TMP_Text>().text.Equals("On");

        Transform parent = toggle.transform.parent?.transform;
        if (parent != null)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Image imageComponent = parent.GetChild(i).GetComponentInChildren<Image>();
                if (imageComponent != null)
                {
                    if (i == toggle.transform.GetSiblingIndex()) // selected toggle
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
    }
}
