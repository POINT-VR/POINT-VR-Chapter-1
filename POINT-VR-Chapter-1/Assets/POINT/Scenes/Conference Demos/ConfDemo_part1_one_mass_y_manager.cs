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

    private void Start()
    {
        SceneUIContainer.SetActive(false);
        snapRing.SetActive(false);
        StartCoroutine(WaitForPlayerSpawn());
        setOfDirectionalArrows.SetActive(false);
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

        // Layers for GameObjects
        int LayerIgnoreRayCast = LayerMask.NameToLayer("Ignore Raycast");
        int LayerGrip = LayerMask.NameToLayer("Grip");

        menus = menuScreens;
        buttons = HeaderButtons;

        // Get UI Manager UIManagerScript
        UIManagerScript = Menu.GetComponent<UIManager>();

        yield return new WaitForSecondsRealtime(1);
        /*
        UIManagerScript.updateCurrentObjective(objective1); // Grid/Space Deformation
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\1_intro_to_grids_and_clocks_1");
        yield return new WaitForSecondsRealtime(16);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\1_intro_to_grids_and_clocks_2");
        yield return new WaitForSecondsRealtime(7);

        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\2_observe_grid_curve_1");
        yield return new WaitForSecondsRealtime(60); // Pause to let player explore

        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_1");
        yield return new WaitForSecondsRealtime(10);
        */
        // Question Time for Player: What does that say about Gravity
        // TODO: Would be cool if after this question the sun sphere animated to the center of the grid. 
        UIManagerScript.updateCurrentObjective(objective2); 
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_2");
        yield return new WaitForSecondsRealtime(7);
        // move massSphere to center of grid
        massSphere.transform.position = new Vector3(4.5f, 2f, 4f);
        setOfDirectionalArrows.SetActive(true); 
        // set massSphere to uninteractable
        massSphere.layer = LayerIgnoreRayCast;
        // spawn six directional arrows
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_3");
        yield return new WaitForSecondsRealtime(2.5f);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_4");
        yield return new WaitForSecondsRealtime(2);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_5");
        //yield return new WaitForSecondsRealtime(6.1);
        // Complete objective
        StartCoroutine(CorrectArrowDirectionTask());
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_6");
        yield return new WaitForSecondsRealtime(1); // Nice Job

        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_7");
        yield return new WaitForSecondsRealtime(22);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_8");
        yield return new WaitForSecondsRealtime(5.5f);
        setOfDirectionalArrows.SetActive(false);
        massSphere.layer = LayerGrip; // End of Task, set sphere back to interactable

        UIManagerScript.updateCurrentObjective(objective3); // Time Deformation
        snapRing.SetActive(true);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\4_observe_time_dialation_1");
        yield return new WaitForSecondsRealtime(8);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\4_observe_time_dialation_2");

        yield return new WaitUntil(() => snapRing.GetComponentInChildren<Rigidbody>() != null);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\4_observe_time_dialation_3");

        UIManagerScript.updateCurrentObjective(objective4); // Continue to next scene
        SceneUIContainer.SetActive(true); // Continue to next scene 

        yield break;
    }

    public IEnumerator CorrectArrowDirectionTask()
    {
        GameObject arrow0 = setOfDirectionalArrows.transform.Find("Directional Arrow").gameObject;
        bool arrow0_state = arrow0.GetComponent<Directional_Arrow_Script>().Correct;

        GameObject arrow1 = setOfDirectionalArrows.transform.Find("Directional Arrow (1)").gameObject;
        arrow1.GetComponent<Directional_Arrow_Script>().test();
        arrow1.GetComponent<Directional_Arrow_Script>().test();
        bool arrow1_state = arrow1.GetComponent<Directional_Arrow_Script>().Correct;

        GameObject arrow2 = setOfDirectionalArrows.transform.Find("Directional Arrow (2)").gameObject;
        arrow2.GetComponent<Directional_Arrow_Script>().test();
        arrow2.GetComponent<Directional_Arrow_Script>().test();
        arrow2.GetComponent<Directional_Arrow_Script>().test();
        bool arrow2_state = arrow2.GetComponent<Directional_Arrow_Script>().Correct;

        GameObject arrow3 = setOfDirectionalArrows.transform.Find("Directional Arrow (3)").gameObject;
        arrow3.GetComponent<Directional_Arrow_Script>().test();
        Debug.Log(arrow3.GetComponent<Directional_Arrow_Script>().Correct);
        bool arrow3_state = arrow3.GetComponent<Directional_Arrow_Script>().Correct;

        GameObject arrow4 = setOfDirectionalArrows.transform.Find("Directional Arrow (4)").gameObject;
        arrow4.GetComponent<Directional_Arrow_Script>().test();
        arrow4.GetComponent<Directional_Arrow_Script>().test();
        arrow4.GetComponent<Directional_Arrow_Script>().test();
        bool arrow4_state = arrow4.GetComponent<Directional_Arrow_Script>().Correct;

        GameObject arrow5 = setOfDirectionalArrows.transform.Find("Directional Arrow (5)").gameObject;
        arrow5.GetComponent<Directional_Arrow_Script>().test();
        Debug.Log(arrow5.GetComponent<Directional_Arrow_Script>().Correct);
        bool arrow5_state = arrow5.GetComponent<Directional_Arrow_Script>().Correct;

        bool state = arrow0_state & arrow1_state & arrow2_state & arrow3_state & arrow4_state & arrow5_state;
        Debug.Log(state);

        if ( state == true)
            yield break;
    }
}
