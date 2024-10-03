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
    [SerializeField] private GameObject setOfDirectionalArrows;

    [Header("Instructions Text")]
    [SerializeField] private string objective1;
    [SerializeField] private string objective2;
    [SerializeField] private string objective3;
    [SerializeField] private string objective4;
    
    // Cache
    private Camera currentCamera = null;
    private GameObject player = null;
    private GameObject menus = null;
    private GameObject buttons = null;
    private UIManager UIManagerScript = null;

    private GameObject arrow0 = null;
    private GameObject arrow1 = null;
    private GameObject arrow2 = null;
    private GameObject arrow3 = null;
    private GameObject arrow4 = null;
    private GameObject arrow5 = null;
    private bool arrow0_state = false;
    private bool arrow1_state = false;
    private bool arrow2_state = false;
    private bool arrow3_state = false;
    private bool arrow4_state = false;
    private bool arrow5_state = false;
    private bool state = false;
    private bool arrowTask = false;

    private void Start()
    {
        SceneUIContainer.SetActive(false);
        snapRing.SetActive(false);
        StartCoroutine(WaitForPlayerSpawn());
        setOfDirectionalArrows.SetActive(false);

        arrow0 = setOfDirectionalArrows.transform.Find("Directional Arrow").gameObject;
        arrow1 = setOfDirectionalArrows.transform.Find("Directional Arrow (1)").gameObject;
        arrow2 = setOfDirectionalArrows.transform.Find("Directional Arrow (2)").gameObject;
        arrow3 = setOfDirectionalArrows.transform.Find("Directional Arrow (3)").gameObject;
        arrow4 = setOfDirectionalArrows.transform.Find("Directional Arrow (4)").gameObject;
        arrow5 = setOfDirectionalArrows.transform.Find("Directional Arrow (5)").gameObject;
    }

    private void Update()
    {   
        /*
        // TODO: Maybe move all these check to a Coroutine so it isn't always checking the state of these arrows
        // Tried a version of a Coroutine, but could not get 
        // yield return new WaitUntil(() => state != false) to work properly
        */
        arrow0_state = arrow0.GetComponent<Directional_Arrow_Script>().Correct;
        arrow1_state = arrow1.GetComponent<Directional_Arrow_Script>().Correct;
        arrow2_state = arrow2.GetComponent<Directional_Arrow_Script>().Correct;
        arrow3_state = arrow3.GetComponent<Directional_Arrow_Script>().Correct;
        arrow4_state = arrow4.GetComponent<Directional_Arrow_Script>().Correct;
        arrow5_state = arrow5.GetComponent<Directional_Arrow_Script>().Correct;
        state = arrow0_state & arrow1_state & arrow2_state & arrow3_state & arrow4_state & arrow5_state;

        // TODO: Also move this to the ArrowDirectionTask Coroutine
        if (arrowTask == true)
        {
            massSphere.GetComponent<Rigidbody>().velocity = Vector3.zero; // move massSphere to center of grid
            massSphere.transform.position = new Vector3(4.5f, 2f, 4f); // move massSphere to center of grid
        }
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

        // Layers for GameObjects
        int LayerIgnoreRayCast = LayerMask.NameToLayer("Ignore Raycast");
        int LayerGrip = LayerMask.NameToLayer("Grip");

        menus = menuScreens;
        buttons = HeaderButtons;

        // Get UI Manager UIManagerScript
        UIManagerScript = Menu.GetComponent<UIManager>();

        UIManagerScript.updateCurrentObjective(objective1); // Grid/Space Deformation
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\1_intro_to_grids_and_clocks_1");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\1_intro_to_grids_and_clocks_2");

        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\2_observe_grid_curve_1");

        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_1");
        // Question Time for Player: What does that say about Gravity

        UIManagerScript.updateCurrentObjective(objective2); 
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_2");
        
        // TODO: Would be cool if after this question the sun sphere animated to the center of the grid. 
        // Instead of just spawing at the center of the grid
        massSphere.transform.parent = null; // Make player drop the sphere
        arrowTask = true; // See Update function, makes sphere stop moving and locates it to the center of the grid
        setOfDirectionalArrows.SetActive(true);  // spawn six directional arrows
        massSphere.layer = LayerIgnoreRayCast; // set massSphere to uninteractable
                
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_3");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_4");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_5");
        yield return new WaitForSeconds(6.1f);
        
        yield return new WaitUntil(() => state != false); // Complete objective
        
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_6");

        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_7");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_8");
        setOfDirectionalArrows.SetActive(false);
        massSphere.layer = LayerGrip; // End of Task, set sphere back to interactable
        arrowTask = false;

        UIManagerScript.updateCurrentObjective(objective3); // Time Deformation
        snapRing.SetActive(true);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\4_observe_time_dialation_1");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\4_observe_time_dialation_2");

        yield return new WaitUntil(() => snapRing.GetComponentInChildren<Rigidbody>() != null);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\4_observe_time_dialation_3");

        UIManagerScript.updateCurrentObjective(objective4); // Continue to next scene
        SceneUIContainer.SetActive(true); // Continue to next scene 

        yield break;
    }
}
