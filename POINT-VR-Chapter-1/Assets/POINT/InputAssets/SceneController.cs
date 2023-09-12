using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// This script should be on the player prefab. It communicates the player's setting to the GameManager before switching scenes and then retrieves this information upon being instantiated.
/// </summary>
public class SceneController : MonoBehaviour
{
    /// <summary>
    /// The audiosource corresponding to the game's music. This should be on the player's head.
    /// </summary>
    [SerializeField] AudioSource music;
    /// <summary>
    /// The menu that first appears telling the player about the game's controls.
    /// </summary>
    [SerializeField] GameObject gripNotice;
    /// <summary>
    /// The UI slider for 'functional' audio
    /// </summary>
    [SerializeField] Slider functional;
    /// <summary>
    /// The UI slider for 'aesthetic' audio
    /// </summary>
    [SerializeField] Slider aesthetic;
    /// <summary>
    /// The class responsible for the player prefab's UI controls
    /// </summary>
    [SerializeField] UIManager uiManager;
    /// <summary>
    /// The parent transform of the language select toggle gameobjects
    /// </summary>
    [SerializeField] Transform toggleBase;
    /// <summary>
    /// The script that pauses the game
    /// </summary>
    [SerializeField] PauseController pause;
    /// <summary>
    /// The hardware controllers for the left hand. Used to remember whether haptics are enabled.
    /// </summary>
    [SerializeField] XRHardwareController leftHand;
    /// <summary>
    /// The hardware controllers for the right hand. Used to remember whether haptics are enabled.
    /// </summary>
    [SerializeField] XRHardwareController rightHand;
    // The actual floor visiblilty is tracked by the UI Manager, but this variable is for inter-scene communication relating to floor visibility
    private bool floorVisible;
    /// <summary>
    /// Setter called by the UI components that alters the bookkeeping of floor visibility
    /// </summary>
    /// <param name="enabled">Whether or not the floor is visible</param>
    public bool FloorVisibility { set { floorVisible = value; } }
    /// <summary>
    /// When Instantiated: Get the player data. Communicate this data to all component arguments.
    /// </summary>
    private void Start()
    {
        GameManager.PlayerData data = GameManager.Instance.GetData();
        music.time = data.musicTime;
        gripNotice.SetActive(data.gripNoticeEnabled);
        functional.value = data.functionalVolume;
        aesthetic.value = data.aestheticVolume;
        uiManager.ActivateLanguageToggle(toggleBase.GetChild((int)GameManager.Instance.languageSelected).gameObject);
        music.mute = false;
        floorVisible = data.floorRendered;
        uiManager.ActivateFloorToggle(floorVisible);
        leftHand.hapticsEnabled = data.hapticsEnabled;
        rightHand.hapticsEnabled = data.hapticsEnabled;
    }
    /// <summary>
    /// Saves the player data to the GameManager and loads a new scene
    /// </summary>
    /// <param name="scene">The scene index as assigned in the build settings</param>
    public void ChangeScene(int scene)
    {
        GameManager.PlayerData data = new GameManager.PlayerData
        {
            musicTime = music.time,
            gripNoticeEnabled = gripNotice.activeSelf,
            functionalVolume = functional.value,
            aestheticVolume = aesthetic.value,
            floorRendered = floorVisible,
            hapticsEnabled = leftHand.hapticsEnabled
        };
        GameManager.Instance.SetData(data);
        pause.Unpause();
        SceneManager.LoadScene(scene);
    }
}