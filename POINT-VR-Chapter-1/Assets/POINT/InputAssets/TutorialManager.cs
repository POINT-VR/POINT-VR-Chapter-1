using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    // Serialized fields
    /// <summary>
    /// The maximum distance between the player and the sphere during the teleportation tutorial, below which the grab tutorial will be triggered.
    /// </summary>
    [SerializeField] private float thresholdDistanceToSphere;
    [Header("References")]
    [SerializeField] private TMP_Text versionText;
    [SerializeField] private Image controlsImage;
    [SerializeField] private GameObject massSphere;
    [Header("Controls Graphics")]
    [SerializeField] private Sprite teleportationSprite;
    [SerializeField] private Sprite grabSprite;
    [SerializeField] private Sprite pushPullSprite;
    [SerializeField] private Sprite menuSprite;
    [Header("Instructions Text")]
    [SerializeField] private string teleportationText;
    [SerializeField] private string grabText;
    [SerializeField] private string pushPullText;
    [SerializeField] private string menuText;


    // Cache
    private TMP_Text instructions = null;
    private Camera currentCamera = null;
    private GameObject player = null;

    void Start()
    {
        if (controlsImage != null)
        {
            instructions = controlsImage.GetComponentInChildren<TMP_Text>();
            controlsImage.gameObject.SetActive(false);
        }

        StartCoroutine(WaitForPlayerSpawn());
        massSphere.SetActive(false);
        versionText.transform.parent.gameObject.SetActive(true);
        versionText.text = "Version: " + Application.version;
    }

    IEnumerator WaitForPlayerSpawn()
    {
        yield return new WaitUntil(() => Camera.current != null);

        // Start menu initialization
        currentCamera = Camera.current;
        this.transform.SetParent(currentCamera.transform, false);
        this.GetComponent<Canvas>().worldCamera = currentCamera;
        player = currentCamera.transform.parent.gameObject;
        player.GetComponent<PauseController>().enabled = false;

        yield break;
    }

    public void StartTutorial()
    {
        // Tutorial initialization
        player.GetComponent<PauseController>().enabled = true; // enable pausing
        versionText.transform.parent.gameObject.SetActive(false); // deactivate start menu
        controlsImage.gameObject.SetActive(true); // activate controls graphics
        massSphere.SetActive(true);

        // Teleportation tutorial
        controlsImage.sprite = teleportationSprite;
        instructions.text = teleportationText;
        
        StartCoroutine(WaitForTeleport());
    }

    IEnumerator WaitForTeleport()
    {
        yield return new WaitUntil(() => Vector3.Distance(player.transform.position, massSphere.transform.position) <= thresholdDistanceToSphere);

        // Grab tutorial
        controlsImage.sprite = grabSprite;
        instructions.text = grabText;

        StartCoroutine(WaitForGrab());

        yield break;
    }

    IEnumerator WaitForGrab()
    {
        yield return new WaitUntil(() => massSphere.transform.parent != null && massSphere.transform.parent.GetComponent<HandController>() != null);

        // Push and pull tutorial
        controlsImage.sprite = pushPullSprite;
        instructions.text = pushPullText;

        StartCoroutine(WaitForPushPull());

        yield break;
    }

    IEnumerator WaitForPushPull()
    {
        bool pushed = false, pulled = false;
        Vector3 previousPosition = massSphere.transform.position;
        yield return new WaitUntil(() => {
            if (Vector3.Distance(player.transform.position, massSphere.transform.position) > Vector3.Distance(player.transform.position, previousPosition))
            {
                pushed = true;
            } else if (Vector3.Distance(player.transform.position, massSphere.transform.position) < Vector3.Distance(player.transform.position, previousPosition))
            {
                pulled = true;
            }
            previousPosition = massSphere.transform.position;
            return pushed && pulled;
        });

        // Menu tutorial
        controlsImage.sprite = menuSprite;
        instructions.text = menuText;

        yield break;
    }
}
