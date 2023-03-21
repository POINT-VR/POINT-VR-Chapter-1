using UnityEngine;
public class PlayerSpawner : MonoBehaviour
{
    /// <summary>
    /// The player prefab to be instantiated
    /// </summary>
    [SerializeField] GameObject player;

    GameObject activePlayer = null;

    private void Awake()
    {
        activePlayer = Instantiate(player, transform);
    }

    public void UIChangeScene(int scene)
    {
        if (activePlayer != null)
        {
            activePlayer.transform.GetChild(0).GetComponentInChildren<SceneController>().ChangeScene(scene);
        }
    }
}