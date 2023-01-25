using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
public class IntializeSpawn : MonoBehaviour
{
    /// <summary>
    /// Generic IntializeSpawn. Should work on any Mass Objects with a RigidBody.
    /// When sent a message "Intialize" will rest a Mass Object to some intial position.
    /// Meant to be used in combination with the UI collider script. 
    /// </summary>
    
    /// <summary>
    /// Mass object that the script is attached to
    /// </summary>
    public Rigidbody massObject;

    /// <summary>
    /// The initialPos the Mass Object will be reset to.
    /// </summary>
    public Vector3 intialPos;
    //[Header("Other Constants")]

    void Start()
    {
        massObject = GetComponent<Rigidbody>(); // This is the massobject that will experience gravity
    }

    void Intialize()
    {
        massObject.transform.position = intialPos;
        massObject.velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        
    }
}
