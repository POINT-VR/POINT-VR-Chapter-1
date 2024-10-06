using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Localization;

public class TutorialManager : MonoBehaviour
{
    // Serialized fields
    /// <summary>
    /// The maximum distance between the player and the sphere during the teleportation tutorial, below which the grab tutorial will be triggered.
    /// </summary>
    [SerializeField] private float thresholdDistanceToTeleportZone;
    [Header("References")]
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
    [SerializeField] private InputActionReference openMenuReference;
    [Header("Controls Graphics")]
    [SerializeField] private Sprite teleportationSprite;
    [SerializeField] private Sprite turnSprite;
    [SerializeField] private Sprite grabSprite;
    [SerializeField] private Sprite pushPullSprite;
    [SerializeField] private Sprite overSprite;
    [SerializeField] private Sprite menuSprite;
    [Header("Instructions Text")]
    [SerializeField] private LocalizedString teleportationText;
    [SerializeField] private LocalizedString turnText;
    [SerializeField] private LocalizedString grabText;
    [SerializeField] private LocalizedString pushPullText;
    [SerializeField] private LocalizedString overText;
    [SerializeField] private LocalizedString menuText;
    [SerializeField] private LocalizedString openMenuText;
    private string teleportationString;
    private string turnString;
    private string grabString;
    private string pushPullString;
    private string overString;
    private string menuString;
    private string openMenuString;


    // Cache
    private TMP_Text instructions = null;
    private Camera currentCamera = null;
    private GameObject player = null;
    private bool pushed = false, pulled = false;
    private GameObject menus = null;
    private GameObject buttons = null;
    private UIManager UIManagerScript = null;

    private void OnEnable()
    {
        teleportationText.StringChanged += UpdateTeleportationString;
        turnText.StringChanged += UpdateTurnString;
        grabText.StringChanged += UpdateGrabString;
        pushPullText.StringChanged += UpdatePushPullString;
        overText.StringChanged += UpdateOverString;
        menuText.StringChanged += UpdateMenuString;
        openMenuText.StringChanged += UpdateOpenMenuString;
    }

    private void OnDisable()
    {
        leftPushingReference.action.started -= Pushed;
        leftPullingReference.action.started -= Pulled;
        rightPushingReference.action.started -= Pushed;
        rightPullingReference.action.started -= Pulled;

        teleportationText.StringChanged -= UpdateTeleportationString;
        turnText.StringChanged -= UpdateTurnString;
        grabText.StringChanged -= UpdateGrabString;
        pushPullText.StringChanged -= UpdatePushPullString;
        overText.StringChanged -= UpdateOverString;
        menuText.StringChanged -= UpdateMenuString;
        openMenuText.StringChanged -= UpdateOpenMenuString;
    }

