using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class ConfDemo_part1_one_mass_y_manager : MonoBehaviour
{
    // Serialized fields
    [Header("References")]
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject massSphere;
    [SerializeField] private GameObject SceneUIContainer;
    [SerializeField] private GameObject snapRing;
    [SerializeField] private InputActionReference openMenuReference;

    [Header("Instructions Text")]
    [SerializeField] private string objective1;
    [SerializeField] private string objective2;
    [SerializeField] private string objective3;
    
    // Cache
    private Camera currentCamera = null;
    private GameObject player = null;
    private GameObject menus = null;
    private GameObject buttons = null;
    private UIManager UIManagerScript = null;

    private void Start()
    {
        SceneUIContainer.SetActive(false);
        snapRing.SetActive(false);
        StartCoroutine(WaitForPlayerSpawn());
    }

    private void Update()
    {
    }

    IEnumerator WaitForPlayerSpawn()
    {
        yield return new WaitUntil(() => Camera.current != null);

        // Start menu initialization
        currentCamera = Camera.current;
        player = currentCamera.transform.parent.gameObject;

        StartCoroutine(StartScene());
    }

    IEnumerator StartScene()
    {
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

        yield return new WaitForSecondsRealtime(1);
        
        UIManagerScript.updateCurrentObjective(objective1); // Grid/Space Deformation
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\1_intro_to_grids_and_clocks_1");
        yield return new WaitForSecondsRealtime(16);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\1_intro_to_grids_and_clocks_2");
        yield return new WaitForSecondsRealtime(7);

        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\2_observe_grid_curve_1");
        yield return new WaitForSecondsRealtime(60); // Pause to let player explore

        UIManagerScript.updateCurrentObjective(objective2); // Time Deformation
        snapRing.SetActive(true);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\4_observe_time_dialation_1");
        yield return new WaitForSecondsRealtime(8);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\4_observe_time_dialation_2");

        yield return new WaitUntil(() => snapRing.GetComponentInChildren<Rigidbody>() != null);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\4_observe_time_dialation_3");

        UIManagerScript.updateCurrentObjective(objective3); // Continue to next scene
        SceneUIContainer.SetActive(true); // Continue to next scene 

        yield break;
    }
}
