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

        // Add rigidbody and collider to prevent player from going out of bounds
        Rigidbody rb = this.gameObject.AddComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Smoother collision detection
        rb.drag = Mathf.Infinity; // Max. drag to prevent sliding against boundary walls
        this.gameObject.AddComponent<SphereCollider>();
    }

    public void UIChangeScene(int scene)
    {
        if (activePlayer != null)
        {
            activePlayer.transform.GetChild(0).GetComponentInChildren<SceneController>().ChangeScene(scene);
        }
    }
}