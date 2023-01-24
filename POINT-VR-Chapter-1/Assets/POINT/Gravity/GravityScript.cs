using UnityEngine;
/// <summary>
/// Generic GravityScipt. Should work on any Mass Objects with a Collider.
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class GravityScript : MonoBehaviour
{   
    /// <summary>
    /// We will get the masses from the Rigidbody components
    /// </summary>
    [SerializeField] Rigidbody[] rigidbodiesThatAttract;
    /// <summary>
    /// Mass object that the script is attached to
    /// </summary>
    private Rigidbody massObject;
    /// <summary>
    /// The coefficient of the magnitude of force
    /// </summary>
    [SerializeField] float power;

    void Start()
    {
        massObject = GetComponent<Rigidbody>(); // This is the massobject that will experience gravity
    }

    private void FixedUpdate()
    {
        Vector3 massPosition = transform.position;
        Vector3 force = Vector3.zero;
        for (int i = 0; i < rigidbodiesThatAttract.Length; i++)
        {
            Vector3 direction = rigidbodiesThatAttract[i].transform.position - massPosition; 
            // Newtonian Gravitational Force
            float mag = power*massObject.mass*rigidbodiesThatAttract[i].mass / direction.sqrMagnitude;
            force += mag * direction; //Displacement from each mass is calculated independently, but combined by vector addition   
        }
        // Move Mass Object
        massObject.AddForce(force / rigidbodiesThatAttract.Length, ForceMode.Force);
    }
}
