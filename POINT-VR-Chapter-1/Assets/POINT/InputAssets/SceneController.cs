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
    [Header("Toggles")]
    /// <summary>
    /// The menu that first appears telling the player about the game's controls.
    /// </summary>
    [SerializeField] BinaryToggle gripNotice;
    /// <summary>
    /// The toggle that controls the visibility of the floor
    /// </summary>
    [SerializeField] BinaryToggle floorToggle;
    /// <summary>
    /// The toggle that controls whether or not haptics are used
    /// </summary>
    [SerializeField] BinaryToggle hapticToggle;
    /// <summary>
    /// The toggle that controls whether or not the highlight manager is active
    /// </summary>
    [SerializeField] BinaryToggle highlightsToggle;
    /// <summary>
    /// When Instantiated: Get the player data. Communicate this data to all component arguments.
    /// </summary>
    private void Start()
    {
        GameManager.PlayerData data = GameManager.Instance.GetData();
        music.time = data.musicTime;
        gripNotice.IsOn = data.gripNoticeEnabled;
        functional.value = data.functionalVolume;
        aesthetic.value = data.aestheticVolume;
        uiManager.ActivateLanguageToggle(toggleBase.GetChild((int)GameManager.Instance.languageSelected).gameObject);
        music.mute = false;
        floorToggle.IsOn = data.floorRendered;
        hapticToggle.IsOn = data.hapticsEnabled;
        highlightsToggle.IsOn = data.highlightsEnabled;
        uiManager.gameObject.SetActive(false);
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
            gripNoticeEnabled = gripNotice.IsOn,
            functionalVolume = functional.value,
            aestheticVolume = aesthetic.value,
            floorRendered = floorToggle.IsOn,
            hapticsEnabled = hapticToggle.IsOn,
            highlightsEnabled = highlightsToggle.IsOn
        };
        GameManager.Instance.SetData(data);
        pause.Unpause();
        SceneManager.LoadScene(scene);
    }
}