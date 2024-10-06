﻿using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class ConfDemo_part2_two_masses : MonoBehaviour
{
    // Serialized fields
    [Header("References")]
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject massSphere;
    [SerializeField] private GameObject SceneUIContainer;
    [SerializeField] private InputActionReference openMenuReference;
 
    [Header("Instructions Text")]
    [SerializeField] private string objective1;
    [SerializeField] private string objective2;
    [SerializeField] private string objective3;

    // Cache
    private TMP_Text instructions = null;
    private Camera currentCamera = null;
    private GameObject player = null;
    private GameObject menus = null;
    private GameObject buttons = null;
    private UIManager UIManagerScript = null;

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
        
        UIManagerScript.UpdateCurrentObjective(objective1); // Grid/Space Deformation Two Masses
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\5_strength_of_gravity_prop_mass_1");

        yield return new WaitForSecondsRealtime(60); // Do space curvature prediction

        UIManagerScript.UpdateCurrentObjective(objective2); // Time Deformation Two Masses
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\5_strength_of_gravity_prop_mass_2");
        yield return new WaitForSecondsRealtime(12);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\5_strength_of_gravity_prop_mass_3");

        yield return new WaitForSecondsRealtime(60); // Do time dialation prediction

        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\5_strength_of_gravity_prop_mass_4");
        yield return new WaitForSecondsRealtime(13);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\5_strength_of_gravity_prop_mass_5");
        yield return new WaitForSecondsRealtime(7);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\5_strength_of_gravity_prop_mass_6");
        yield return new WaitForSecondsRealtime(16);

        UIManagerScript.UpdateCurrentObjective(objective3); // Continue to next scene
        SceneUIContainer.SetActive(true); // Continue to next scene 
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene2\\5_strength_of_gravity_prop_mass_7");

        yield break;
    }
}
