using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// The script that manages pausing and bringing up the menu. Should be attached to the player.
/// </summary>
public class PauseController : MonoBehaviour
{
    /// <summary>
    /// The input reference for turning the menu on and off
    /// </summary>
    [SerializeField] InputActionReference toggleReference;
    /// <summary>
    /// The distance that the menu spawns away from the camera
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
    [SerializeField] HandController leftHand, rightHand;
    /// <summary>
    /// Necessary for resizing lasers on game pause
    /// </summary>
    [SerializeField] Transform laserRight, laserLeft;
    /// <summary>
    /// Set to reduce the visibility of the laser clipping through the menu
    /// </summary>
    [SerializeField] float reducedLaserSize;
    private bool gamePaused;
    private float laserSize;
    private GameObject[] disabledObjects;
    private void Awake()
    {
        toggleReference.action.Enable();
        toggleReference.action.started += Toggle;
        laserSize = laserLeft.localScale.y;
        gamePaused = false;
    }

    private void Toggle(InputAction.CallbackContext ctx)
    {
        gripNotice.SetActive(false);
        rightHand.Release();
        leftHand.Release();
        laserLeft.localScale = new Vector3(laserLeft.localScale.x, gamePaused ? laserSize : reducedLaserSize, laserLeft.localScale.z);
        laserRight.localScale = laserLeft.localScale;
        laserLeft.localPosition = new Vector3(laserLeft.localPosition.x, laserLeft.localPosition.y, gamePaused ? laserSize : reducedLaserSize);
        laserRight.localPosition = laserLeft.localPosition;
        GameObject[] gameObjects = (GameObject[]) FindObjectsOfType(typeof(GameObject));
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
        menu.SetActive(gamePaused);
        if (gamePaused)
        {
            uiContainer.transform.SetParent(transform);
        }
        else
        {
            uiContainer.transform.SetParent(mainCamera);
            uiContainer.transform.SetPositionAndRotation(mainCamera.position + mainCamera.forward * distanceFromCamera, mainCamera.rotation);
        }
    }
    private void OnDestroy()
    {
        toggleReference.action.Disable();
        toggleReference.action.started -= Toggle;
    }

}
