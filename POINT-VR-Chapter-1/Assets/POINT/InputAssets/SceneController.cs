using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneController : MonoBehaviour
{
    [SerializeField] AudioSource music;
    [SerializeField] GameObject gripNotice;
    [SerializeField] Slider functional;
    [SerializeField] Slider aesthetic;
    private void Awake()
    {
        GameManager.PlayerData data = GameManager.Instance.GetData();
        music.time = data.musicTime;
        gripNotice.SetActive(data.gripNoticeEnabled);
        functional.value = data.functionalVolume;
        aesthetic.value = data.aestheticVolume;
    }
    public void ChangeScene(int scene)
    {
        GameManager.PlayerData data;
        data.musicTime = music.time;
        data.gripNoticeEnabled = gripNotice.activeSelf;
        data.functionalVolume = functional.value;
        data.aestheticVolume = aesthetic.value;
        GameManager.Instance.SetData(data);
        SceneManager.LoadScene(scene);
    }
}
