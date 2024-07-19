using UnityEngine;
public class GameManager : MonoBehaviour
{
    public struct PlayerData
    {
        public float musicTime;
        public bool gripNoticeEnabled;
        public float functionalVolume;
        public float aestheticVolume;
        public bool isFloorVisible;
        public bool isHapticsEnabled;
        public bool isControllerHighlighted;
        public Language subtitleLanguage;
    }
    public static GameManager Instance { get; private set; }
    public enum Language { Disabled = 0, English = 1, Spanish = 2, French = 3, Mandarin = 4, Japanese = 5, Hindi = 6, Arabic = 7, Bengali = 8, Russian = 9, Portuguese = 10, Malay = 11, Urdu = 12, Telugu = 13, Korean = 14 };
    ///Language is stored separately. This variable can be modified directly.
    public Language subtitlesLanguage;
    PlayerData data;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            data.musicTime = 0;
            data.gripNoticeEnabled = false; //Potentionally remove in the future
            data.functionalVolume = 1.0f;
            data.aestheticVolume = 1.0f;
            data.isHapticsEnabled = true;
            data.isFloorVisible = false;
            data.isControllerHighlighted = true;
            data.subtitleLanguage = Language.English;
        }
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void SetData(PlayerData input)
    {
        data = input;
    }
    public PlayerData GetData()
    {
        return data;
    }
}