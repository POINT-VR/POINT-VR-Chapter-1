using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Stores integer values under scene names. Do not change these intergers (you should be changing the SceneDict dictionary below). Integers are linked to build settings scene integers in the dictionary SceneDict (defined in SceneController).
/// There are two layers of this (enum here, and the dict down below) because the Unity Editor stores the value of the enum, not its name. So if the build settings change and you change the enum in the code then what the editor stores
/// won't actually change. 
/// </summary>
public enum SceneNumEnum
{
    StartMenu = 114,
    Tutorial = 97,
    Ch1_Scene1 = 115,
    Conf_Demo_P1 = 116,
    Conf_Demo_P2 = 108,
    Conf_Demo_P3 = 101,
    EndCredits = 121
}

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
    [SerializeField] BinaryToggle gripNotice; //Potentionally remove in the future
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

    /// <summary>
    /// Relates the shown enum (left) to scene numbers in build settings. If the scene numbers in build settings change you should update
    /// the right side of the dictionary (the values) with the new numbers. 
    /// </summary>
    private Dictionary<int, int> SceneDict = new Dictionary<int, int>
    {
        { (int) SceneNumEnum.StartMenu, 0 },
        { (int) SceneNumEnum.Tutorial, 1 },
        { (int) SceneNumEnum.Ch1_Scene1, 2 },
        { (int) SceneNumEnum.Conf_Demo_P1, 3 },
        { (int) SceneNumEnum.Conf_Demo_P2, 4 },
        { (int) SceneNumEnum.Conf_Demo_P3, 5 },
        { (int) SceneNumEnum.EndCredits, 6 }
    };
    private void Start()
    {
        GameManager.PlayerData data = GameManager.Instance.GetData();
        music.time = data.musicTime;
        gripNotice.IsOn = data.gripNoticeEnabled;
        functional.value = data.functionalVolume;
        aesthetic.value = data.aestheticVolume;
        uiManager.Language = (int)data.language;
        uiManager.SubtitleLanguage = (int)data.subtitleLanguage;
        music.mute = false;
        floorToggle.IsOn = data.isFloorVisible;
        hapticToggle.IsOn = data.isHapticsEnabled;
        highlightsToggle.IsOn = data.isControllerHighlighted;
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
            isFloorVisible = floorToggle.IsOn,
            isHapticsEnabled = hapticToggle.IsOn,
            isControllerHighlighted = highlightsToggle.IsOn,
            language = (GameManager.Language)uiManager.Language,
            subtitleLanguage = (GameManager.Language)uiManager.SubtitleLanguage
        };
        GameManager.Instance.SetData(data);
        pause.Unpause();
        SceneManager.LoadScene(scene);
    }

    // Using a function here so the dictionary can't be modified and we don't have to bother passing larger object (I lack C# knowledge; there may be a better way to do this)
    public int OperateSceneDict(int SceneNumEnum_number)
    {
        return SceneDict[SceneNumEnum_number];
    }
}
