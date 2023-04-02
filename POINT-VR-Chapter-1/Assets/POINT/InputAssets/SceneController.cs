using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneController : MonoBehaviour
{
    [SerializeField] AudioSource music;
    [SerializeField] GameObject gripNotice;
    [SerializeField] Slider functional;
    [SerializeField] Slider aesthetic;
    [SerializeField] UIManager uiManager;
    [SerializeField] Transform toggleBase;
    [SerializeField] PauseController pause;
    private bool floorEnabled;
    private void Start()
    {
        GameManager.PlayerData data = GameManager.Instance.GetData();
        music.time = data.musicTime;
        gripNotice.SetActive(data.gripNoticeEnabled);
        functional.value = data.functionalVolume;
        aesthetic.value = data.aestheticVolume;
        uiManager.ActivateLanguageToggle(toggleBase.GetChild((int)GameManager.Instance.languageSelected).gameObject);
        music.mute = false;
        floorEnabled = data.floorRendered;
        uiManager.ActivateFloorToggle(floorEnabled);
    }
    public void ChangeScene(int scene)
    {
        GameManager.PlayerData data = new GameManager.PlayerData
        {
            musicTime = music.time,
            gripNoticeEnabled = gripNotice.activeSelf,
            functionalVolume = functional.value,
            aestheticVolume = aesthetic.value,
            floorRendered = floorEnabled
        };
        GameManager.Instance.SetData(data);
        pause.Unpause();
        SceneManager.LoadScene(scene);
    }
    public void UpdateFloor(bool enabled)
    {
        floorEnabled = enabled;
    }
}