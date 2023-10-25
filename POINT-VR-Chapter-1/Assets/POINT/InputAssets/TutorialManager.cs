using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class TutorialManager : MonoBehaviour
{
    // Serialized fields
    /// <summary>
    /// The maximum distance between the player and the sphere during the teleportation tutorial, below which the grab tutorial will be triggered.
    /// </summary>
    [SerializeField] private float thresholdDistanceToTeleportZone;
    [Header("References")]
    [SerializeField] private TMP_Text versionText;
    [SerializeField] private Image controlsImage;
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject massSphere;
    [SerializeField] private GameObject teleportZone1;
    [SerializeField] private GameObject teleportZone2;
    [SerializeField] private GameObject teleportZone3;
    [SerializeField] private GameObject SceneUIContainer;
    [SerializeField] private InputActionReference leftPushingReference;
    [SerializeField] private InputActionReference leftPullingReference;
    [SerializeField] private InputActionReference rightPushingReference;
    [SerializeField] private InputActionReference rightPullingReference;
    [Header("Controls Graphics")]
    [SerializeField] private Sprite teleportationSprite;
    [SerializeField] private Sprite turnSprite;
    [SerializeField] private Sprite grabSprite;
    [SerializeField] private Sprite pushPullSprite;
    [SerializeField] private Sprite overSprite;
    [SerializeField] private Sprite menuSprite;
    [Header("Instructions Text")]
    [SerializeField] private string teleportationText;
    [SerializeField] private string turnText;
    [SerializeField] private string grabText;
    [SerializeField] private string pushPullText;
    [SerializeField] private string overText;
    [SerializeField] private string menuText;
    [SerializeField] private string openMenuText;
    // Cache
    private TMP_Text instructions = null;
    private Camera currentCamera = null;
    private GameObject player = null;
    private bool pushed = false, pulled = false;
    private GameObject menus = null;
    private GameObject buttons = null;
    private UIManager script = null;
    private void OnDisable()
    {
        leftPushingReference.action.started -= Pushed;
        leftPullingReference.action.started -= Pulled;
        rightPushingReference.action.started -= Pushed;
        rightPullingReference.action.started -= Pulled;
    }

    private void Start()
    {
        if (controlsImage != null)
        {
            instructions = controlsImage.GetComponentInChildren<TMP_Text>();
            controlsImage.gameObject.SetActive(false);
        }

        StartCoroutine(WaitForPlayerSpawn());
        floor.SetActive(false); // deactivate floor to prevent teleporting before tutorial start
        massSphere.SetActive(false);
        teleportZone1.SetActive(false);
        teleportZone2.SetActive(false);
        teleportZone3.SetActive(false);
        SceneUIContainer.SetActive(false);
        versionText.transform.parent.gameObject.SetActive(true);
        versionText.text = "Version: " + Application.version;
    }

    private void Update()
    {
        if (currentCamera != null)
        {
            this.transform.LookAt(currentCamera.transform);
            this.transform.Rotate(0, 180, 0);
        }
    }

    IEnumerator WaitForPlayerSpawn()
    {
        yield return new WaitUntil(() => Camera.current != null);

        // Start menu initialization
        currentCamera = Camera.current;
        this.GetComponent<Canvas>().worldCamera = currentCamera;
        player = currentCamera.transform.parent.gameObject;

        // player.GetComponent<PauseController>().enabled = false; // This should disable pausing, but it currently soft locks the player from continueing
                                                               // TODO: Disable the Oculus pause to bring up the UI menu

        player.GetComponent<TurnController>().enabled = false;
        this.transform.SetParent(player.transform.parent, false);
        leftPushingReference.action.started += Pushed;
        leftPullingReference.action.started += Pulled;
        rightPushingReference.action.started += Pushed;
        rightPullingReference.action.started += Pulled;

        yield break;
    }

    public void StartTutorial()
    {
        // Tutorial initialization
        player.GetComponent<PauseController>().enabled = true; // enable pausing
        player.GetComponent<TurnController>().enabled = true; // enable snap turn
        versionText.transform.parent.gameObject.SetActive(false); // deactivate start menu
        controlsImage.gameObject.SetActive(true); // activate controls graphics
        floor.SetActive(true); // activate floor for teleportation
        massSphere.SetActive(true);
        teleportZone1.SetActive(true);
        float teleportRingScale = (2 * thresholdDistanceToTeleportZone) / teleportZone1.GetComponent<SpriteRenderer>().size.x;
        teleportZone1.transform.localScale = new Vector3(teleportRingScale, teleportRingScale, teleportRingScale);
        teleportZone2.transform.localScale = new Vector3(teleportRingScale/2, teleportRingScale/2, teleportRingScale/2);
        teleportZone3.transform.localScale = new Vector3(teleportRingScale/3, teleportRingScale/3, teleportRingScale/3);

        // Turn tutorial
        controlsImage.sprite = turnSprite;
        instructions.text = turnText;

        //Instantiate menus from player prefab and buttons from player prefab as well
        GameObject mainCamera = player.transform.Find("Main Camera").gameObject;
        GameObject UIContainer = mainCamera.transform.Find("UI Container").gameObject;
        GameObject Menu = UIContainer.transform.Find("Menu").gameObject;
        GameObject HeaderButtons = Menu.transform.Find("HeaderButtons").gameObject;
        //GameObject HeaderButtons = Menu.transform.Find("Buttons").gameObject; //if testing with emulator use this
        GameObject menuScreens = Menu.transform.Find("MenuScreens").gameObject;

        menus = menuScreens;
        buttons = HeaderButtons;

        //Get UI Manager script
        script = Menu.GetComponent<UIManager>();
        StartCoroutine(WaitForTurn());
    }

    IEnumerator WaitForTurn()
    {
        float initialRotation = player.transform.rotation.y;

        yield return new WaitUntil(() => player.transform.rotation.y != initialRotation);

        // Teleportation tutorial
        controlsImage.sprite = teleportationSprite;
        instructions.text = teleportationText;

        StartCoroutine(WaitForTeleport()); 

        yield break;
    }

    IEnumerator WaitForTeleport()
    {
        // 3 teleport rings of increasing precision to achieve
        yield return new WaitUntil(() => Vector3.Distance(player.transform.position, teleportZone1.transform.position) <= thresholdDistanceToTeleportZone);

        teleportZone1.SetActive(false);
        teleportZone2.SetActive(true);
        yield return new WaitUntil(() => Vector3.Distance(player.transform.position, teleportZone2.transform.position) <= thresholdDistanceToTeleportZone/2);

        teleportZone2.SetActive(false);
        teleportZone3.SetActive(true);
        yield return new WaitUntil(() => Vector3.Distance(player.transform.position, teleportZone3.transform.position) <= thresholdDistanceToTeleportZone/3);
        teleportZone3.SetActive(false);

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
        pushed = false;
        pulled = false;
        yield return new WaitUntil(() => pushed && pulled);

        // Activate Scene Select
        // controlsImage.sprite = overSprite;
        // controlsImage.GetComponent<Image>().color = new Color32(255,255,255,0); // Makes image transparent, need to undone to controls image later
        // instructions.text = overText;
        // SceneUIContainer.SetActive(true);

        StartCoroutine(WaitForMenuPopup());

        yield break;
    }

    private void Pushed(InputAction.CallbackContext obj)
    {
        if (massSphere.activeInHierarchy && massSphere.transform.parent != null && massSphere.transform.parent.GetComponent<HandController>() != null)
        {
            pushed = true;
        }
    }

    private void Pulled(InputAction.CallbackContext obj)
    {
        if (massSphere.activeInHierarchy && massSphere.transform.parent != null && massSphere.transform.parent.GetComponent<HandController>() != null)
        {
            pulled = true;
        }
    }

    IEnumerator WaitForMenuPopup()
    {
        controlsImage.sprite = menuSprite;
        instructions.text = openMenuText;
        yield return new WaitUntil(() => menus.activeInHierarchy == true); //Waiting until player opens menu
        StartCoroutine(WaitForControlsScreenSelection());
        yield break;
    }

    IEnumerator WaitForControlsScreenSelection() 
    {

        controlsImage.sprite = overSprite;
        controlsImage.GetComponent<Image>().color = new Color32(255,255,255,0); // Makes image transparent, need to undone to controls image later

        script.ActivateMenu(menus.transform.Find("ControlsMenu").gameObject); //Activate Menus and Buttons
        script.ActivateButton(buttons.transform.Find("ControlsButton").gameObject);

        yield return new WaitForSecondsRealtime(5); //Temporary until narration is implemented
        StartCoroutine(WaitForGeneralMenu());
        yield break;
    }

    IEnumerator WaitForGeneralMenu() 
    {
        script.ActivateMenu(menus.transform.Find("GeneralMenu").gameObject); //Activate Menus and Buttons
        script.ActivateButton(buttons.transform.Find("GeneralButton").gameObject);

        yield return new WaitForSecondsRealtime(5); //Temporary until narration is implemented
        StartCoroutine(WaitForSceneSelection());
        yield break;
    }

    IEnumerator WaitForSceneSelection()
    {
        script.ActivateMenu(menus.transform.Find("ScenesMenu").gameObject); //Activate Menus and Buttons
        script.ActivateButton(buttons.transform.Find("ScenesButton").gameObject);

        yield return new WaitForSecondsRealtime(5); //Temporary until narration is implemented
        instructions.text = overText;
        SceneUIContainer.SetActive(true);
        yield break;
    }
}
