using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.InputSystem;

public class ConfDemo_part3_ranking_masses : MonoBehaviour
{
    // Serialized fields
    [Header("References")]
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject massSphere;
    [SerializeField] private GameObject SceneUIContainer;
    [SerializeField] private GameObject TaskUIContainer;
    [SerializeField] private GameObject Task2UIContainer;
    [SerializeField] private InputActionReference openMenuReference;

    [Header("Instructions Text")]
    [SerializeField] private LocalizedString objective1;
    [SerializeField] private LocalizedString objective2;
    [SerializeField] private LocalizedString objective3;

    // Cache
    private TMP_Text instructions = null;
    private Camera mainCamera = null;
    private GameObject player = null;
    private UIManager UIManagerScript = null;

    private bool isTask2Answered = false;

    private void Start()
    {
        SceneUIContainer.SetActive(false);
        StartCoroutine(WaitForPlayerSpawn());
    }

    private void Update()
    {
    }

    IEnumerator WaitForPlayerSpawn()
    {
        yield return new WaitUntil(() => Camera.main != null);

        // Start menu initialization
        mainCamera = Camera.main;
        //this.GetComponent<Canvas>().worldCamera = mainCamera;
        player = mainCamera.transform.parent.gameObject;

        StartCoroutine(StartScene());
        yield break;
    }

    IEnumerator StartScene()
    {
        //Instantiate menus from player prefab and buttons from player prefab as well
        GameObject mainCamera = player.transform.Find("Main Camera").gameObject;
        GameObject UIContainer = mainCamera.transform.Find("UI Container").gameObject;
        GameObject Menu = UIContainer.transform.Find("Menu").gameObject;

        // Get UI Manager UIManagerScript
        UIManagerScript = Menu.GetComponent<UIManager>();

        yield return new WaitForSecondsRealtime(1);
        
        UIManagerScript.UpdateCurrentObjective(objective1); // Rank different masses
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\6_put_masses_in_order_1");

        // Do ranking, see SortHolder.cs
        // Audio: Nice Job, continue
        // or Audio: Try Again

        yield break;
    }

    public IEnumerator RadiiTaskAudio()
    {
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\7_radius_activity_same_mass_1");
        yield return new WaitForSecondsRealtime(13);
        if (!isTask2Answered)
        {
            player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\7_radius_activity_same_mass_2");
        }
        yield break;
    }

    public void PlayRadiiTask()
    {
        // Called in the OnCast() in the Task1 UI -> Next Task Button, once they click to continue to Task 2
        UIManagerScript.UpdateCurrentObjective(objective2); // Rank different radii, same mass
        StartCoroutine(RadiiTaskAudio());
    }

    public void PlayEndRadiiTaskWrong()
    {
        // Called in the OnCast() of Task2 UI -> Big, wrong answer try again
        isTask2Answered = true;
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\6_put_masses_in_order_3");
    }

    public IEnumerator EndRadiiAudio()
    {
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\7_radius_activity_same_mass_3");
        UIManagerScript.UpdateCurrentObjective(objective3); // Congrats, all done
        SceneUIContainer.SetActive(true);

        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("temporary_ending_1");
        yield return new WaitForSeconds(15.2f);

        SceneController sceneController = player.GetComponentInChildren<SceneController>(); // Automatically go to Credits
        if (sceneController != null)
            {
                sceneController.ChangeScene(6);
            }
        yield break;
    }

    public void PlayEndRadiiTask()
    {
        // Called in the OnCast() of Task2 UI -> Small, Right answer
        isTask2Answered = true;
        StartCoroutine(EndRadiiAudio());        
    }
}
