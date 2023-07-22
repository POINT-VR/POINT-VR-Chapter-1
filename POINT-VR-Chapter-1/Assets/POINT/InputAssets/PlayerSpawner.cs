using UnityEngine;
public class PlayerSpawner : MonoBehaviour
{
    /// <summary>
    /// The player prefab to be instantiated when using a VR headset
    /// </summary>
    [SerializeField] GameObject player;
    /// <summary>
    /// The emulator prefab to be instantiated when using the unity editor
    /// </summary>
    [SerializeField] GameObject emulator;

    GameObject activePlayer = null;

    private void Awake()
    {
        if (Application.isEditor)
        {
            activePlayer = Instantiate(emulator, transform);
        } else
        {
            activePlayer = Instantiate(player, transform);
        }
    }

    public void UIChangeScene(int scene)
    {
        if (activePlayer != null)
        {
            activePlayer.transform.GetChild(0).GetComponentInChildren<SceneController>().ChangeScene(scene);
        }
    }
}