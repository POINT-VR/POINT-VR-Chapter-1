using UnityEngine;
public class GameManager : MonoBehaviour
{
    public struct PlayerData
    {
        public float musicTime;
        public bool gripNoticeEnabled;
        public float functionalVolume;
        public float aestheticVolume;
    }
    public static GameManager Instance { get; private set; }
    public enum Language { Disabled, English };
    ///Language is stored separately. This variable can be modified directly.
    public Language languageSelected;
    PlayerData data;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            data.musicTime = 0;
            data.gripNoticeEnabled = true;
            data.functionalVolume = 1;
            data.aestheticVolume = 1;
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