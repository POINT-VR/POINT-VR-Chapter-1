﻿using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
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
    [SerializeField] private GameObject task3Location;
    [SerializeField] private GameObject continue1;

    [Header("Instructions Text")]
    [SerializeField] private LocalizedString objective1Text;
    [SerializeField] private LocalizedString objective2Text;
    [SerializeField] private LocalizedString objective3Text;
    [SerializeField] private LocalizedString objective4Text;

    private string objective1string;
    private string objective2string;
    private string objective3string;
    private string objective4string;

    // Cache
    private Camera currentCamera = null;
    private GameObject player = null;
    private GameObject menus = null;
    private GameObject buttons = null;
    private UIManager UIManagerScript = null;
    private DirectionalArrow[] directionalArrows;
    private static bool objectiveContinue = false;

    private void Start()
    {
        SceneUIContainer.SetActive(false);
        snapRing.SetActive(false);
        StartCoroutine(WaitForPlayerSpawn());
        setOfDirectionalArrows.SetActive(false);
        directionalArrows = setOfDirectionalArrows.GetComponentsInChildren<DirectionalArrow>(true);
        task3Location.SetActive(false);
        continue1.SetActive(false);
    }

    private IEnumerator WaitForPlayerSpawn()
    {
        yield return new WaitUntil(() => Camera.current != null);

        // Start menu initialization
        currentCamera = Camera.current;
        player = currentCamera.transform.parent.gameObject;

        StartCoroutine(StartScene());
    }

    private IEnumerator StartScene()
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

        yield return new WaitForSeconds(1);
        UIManagerScript.UpdateCurrentObjective(objective1Text); // Grid/Space Deformation
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\1_intro_to_grids_and_clocks_1");
        yield return new WaitForSeconds(16);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\1_intro_to_grids_and_clocks_2");
        yield return new WaitForSeconds(7);

        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\2_observe_grid_curve_1");
        yield return new WaitForSeconds(20.4f); // Continue Button
        continue1.SetActive(true);
        Debug.Log("Press the 'continue' button when you are ready to move on."); 
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("continue");
        yield return new WaitUntil(() => ConfDemo_part1_one_mass_y_manager.objectiveContinue == true);
        continue1.SetActive(false);

        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\2_observe_grid_curve_2");
        yield return new WaitForSeconds(5.9f);

        task3Location.SetActive(true);
        yield return new WaitUntil(() => task3Location.transform.GetChild(1).gameObject.GetComponentInChildren<Rigidbody>() != null); // Reach SnapRing
        massSphere.transform.parent = null; // Make player drop the sphere
        massSphere.layer = LayerMask.NameToLayer("Ignore Raycast"); // set massSphere to uninteractable
        // Makes sphere stop moving
        massSphere.GetComponent<Rigidbody>().velocity = Vector3.zero; // move massSphere to center of grid
        massSphere.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; // freeze massSphere in place
        task3Location.SetActive(false);

        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_1");
        yield return new WaitForSeconds(7.6f);
        // Question Time for Player: What does that say about Gravity

        UIManagerScript.UpdateCurrentObjective(objective2Text); 
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_2");
        yield return new WaitForSeconds(7.1f);

        ArrowTask();

        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_3");
        yield return new WaitForSeconds(2.5f);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_4");
        yield return new WaitForSeconds(2);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_5");
        yield return new WaitForSeconds(6.1f);
    }

    private void ArrowTask()
    {
        setOfDirectionalArrows.SetActive(true);  // spawn six directional arrows
    }

    public void CheckArrowTask()
    {
        foreach (DirectionalArrow directionalArrow in directionalArrows)
        {
            if (!directionalArrow.IsCorrect) return;
        }

        StartCoroutine(StartScenePart2());
    }

    private IEnumerator StartScenePart2()
    {
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_6");
        yield return new WaitForSeconds(1); // Nice Job

        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_7");
        yield return new WaitForSeconds(22);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\3_direction_of_curvature_arrow_8");
        yield return new WaitForSeconds(5.5f);
        setOfDirectionalArrows.SetActive(false);
        massSphere.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None; // End of Task, set sphere back to movable
        massSphere.layer = LayerMask.NameToLayer("Grip"); // End of Task, set sphere back to interactable
        task3Location.SetActive(true); // Reactivate because this seems to break the second snapRing snaping ability
        task3Location.transform.GetChild(0).gameObject.SetActive(false);

        UIManagerScript.UpdateCurrentObjective(objective3Text); // Time Deformation
        snapRing.SetActive(true);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\4_observe_time_dialation_1");
        yield return new WaitForSeconds(8);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\4_observe_time_dialation_2");

        yield return new WaitUntil(() => snapRing.GetComponentInChildren<Rigidbody>() != null);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\4_observe_time_dialation_3");

        UIManagerScript.UpdateCurrentObjective(objective4Text); // Continue to next scene
        SceneUIContainer.SetActive(true); // Continue to next scene 

        yield break;
    }

    public void ContinueObjective() {
        objectiveContinue = true;
    }
}
