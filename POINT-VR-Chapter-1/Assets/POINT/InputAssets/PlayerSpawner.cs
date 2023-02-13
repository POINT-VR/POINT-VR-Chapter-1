using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerSpawner : MonoBehaviour
{
    /// <summary>
    /// The player prefab to be instantiated
    /// </summary>
    [SerializeField] GameObject player;
    private void Awake()
    {
        Instantiate(player, transform);
    }
    public void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
