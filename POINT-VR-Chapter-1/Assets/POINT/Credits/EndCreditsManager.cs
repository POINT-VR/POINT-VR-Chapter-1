using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EndCreditsManager : MonoBehaviour
{
    // Rich text tags for different classes of text
    private const string CATEGORY_TAGS_OPEN = "<size=\"12\"><uppercase><b>";
    private const string CATEGORY_TAGS_CLOSE = "</b></uppercase></size>";

    private const string POSITION_TAGS_OPEN = "<b>";
    private const string POSITION_TAGS_CLOSE = "</b>";

    private const string NAME_TAGS_OPEN = "";
    private const string NAME_TAGS_CLOSE = "";

    #region Classes for JSON Parsing
    [System.Serializable]
    private class Credits
    {
        public CreditsCategory[] creditsCategories;
    }

    [System.Serializable]
    private class CreditsCategory
    {
        public string categoryTitle;
        public CreditsPosition[] positions;
    }

    [System.Serializable]
    private class CreditsPosition
    {
        public string positionTitle;
        public string[] names;
    }
    #endregion

    [Header("Credits Segments")]
    [Tooltip("The GameObject containing all components for the funding acknowledgements section")]
    [SerializeField] private GameObject fundingAcknowledgements = null;

    [Tooltip("The GameObject containing all components for the links section")]
    [SerializeField] private GameObject projectLinks = null;

    [Tooltip("Duration for each fade in and out")]
    [SerializeField] private float fadeDuration = 0.0f;

    [Header("Credits Scroll")]
    [Tooltip("Number of seconds after player spawn for the credits to start playing")]
    [SerializeField] private float delayBeforePlay = 0.0f;

    [Tooltip("TextMesh Pro component that will display credits text")]
    [SerializeField] private TMP_Text creditsText = null;

    [Tooltip("The .json file containing the credits")]
    [SerializeField] private TextAsset creditsFile;

    [Tooltip("The UI Collider object representing the clickable area to pause the credits scroll")]
    [SerializeField] private UICollider clickableMask = null;

    [Tooltip("The y-coordinate of the world position where the credits start fading in")]
    [SerializeField] private float startY = 0.0f;

    [Tooltip("The y-coordinate of the world position where the credits start fading out")]
    [SerializeField] private float endY = 0.0f;

    [Tooltip("The distance along the y-axis over which the text fades in or out")]
    [SerializeField] private float fadeDistance = 0.0f;

    [Tooltip("The speed (units per second) that the credits are scrolling")]
    [SerializeField] private float scrollSpeed = 5.0f;

    /// <summary>
    ///  Canvas containing canvasText
    /// </summary>
    private Canvas canvas = null;

    /// <summary>
    /// Whether credits are currently scrolling (or are paused)
    /// </summary>
    private bool isPlaying = true;

    /// <summary>
    /// The y-coordinate at which the scroll should stop (because all text has faded out)
    /// </summary>
    private float endScrollY = 0.0f;

    private Camera currentCamera = null;
    private GameObject player = null;

    private IEnumerator Start()
    {
        yield return WaitForPlayerSpawn();

        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("temporary_ending_2");

        if (fundingAcknowledgements != null)
        {
            TMP_Text[] fundingTexts = fundingAcknowledgements.GetComponentsInChildren<TMP_Text>();
            Image[] fundingImages = fundingAcknowledgements.GetComponentsInChildren<Image>();

            // Fade in funding acknowledgements
            foreach (TMP_Text fundingText in fundingTexts)
            {
                StartCoroutine(FadeInText(fundingText, fadeDuration));
            }
            foreach (Image fundingImage in fundingImages)
            {
                StartCoroutine(FadeInImage(fundingImage, fadeDuration));
            }
            yield return new WaitForSeconds(fadeDuration);

            yield return new WaitForSeconds(8.0f);

            // Fade out funding acknowledgements
            foreach (TMP_Text fundingText in fundingTexts)
            {
                StartCoroutine(FadeOutText(fundingText, fadeDuration));
            }
            foreach (Image fundingImage in fundingImages)
            {
                StartCoroutine(FadeOutImage(fundingImage, fadeDuration));
            }
            yield return new WaitForSeconds(fadeDuration);
        }

        if (creditsFile != null && creditsText != null)
        {
            creditsText.gameObject.SetActive(true);
            clickableMask.gameObject.SetActive(true);
            ParseCredits();
            yield return ScrollCredits();
            creditsText.gameObject.SetActive(false);
            clickableMask.gameObject.SetActive(false);
        }

        if (projectLinks != null)
        {
            projectLinks.SetActive(true);
            TMP_Text[] projectTexts = projectLinks.GetComponentsInChildren<TMP_Text>();
            Image[] projectImages = projectLinks.GetComponentsInChildren<Image>();

            player.GetComponent<NarrationManager>().PlayClipWithSubtitles("temporary_ending_3");
            // Fade in project links
            foreach (TMP_Text projectText in projectTexts)
            {
                StartCoroutine(FadeInText(projectText, fadeDuration));
            }
            foreach (Image projectImage in projectImages)
            {
                StartCoroutine(FadeInImage(projectImage, fadeDuration));
            }
        }

        yield break;
    }

    private IEnumerator WaitForPlayerSpawn()
    {
        yield return new WaitUntil(() => Camera.current != null);

        currentCamera = Camera.current;
        player = currentCamera.transform.parent.gameObject;

        yield break;
    }

    private void ParseCredits()
    {
        canvas = creditsText.GetComponentInParent<Canvas>();

        // Parse JSON file
        Credits credits = JsonUtility.FromJson<Credits>(creditsFile.text);

        // Build credits text
        string creditsString = "";
        foreach (CreditsCategory category in credits.creditsCategories)
        {
            creditsString += CATEGORY_TAGS_OPEN + category.categoryTitle + CATEGORY_TAGS_CLOSE + "\n\n";
            foreach (CreditsPosition position in category.positions)
            {
                if (position.positionTitle.Length > 0)
                {
                    creditsString += POSITION_TAGS_OPEN + position.positionTitle + POSITION_TAGS_CLOSE + "\n";
                }

                foreach (string name in position.names)
                {
                    creditsString += NAME_TAGS_OPEN + name + NAME_TAGS_CLOSE + "\n";
                }
                creditsString += "\n";
            }
            creditsString += "\n";
        }
        creditsText.text = creditsString;

        // Update shader parameters based on script input
        Shader.SetGlobalFloat("_Credits_StartY", startY / (canvas == null ? 1.0f : canvas.transform.localScale.y));
        Shader.SetGlobalFloat("_Credits_EndY", endY / (canvas == null ? 1.0f : canvas.transform.localScale.y));
        Shader.SetGlobalFloat("_Credits_FadeDistance", fadeDistance / (canvas == null ? 1.0f : canvas.transform.localScale.y));

        creditsText.transform.position = new Vector3(creditsText.transform.position.x, startY, creditsText.transform.position.z);
        endScrollY = endY + (creditsText.preferredHeight * (canvas == null ? 1.0f : canvas.transform.localScale.y));

        // Set clickable mask to define region that player can click to pause the credits scroll
        if (clickableMask != null && clickableMask.TryGetComponent(out BoxCollider boxCollider))
        {
            boxCollider.transform.position = new Vector3(boxCollider.transform.position.x, (endY + startY) / 2.0f, boxCollider.transform.position.z);
            boxCollider.size = new Vector3(creditsText.preferredWidth, (endY - startY) / (canvas == null ? 1.0f : canvas.transform.localScale.y), 1.0f);
        }
    }

    private IEnumerator ScrollCredits()
    {
        yield return new WaitForSeconds(delayBeforePlay);
        while (creditsText.transform.position.y < endScrollY)
        {
            if (isPlaying)
            {
                creditsText.transform.position += scrollSpeed * Time.deltaTime * Vector3.up;
            }
            yield return null;
        }
    }

    public void ToggleCreditsPlay()
    {
        isPlaying = !isPlaying;
    }

    private IEnumerator FadeInImage(Image image, float duration)
    {
        float timePassed = 0.0f;
        while (timePassed < duration)
        {
            image.color = Color.Lerp(Color.clear, Color.white, timePassed / duration);
            timePassed += Time.deltaTime;
            yield return null;
        }

        image.color = Color.white;
    }

    private IEnumerator FadeInText(TMP_Text text, float duration)
    {
        float timePassed = 0.0f;
        while (timePassed < duration)
        {
            text.color = Color.Lerp(Color.clear, Color.white, timePassed / duration);
            timePassed += Time.deltaTime;
            yield return null;
        }

        text.color = Color.white;
    }

    private IEnumerator FadeOutImage(Image image, float duration)
    {
        float timePassed = 0.0f;
        while (timePassed < duration)
        {
            image.color = Color.Lerp(Color.white, Color.clear, timePassed / duration);
            timePassed += Time.deltaTime;
            yield return null;
        }

        image.color = Color.clear;
    }

    private IEnumerator FadeOutText(TMP_Text text, float duration)
    {
        float timePassed = 0.0f;
        while (timePassed < duration)
        {
            text.color = Color.Lerp(Color.white, Color.clear, timePassed / duration);
            timePassed += Time.deltaTime;
            yield return null;
        }

        text.color = Color.clear;
    }

    public void OpenWebsite()
    {
        Application.OpenURL("https://icasu.illinois.edu/outreach/point-vr");
    }

    public void OpenRepo()
    {
        Application.OpenURL("https://github.com/POINT-VR/POINT-VR-Chapter-1");
    }
}