    private IEnumerator Start()
    {
        if (controlsImage != null)
        {
            instructions = controlsImage.GetComponentInChildren<TMP_Text>();
            controlsImage.gameObject.SetActive(false);
        }

        floor.SetActive(false); // deactivate floor to prevent teleporting before tutorial start
        massSphere.SetActive(false);
        teleportZone1.SetActive(false);
        teleportZone2.SetActive(false);
        teleportZone3.SetActive(false);
        SceneUIContainer.SetActive(false);

        yield return WaitForPlayerSpawn();
        StartTutorial();
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

    private void StartTutorial()
    {
        // Tutorial initialization
        player.GetComponent<PauseController>().enabled = true; // enable pausing
        player.GetComponent<TurnController>().enabled = true; // enable snap turn
        controlsImage.gameObject.SetActive(true); // activate controls graphics
        floor.SetActive(true); // activate floor for teleportation
        massSphere.SetActive(true);
        teleportZone1.SetActive(true);
        float teleportRingScale = (2 * thresholdDistanceToTeleportZone) / teleportZone1.GetComponent<SpriteRenderer>().size.x;
        teleportZone1.transform.localScale = new Vector3(teleportRingScale, teleportRingScale, teleportRingScale);
        teleportZone2.transform.localScale = new Vector3(teleportRingScale/2, teleportRingScale/2, teleportRingScale/2);
        teleportZone3.transform.localScale = new Vector3(teleportRingScale/3, teleportRingScale/3, teleportRingScale/3);

        //Instantiate menus from player prefab and buttons from player prefab as well
        GameObject mainCamera = player.transform.Find("Main Camera").gameObject;
        GameObject UIContainer = mainCamera.transform.Find("UI Container").gameObject;
        GameObject Menu = UIContainer.transform.Find("Menu").gameObject;
        GameObject HeaderButtons = Menu.transform.Find("HeaderButtons").gameObject;
        // GameObject HeaderButtons = Menu.transform.Find("Buttons").gameObject; //if testing with emulator use this
        GameObject menuScreens = Menu.transform.Find("MenuScreens").gameObject;

        menus = menuScreens;
        buttons = HeaderButtons;

        // Get UI Manager UIManagerScript
        UIManagerScript = Menu.GetComponent<UIManager>();

        // Turn tutorial
        controlsImage.sprite = turnSprite;
        instructions.text = turnString;
        
        player.GetComponentInChildren<UIManager>(true).updateCurrentObjective(instructions.text);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Tutorial\\Tutorial_Intro");

        StartCoroutine(WaitForTurn());
    }

    IEnumerator WaitForTurn()
    {
        float initialRotation = player.transform.rotation.y;

        yield return new WaitUntil(() => player.transform.rotation.y != initialRotation);

        // Teleportation tutorial

        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Tutorial\\Tutorial_Teleport");
        controlsImage.sprite = teleportationSprite;
        instructions.text = teleportationString;

        player.GetComponentInChildren<UIManager>(true).updateCurrentObjective(instructions.text);

        StartCoroutine(WaitForTeleport()); 

        yield break;
    }

    IEnumerator WaitForTeleport()
    {
        // 3 teleport rings of increasing precision to achieve
        yield return new WaitUntil(() => Vector3.Distance(player.transform.position, teleportZone1.transform.position) <= thresholdDistanceToTeleportZone);

        
        teleportZone1.SetActive(false);
        teleportZone2.SetActive(true);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Tutorial\\Tutorial_Teleport_Test_1");
        yield return new WaitUntil(() => Vector3.Distance(player.transform.position, teleportZone2.transform.position) <= thresholdDistanceToTeleportZone/2);

        
        teleportZone2.SetActive(false);
        teleportZone3.SetActive(true);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Tutorial\\Tutorial_Teleport_Test_2");
        yield return new WaitUntil(() => Vector3.Distance(player.transform.position, teleportZone3.transform.position) <= thresholdDistanceToTeleportZone/3);
        teleportZone3.SetActive(false);

        // Grab tutorial
        controlsImage.sprite = grabSprite;
        instructions.text = grabString;

        player.GetComponentInChildren<UIManager>(true).updateCurrentObjective(instructions.text);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Tutorial\\Tutorial_Grab");

        StartCoroutine(WaitForGrab());

        yield break;
    }

    IEnumerator WaitForGrab()
    {
        yield return new WaitUntil(() => massSphere.transform.parent != null && massSphere.transform.parent.GetComponent<HandController>() != null);

        // Push and pull tutorial
        controlsImage.sprite = pushPullSprite;
        instructions.text = pushPullString;

        player.GetComponentInChildren<UIManager>(true).updateCurrentObjective(instructions.text);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Tutorial\\Tutorial_Push&Pull");

        StartCoroutine(WaitForPushPull());

        yield break;
    }

    IEnumerator WaitForPushPull()
    {
        pushed = false;
        pulled = false;
        yield return new WaitUntil(() => pushed && pulled);

        // OLD: Controls Diagram and then finishing text
        //controlsImage.sprite = overSprite;
        //controlsImage.GetComponent<Image>().color = new Color32(255,255,255,0); // Makes image transparent, need to undone to controls image later
        //instructions.text = overText;
        //SceneUIContainer.SetActive(true);

        // Activate Menu
        controlsImage.sprite = menuSprite;
        instructions.text = openMenuString;

        player.GetComponentInChildren<UIManager>(true).updateCurrentObjective(instructions.text);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Tutorial\\Tutorial_Menu_Open");

        StartCoroutine(WaitForMenuPopup());

        yield break;
    }

    IEnumerator WaitForMenuPopup()
    {
        StartCoroutine(WaitForControlsScreenSelection());
        yield break;
    }

    IEnumerator WaitForControlsScreenSelection() 
    {
        //Waiting until player opens menu + explicit press of left menu control
        yield return new WaitUntil(() => openMenuReference && menus.activeInHierarchy == true);

        //controlsImage.sprite = overSprite;
        //controlsImage.GetComponent<Image>().color = new Color32(255,255,255,0); // Makes image transparent, need to undone to controls image later

        UIManagerScript.ActivateMenu(menus.transform.Find("ControlsMenu").gameObject); //Activate Menus and Buttons
        UIManagerScript.ActivateButton(buttons.transform.Find("ControlsButton").gameObject);

        AudioListener.pause = false; // Temporary fix to make the audio play when the game is in a paused state

        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Tutorial\\Tutorial_Menu_Forget_Controls");

        yield return new WaitForSecondsRealtime(4); // Set to the audio file above's duration in seconds 
        StartCoroutine(WaitForGeneralMenu());
        yield break;
    }

    IEnumerator WaitForGeneralMenu() 
    {
        //Waiting until player opens menu + explicit press of left menu control
        yield return new WaitUntil(() => openMenuReference && menus.activeInHierarchy == true);

        UIManagerScript.ActivateMenu(menus.transform.Find("GeneralMenu").gameObject); //Activate Menus and Buttons
        UIManagerScript.ActivateButton(buttons.transform.Find("GeneralButton").gameObject);

        AudioListener.pause = false; // Temporary fix to make the audio play when the game is in a paused state

        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Tutorial\\Tutorial_Menu_Options");

        yield return new WaitForSecondsRealtime(17); // Set to the audio file above's duration in seconds 
        StartCoroutine(WaitForSceneSelection());
        yield break;
    }

    IEnumerator WaitForSceneSelection()
    {
        //Waiting until player opens menu + explicit press of left menu control
        yield return new WaitUntil(() => openMenuReference && menus.activeInHierarchy == true);

        UIManagerScript.ActivateMenu(menus.transform.Find("ScenesMenu").gameObject); //Activate Menus and Buttons
        UIManagerScript.ActivateButton(buttons.transform.Find("ScenesButton").gameObject);

        AudioListener.pause = false; // Temporary fix to make the audio play when the game is in a paused state

        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Tutorial\\Tutorial_Menu_Scene_Select");

        yield return new WaitForSecondsRealtime(11); // Set to the audio file above's duration in seconds 

        instructions.text = overString;
        SceneUIContainer.SetActive(true);

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

    #region Localization helper methods
    // TODO: Investigate if there is a way for only one function to be made that can be applied to all
    private void UpdateTeleportationString(string s)
    {
        if (instructions.text == teleportationString)
        {
            instructions.text = s;
            player.GetComponentInChildren<UIManager>(true).updateCurrentObjective(instructions.text);
        }
        else
        {
            teleportationString = s;
        }
    }

    private void UpdateTurnString(string s)
    {
        if (instructions.text == turnString)
        {
            instructions.text = s;
            player.GetComponentInChildren<UIManager>(true).updateCurrentObjective(instructions.text);
        }
        else
        {
            turnString = s;
        }
    }

    private void UpdateGrabString(string s)
    {
        if (instructions.text == grabString)
        {
            instructions.text = s;
            player.GetComponentInChildren<UIManager>(true).updateCurrentObjective(instructions.text);
        }
        else
        {
            grabString = s;
        }
    }

    private void UpdatePushPullString(string s)
    {
        if (instructions.text == pushPullString)
        {
            instructions.text = s;
            player.GetComponentInChildren<UIManager>(true).updateCurrentObjective(instructions.text);
        }
        else
        {
            pushPullString = s;
        }
    }

    private void UpdateOverString(string s)
    {
        if (instructions.text == overString)
        {
            instructions.text = s;
            player.GetComponentInChildren<UIManager>(true).updateCurrentObjective(instructions.text);
        }
        else
        {
            overString = s;
        }
    }

    private void UpdateMenuString(string s)
    {
        if (instructions.text == menuString)
        {
            instructions.text = s;
            player.GetComponentInChildren<UIManager>(true).updateCurrentObjective(instructions.text);
        }
        else
        {
            menuString = s;
        }
    }

    private void UpdateOpenMenuString(string s)
    {
        if (instructions.text == openMenuString)
        {
            instructions.text = s;
            player.GetComponentInChildren<UIManager>(true).updateCurrentObjective(instructions.text);
        }
        else
        {
            openMenuString = s;
        }
    }
    #endregion
}
