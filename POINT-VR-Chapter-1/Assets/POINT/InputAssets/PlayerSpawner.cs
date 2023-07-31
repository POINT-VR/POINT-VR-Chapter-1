using UnityEngine;
public class PlayerSpawner : MonoBehaviour
{
    /// <summary>
    /// The player prefab to be instantiated when using a VR headset
    /// </summary>
    [SerializeField] GameObject player;
    /// <summary>
    /// The emulator prefab to be instantiated for testing
    /// </summary>
    [SerializeField] GameObject emulator;

    GameObject activePlayer = null;

    private void Awake()
    {
    #if !UNITY_EDITOR
        activePlayer = Instantiate(player, transform);
    #else
        activePlayer = Instantiate(emulator, transform);
    #endif
    }

    public void UIChangeScene(int scene)
    {
        if (activePlayer != null)
        {
            activePlayer.transform.GetChild(0).GetComponentInChildren<SceneController>().ChangeScene(scene);
        }
    }
}