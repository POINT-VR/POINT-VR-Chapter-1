using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// The script that manages pausing and bringing up the menu. Should be attached to the player.
/// </summary>
public class PauseControllerEmulator : MonoBehaviour
{
    /// <summary>
    /// The input reference for turning the menu on and off
    /// </summary>
    [SerializeField] InputActionReference toggleReference;
    /// <summary>
    /// The main camera
    /// </summary>
    [SerializeField] Transform mainCamera;
    /// <summary>
    /// The distance that the menu spawns away from the camera
    /// </summary>
    [SerializeField] float distanceFromCamera;
    /// <summary>
    /// The pause menu that is only visible while the game is paused
    /// </summary>
    [Header("Specific objects affected by pausing")]
    [SerializeField] GameObject menu;
    /// <summary>
    /// The UI container
    /// </summary>
    [SerializeField] GameObject uiContainer;
    /// <summary>
    /// The Gripnotice
    /// </summary>
    [SerializeField] GameObject gripNotice;
    /// <summary>
    /// Necessary for dropping objects on game pause
    /// </summary>
    [SerializeField] HandControllerEmulator rightHand;
    /// <summary>
    /// Necessary for resizing lasers on game pause
    /// </summary>
    [SerializeField] Transform laserRight;
    /// <summary>
    /// Set to reduce the visibility of the laser clipping through the menu
    /// </summary>
    [SerializeField] float reducedLaserSize;
    private bool gamePaused;
    private float laserSize;
    private GameObject[] disabledObjects;
    private void OnEnable()
    {
        toggleReference.action.Enable();
        toggleReference.action.started += Toggle;
        laserSize = laserRight.localScale.y;
        gamePaused = false;
    }

    private void Toggle(InputAction.CallbackContext ctx)
    {
        gripNotice.SetActive(false);
        rightHand.Release();
        laserRight.localScale = new Vector3(laserRight.localScale.x, gamePaused ? laserSize : reducedLaserSize, laserRight.localScale.z);
        laserRight.localPosition = new Vector3(laserRight.localPosition.x, laserRight.localPosition.y, gamePaused ? laserSize : reducedLaserSize);
        GameObject[] gameObjects = (GameObject[])FindObjectsOfType(typeof(GameObject));
        if (gamePaused)
        {
            gameObjects = disabledObjects;
        }
        else
        {
            disabledObjects = gameObjects;
        }
        foreach (GameObject g in gameObjects)
        {
            if (!g.CompareTag("Player"))
            {
                g.SetActive(gamePaused);
            }
        }
        gamePaused = !gamePaused;
        Time.timeScale = gamePaused ? 0.0f : 1.0f;
        AudioListener.pause = gamePaused;
        menu.SetActive(gamePaused);
        if (gamePaused)
        {
            uiContainer.transform.SetParent(transform.parent);
        }
        else
        {
            uiContainer.transform.SetParent(mainCamera);
            uiContainer.transform.SetPositionAndRotation(mainCamera.position + mainCamera.forward * distanceFromCamera, mainCamera.rotation);
        }
    }
    private void OnDisable()
    {
        toggleReference.action.Disable();
        toggleReference.action.started -= Toggle;
    }
    private void OnApplicationFocus(bool focus)
    {
#if UNITY_EDITOR // Running this in the Unity Editor will make debugging impossible
        return;
#endif
#pragma warning disable CS0162 // Unreachable code detected
        if (!focus && !gamePaused)
        {
            Toggle(new InputAction.CallbackContext());
        }
#pragma warning restore CS0162 // Unreachable code detected
    }
    public void Unpause()
    {
        if (gamePaused)
        {
            Toggle(new InputAction.CallbackContext());
        }
    }
}
