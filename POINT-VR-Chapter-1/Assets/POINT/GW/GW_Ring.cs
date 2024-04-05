using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GW_Ring : MonoBehaviour
{
    
    [SerializeField] private GameObject SphereMesh;
    [SerializeField] public int numberOfMeshes = 12;
    //[SerializeField] public float radius = 5.0f;
    [SerializeField] private GameObject ring;
    [SerializeField] public float PercentOfPlusMode;
    [SerializeField] public float PercentOfCrossMode;
    [SerializeField] public float PercentOfBreathingMode;
    [SerializeField] public float PercentOfLongitudinalMode;
    [SerializeField] public float PercentOfXMode;
    [SerializeField] public float PercentOfYMode;

    [SerializeField] public float phase;
    [SerializeField] public float ampIndex;

    private List<GameObject> sphere_array;
    private List<float> angles_array = new List<float>();
    private List<Vector3> sphere_pos_array = new List<Vector3>();
    private GW_GravityScript gw_gravity;

    private bool doneSpawningSpheres = false;

    public float centralZ = -9999;
    private float ringRadius;

    //public float phase;


    // Start is called before the first frame update
    void Start()
    {

        //sphere_array = new GameObject[numberOfMeshes];

        gw_gravity = GetComponent<GW_GravityScript>();
        
        //Initialize mesh and angles array, to create our coordinate system which we use to translate particles
        //Redundant, somehow does not work if enabled
        //sphere_array = new List<GameObject>(numberOfMeshes);
        //angles_array = new List<float>(numberOfMeshes);
        //sphere_pos_array = new List<Vector3>(numberOfMeshes);

        //SpawnCircle();

        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(angles_array.Count);
    }

    private void FixedUpdate()
    {
        //Sanity check
        if (doneSpawningSpheres)
        {
            for (int k = 0; k < numberOfMeshes; k++)
            {

                Vector3 pos;
                //Debug.Log(centralZ);
                //Vector3 pos =  PercentOfCrossMode * gw_gravity.MoveCrossMode(sphere_pos_array[i], ring.transform.position, angles_array[i]);

                //Gives coordinates of oscillations of particles based on plus mode and cross mode polarizations of gravitational waves, can be controlled
                //Phasor addition of Plus and Cross Mode polarization - based oscillations used

                //If we assigned a central z (sphere), use algorithm for fixed oscillations in longitudinal/x/y modes
                if (centralZ > -999)
                {
                    pos = gw_gravity.CalculateOscillations(sphere_pos_array[k], ring.transform.position, ringRadius, phase, centralZ, PercentOfPlusMode, PercentOfCrossMode, PercentOfBreathingMode, PercentOfLongitudinalMode, PercentOfXMode, PercentOfYMode);
                }
                //We didn't assign a central z (Tube), use algorithm for propagating oscillations in longitudinal/x/y modes
                else
                {
                    pos = gw_gravity.CalculateOscillations(sphere_pos_array[k], ring.transform.position,true, phase, ampIndex, PercentOfPlusMode, PercentOfCrossMode, PercentOfBreathingMode, PercentOfLongitudinalMode, PercentOfXMode, PercentOfYMode);
                }
                // pos.z = sphere_array[i].transform.position.z;

                //Debug.Log(sphere_array[0].transform.position.z);
                //Translates particle to the calculated coordinate
                sphere_array[k].transform.position = pos;
            }
        }
    }

    public void SpawnCircle(float radius = 5.0f, float maxradius = 5.0f, float centerZ = -9999)
    {
        // Initialize mesh and angles array, to create our coordinate system which we use to translate particle
        /**sphere_array = new List<GameObject>(numberOfMeshes);
        angles_array = new List<float>(numberOfMeshes);
        sphere_pos_array = new List<Vector3>(numberOfMeshes);**/

        List<GameObject> localArr = new List<GameObject>();
        List<float> localAngs = new List<float>();
        List<Vector3> localPos = new List<Vector3>();

        Vector3 center = transform.position;

        //Setting up angular step for the ring of particles, based on the number of meshes
        float global_ang = 360.0f / numberOfMeshes;

        ringRadius = maxradius;
        centralZ = centerZ;

        for (int j = 0; j < numberOfMeshes; j++)
        {
            //Set up angular distance of each particle in the ring
            float a = j * global_ang;
            //Debug.Log(a);
            

            //Arranges the system of particles and gives a coordinate of the particle based on a circle with a center and radius given, definition below
            Vector3 posi = GenLocs(center, radius, a);
            //Debug.Log(posi.x);
            //Debug.Log(posi.y);

            //Instantiate particle mesh prefab and store particle GameObject, particle position, and angular distance of the particle in our coordinate arrays
            GameObject instancer = GameObject.Instantiate(SphereMesh);
            instancer.tag = "tp"; // Tag is picked up by trail manager
            instancer.transform.position = posi;
            //Debug.Log(instancer.transform.position.x);
            localArr.Add(instancer);
            //sphere_array[j] = instancer;
            localPos.Add(posi);
            localAngs.Add(a);

            //Parents the particle to the ring GameObject
            instancer.transform.parent = ring.transform;

            sphere_array = localArr;
            sphere_pos_array = localPos;
            angles_array = localAngs;

        }

        doneSpawningSpheres = true;

    }
    Vector3 GenLocs(Vector3 center, float radius, float ang)
    {
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }
}
