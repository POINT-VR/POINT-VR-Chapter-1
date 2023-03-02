using UnityEngine;
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
}