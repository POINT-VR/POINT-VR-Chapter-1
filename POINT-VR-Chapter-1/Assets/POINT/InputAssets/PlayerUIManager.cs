using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    //Some variables which could be controlled by sliders and all in the Scene Viewer
    [Header("References")]
    [SerializeField] private GameObject SceneUIContainer;
    [SerializeField] private Image controlsImage;
    [Header("Instructions Text")]
    [SerializeField] private string instructionText;
    /**[SerializeField] private string turnText;
    [SerializeField] private string grabText;
    [SerializeField] private string pushPullText;
    [SerializeField] private string overText;
    [SerializeField] private string menuText;**/
    [Header("Instructions Timer")]
    [SerializeField] private int waitPeriod;

    // Cache
    private TMP_Text instructions = null;
    private Camera currentCamera = null;
    private GameObject player = null;

    // Start is called before the first frame update
    void Start()
    {
        if (controlsImage != null)
        {
            instructions = controlsImage.GetComponentInChildren<TMP_Text>();//Fetches the instructions text object from the scene and stores in in the cache for reference
            controlsImage.gameObject.SetActive(false);//Disables the controls text container
        }
        instructions = controlsImage.GetComponentInChildren<TMP_Text>();//Redundant, see the declaration above
        StartCoroutine(WaitForPlayerSpawn());//starts the player spawn coroutine defined below
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentCamera != null)
        {
            this.transform.LookAt(currentCamera.transform);//syncs transform of UI container with the Active Camera (Player Camera)
            this.transform.Rotate(0, 180, 0);//syncs rotation of UI container with the Active Camera (Player Camera)
        }
    }

    IEnumerator WaitForPlayerSpawn()
    {
        yield return new WaitUntil(() => Camera.current != null);//waits until camera/player is spawned, then executes code below

        // Start menu initialization
        currentCamera = Camera.current;//fetches the Active Camera from the scene, then stores it in the cache for reference
        this.GetComponent<Canvas>().worldCamera = currentCamera;//sets Active Camera for UI objects to the current camera
        player = currentCamera.transform.parent.gameObject;//fetches the Player from the scene, then stores it in the cache for reference
        this.transform.SetParent(player.transform.parent, false);//sets the Player as the parent of the object this script is attached to



        StartCoroutine(WaitUntilTime());//starts the wait until time coroutine defined below

        yield break;
    }
    IEnumerator WaitUntilTime()
    {
        instructions.text = instructionText;//sets the text content of the text object to the instructionsText we set from the Properties tab in Scene Viewer
        controlsImage.gameObject.SetActive(true);//enable the UI container, making it visible
        yield return new WaitForSeconds(waitPeriod);//waits until the waitPeriod (in seconds in real time) is elapsed, then executes code below
        controlsImage.gameObject.SetActive(false);//disable the UI container, making it invisible
        yield break;
    }
}
