using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
public class MeshDeformScript : MonoBehaviour
{
    /// <summary>
    /// Generic GravityScipt. Should work on any Mass Objects with a Collider.
    /// </summary>

    /// <summary>
    /// We will get the masses from the Rigidbody components
    /// </summary>
    public Rigidbody[] rigidbodiesToDeformAround;
    [Header("Other Constants")]

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3[] massPositions = new Vector3[rigidbodiesToDeformAround.Length];
        for (int i = 0; i < rigidbodiesToDeformAround.Length; i++) //Puts the mass positions on the stack ahead of time
        {
            massPositions[i] = rigidbodiesToDeformAround[i].transform.position;

            for (int j = 0; j < rigidbodiesToDeformAround.Length; j++) // Gets the Forces acting on a mass object i
            {
                if (i == j) // No self force for mass object
                {
                    continue;
                }

                

            }

            // Move Mass Object i

        }
    }
}
