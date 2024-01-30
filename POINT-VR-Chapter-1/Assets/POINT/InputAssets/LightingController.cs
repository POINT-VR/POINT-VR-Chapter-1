using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject directionalLight;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn a directional light at a fixed place above the player.
        GameObject lightGameObject = Instantiate(directionalLight);
        lightGameObject.transform.SetParent(transform); // currently attached to player, don't know why player.transform raises an error
        lightGameObject.transform.position = new Vector3(0, 5, 0);
        lightGameObject.transform.rotation = Quaternion.identity;
        lightGameObject.transform.Rotate(45, 0, 0, Space.World);
    }
}
