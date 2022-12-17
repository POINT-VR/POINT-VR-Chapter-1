using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
public class GravityScript : MonoBehaviour
{
    /// <summary>
    /// Generic GravityScipt. Should work on any Mass Objects with a Collider.
    /// </summary>

    /// <summary>
    /// We will get the masses from the Rigidbody components
    /// </summary>
    public Rigidbody[] rigidbodiesThatAttract;
    
    /// <summary>
    /// Mass object that the script is attached to
    /// </summary>
    public Rigidbody massObject;
    //[Header("Other Constants")]
    public float power = 1f;

    void Start()
    {
        massObject = GetComponent<Rigidbody>(); // This is the massobject that will experience gravity
    }

    void OnCollisionEnter(Collision coll)
    {
        massObject.velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        Vector3 massPosition = massObject.transform.position;

        Vector3[] otherMassPositions = new Vector3[rigidbodiesThatAttract.Length];

        Vector3 force = Vector3.zero;

        for (int i = 0; i < rigidbodiesThatAttract.Length; i++) //Puts the mass positions on the stack ahead of time
        {
            
            otherMassPositions[i] = rigidbodiesThatAttract[i].transform.position;
            Vector3 direction = otherMassPositions[i] - massPosition; 

            // Newtonian Gravitational Force
            float mag = power*massObject.mass*rigidbodiesThatAttract[i].mass / direction.sqrMagnitude;

            force += mag * direction / rigidbodiesThatAttract.Length; //Displacement from each mass is calculated independently, but combined by vector addition   
        }

        // Move Mass Object
        massObject.AddForce(force, ForceMode.Force);
    }
}
