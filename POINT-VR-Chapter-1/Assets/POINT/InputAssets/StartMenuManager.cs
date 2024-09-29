using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    [Tooltip("Image containing the POINT logo, to fade in before the start menu")]
    [SerializeField] private Image pointLogo = null;

    [Tooltip("Start menu containing simulation logo and interactable buttons")]
    [SerializeField] private GameObject fullMenu = null;

    [Tooltip("TextMeshPro component that displays the current version number")]
    [SerializeField] private TMP_Text versionText = null;

    [Tooltip("Transform containing the subtitle toggles")]
    [SerializeField] private Transform subtitleParent = null;

    /// <summary>
    /// A reference to the player GameObject
    /// </summary>
    private GameObject player = null;

    /// <summary>
    /// A reference to the player UI Manager
    /// </summary>
    private UIManager uiManager = null;

    /// <summary>
    /// Components within the menu that need to face the player
    /// </summary>
    private Transform[] menuComponents;

    /// <summary>
    ///  The last saved position of the camera (for menu components to face towards)
    /// </summary>
    private Vector3 lastCameraPosition = Vector3.positiveInfinity;

    private int subtitleLanguage = 1;
    public int SubtitleLanguage
    {
        set
        {
            if (uiManager == null)
            {
                if (player != null)
                {
                    uiManager = player.GetComponentInChildren<UIManager>(true);
                }
            }

            if (uiManager != null && subtitleParent != null)
            {
                for (int i = 0; i < subtitleParent.childCount; i++)
                {
                    Image imageComponent = subtitleParent.GetChild(i).GetComponentInChildren<Image>();
                    if (imageComponent != null)
                    {
                        if (i == value) // selected toggle
                        {
                            imageComponent.sprite = uiManager.toggleSelected;
                        }
                        else
                        {
                            imageComponent.sprite = uiManager.toggleUnselected;
                        }
                    }
                }
            }
            
            subtitleLanguage = value;
        }
    }

    private IEnumerator Start()
    {
        if (fullMenu != null)
        {
            fullMenu.SetActive(false);
            menuComponents = fullMenu.transform.GetComponentsInChildren<Transform>();
        }

        StartCoroutine(DisablePause());

        if (versionText != null)
        {
            versionText.text = "Version: " + Application.version;
        }

        if (pointLogo != null)
        {
            yield return FadeIn(pointLogo, 2.0f);
            yield return new WaitForSeconds(1.0f);
            yield return FadeOut(pointLogo, 1.0f);
            pointLogo.gameObject.SetActive(false);
        }

        if (fullMenu != null)
        {
            fullMenu.SetActive(true);
        }

        yield break;
    }

    private void Update()
    {
        if (Camera.current != null && lastCameraPosition != Camera.current.transform.position)
        {
            lastCameraPosition = Camera.current.transform.position;
            foreach (Transform menuComponent in menuComponents)
            {
                if (menuComponent != fullMenu.transform)
                {
                    menuComponent.LookAt(Camera.current.transform.position);
                    menuComponent.Rotate(Vector3.up * 180.0f);
                }
                
            }
        }
    }

    private IEnumerator DisablePause()
    {
        yield return new WaitUntil(() => Camera.current != null);

        player = Camera.current.transform.parent.gameObject;
        player.GetComponent<PauseController>().ToggleReference.action.Disable();
    }

    private IEnumerator FadeIn(Image image, float duration)
    {
        if (image != null)
        {
            float timePassed = 0.0f;
            image.color = Color.clear;
            
            while (timePassed < duration)
            {
                image.color = Color.Lerp(Color.clear, Color.white, timePassed / duration);
                timePassed += Time.deltaTime;
                yield return null;
            }

            image.color = Color.white;
        }
        yield break;
    }

    private IEnumerator FadeOut(Image image, float duration)
    {
        if (image != null)
        {
            float timePassed = 0.0f;
            image.color = Color.white;

            while (timePassed < duration)
            {
                image.color = Color.Lerp(Color.white, Color.clear, timePassed / duration);
                timePassed += Time.deltaTime;
                yield return null;
            }

            image.color = Color.clear;
        }
        yield break;
    }

    public void OpenOptions()
    {
        if (player != null && player.TryGetComponent(out PauseController pauseController))
        {
            pauseController.Pause();
        }
    }

    public void StartGame()
    {
        if (uiManager == null)
        {
            if (player != null)
            {
                uiManager = player.GetComponentInChildren<UIManager>(true);
            }
        }

        if (uiManager != null)
        {
            uiManager.SubtitleLanguage = subtitleLanguage;
        }

        if (player != null)
        {
            SceneController sceneController = player.GetComponentInChildren<SceneController>();
            if (sceneController != null)
            {
                sceneController.ChangeScene(1);
            }
        }
    }
}
