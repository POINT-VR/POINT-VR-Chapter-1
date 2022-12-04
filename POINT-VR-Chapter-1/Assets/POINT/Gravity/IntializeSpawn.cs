using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
public class IntializeSpawn : MonoBehaviour
{
    /// <summary>
    /// Generic GravityScipt. Should work on any Mass Objects with a Collider.
    /// </summary>
    
    /// <summary>
    /// Mass object that the script is attached to
    /// </summary>
    public Rigidbody massObject;
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
