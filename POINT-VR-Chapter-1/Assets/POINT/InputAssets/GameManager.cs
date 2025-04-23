using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    const string SAVED_SETTINGS_FILE_NAME = "gameSettings.json";

    [System.Serializable]
    public struct PlayerData
    {
        public float functionalVolume;
        public float aestheticVolume;
        public bool isFloorVisible;
        public bool isHapticsEnabled;
        public bool isControllerHighlighted;
        public Language language;
        public Language subtitleLanguage;
    }
    public static GameManager Instance { get; private set; }

    #region Field setters
    public float FunctionalVolume
    {
        set
        {
            data.functionalVolume = value;
            SaveData();
        }
    }

    public float AestheticVolume
    {
        set
        {
            data.aestheticVolume = value;
            SaveData();
        }
    }

    public bool IsFloorVisible
    {
        set
        {
            data.isFloorVisible = value;
            SaveData();
        }
    }

    public bool IsHapticsEnabled
    {
        set
        {
            data.isHapticsEnabled = value;
            SaveData();
        }
    }

    public bool IsControllerHighlighted
    {
        set
        {
            data.isControllerHighlighted = value;
            SaveData();
        }
    }

    public Language GameLanguage
    {
        set
        {
            data.language = value;
            SaveData();
        }
    }

    public Language SubtitleLanguage
    {
        set
        {
            data.subtitleLanguage = value;
            SaveData();
        }
    }
    #endregion

    public enum Language { Disabled = 0, English = 1, Spanish = 2, French = 3, Mandarin = 4, Japanese = 5, Hindi = 6, Arabic = 7, Bengali = 8, Russian = 9, Portuguese = 10, Malay = 11, Urdu = 12, Telugu = 13, Korean = 14 };
    
    private PlayerData data;
    public PlayerData Data
    {
        get
        {
            return data;
        }
    }

    private float musicTime = 0.0f;
    public float MusicTime
    {
        get
        {
            return musicTime;
        }

        set
        {
            musicTime = value;
        }
    }
    
    private string settingsFilePath = null;

    private void Awake()
    {
        settingsFilePath = Application.persistentDataPath + "/" + SAVED_SETTINGS_FILE_NAME;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            if (File.Exists(settingsFilePath))
            {
                // Read settings from JSON file
                string settings = File.ReadAllText(settingsFilePath);
                data = JsonUtility.FromJson<PlayerData>(settings);
            }
            else
            {
                // Default settings
                data.functionalVolume = 1.0f;
                data.aestheticVolume = 1.0f;
                data.isHapticsEnabled = true;
                data.isFloorVisible = false;
                data.isControllerHighlighted = true;
                data.language = Language.English;
                data.subtitleLanguage = Language.English;

                // Create JSON file
                string jsonString = JsonUtility.ToJson(data);
                File.WriteAllText(settingsFilePath, jsonString);
            }
        } else
        {
            Debug.Log(Instance);
        }

        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void SaveData()
    {
        string jsonString = JsonUtility.ToJson(data);
        File.WriteAllText(settingsFilePath, jsonString);
    }
}