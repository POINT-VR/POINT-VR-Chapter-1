using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GW_Detector : MonoBehaviour
{

    [SerializeField] private GameObject SphereMesh;
    
    [SerializeField] public float radius = 5.0f;
    [SerializeField] private GameObject detector;

    private int numberOfMeshes = 2;
    private List<GameObject> sphere_array;
    private List<Vector3> sphere_pos_array;
    private List<float> angles_array;
    private List<float> arms_length_array;
    private GW_GravityScript gw_gravity;

    private Vector3 SourceLocation;
    private Quaternion SourceRotation;
    private Vector3 radialToSource;

    [SerializeField] public float PercentOfPlusMode;
    [SerializeField] public float PercentOfCrossMode;
    [SerializeField] public float PercentOfBreathingMode;
    [SerializeField] public float PercentOfLongitudinalMode;
    [SerializeField] public float PercentOfXMode;
    [SerializeField] public float PercentOfYMode;

    private float phi;
    private float theta;
    private float psi;


    // Start is called before the first frame update
    void Start()
    {
        gw_gravity = GetComponent<GW_GravityScript>();

        //Initialize mesh and angles array, to create our coordinate system which we use to translate particles
        sphere_array = new List<GameObject>(numberOfMeshes);
        sphere_pos_array = new List<Vector3>(numberOfMeshes);
        arms_length_array = new List<float>(numberOfMeshes);
        angles_array = new List<float>(numberOfMeshes);



        //Center position of the GameObject this script is attached to, used to spawn particles around it
        Vector3 center = transform.position;

        Instantiate(SphereMesh, center, Quaternion.identity);

        float global_ang = 90.0f;

        for (int i = 0; i < numberOfMeshes; i++)
        {
            //Set up angular distance of each particle in the ring
            float a = i * global_ang;

            //Arranges the system of particles and gives a coordinate of the particle based on a circle with a center and radius given, definition below
            Vector3 pos = SpawnCircle(center, radius, a);

            //Instantiate particle mesh prefab and store particle GameObject, particle position, and angular distance of the particle in our coordinate arrays
            GameObject instance = Instantiate(SphereMesh, pos, Quaternion.identity) as GameObject;
            sphere_array.Add(instance);
            sphere_pos_array.Add(pos);
            angles_array.Add(a);

            //Parents the particle to the ring GameObject
            instance.transform.parent = detector.transform;

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //BAD BAD BAD, ONLY FOR TESTING, REMOVE MAGIC NUMBERS IF IT WORKS
        float centralZ = transform.position.z + 0.4f;

        //Sanity check
        if (true)
        {
            for (int i = 0; i < numberOfMeshes; i++)
            {


                Vector3 pos = gw_gravity.CalculateOscillations(sphere_pos_array[i], transform.position, radius, 0.4f, centralZ, PercentOfPlusMode, PercentOfCrossMode, PercentOfBreathingMode, PercentOfLongitudinalMode, PercentOfXMode, PercentOfYMode);

                // pos.z = sphere_array[i].transform.position.z;

                //Translates particle to the calculated coordinate
                sphere_array[i].transform.position = pos;
            }
        }
    }

    Vector3 SpawnCircle(Vector3 center, float radius, float ang)
    {

        //Generates a coordinate based on a parametric equation of a circle based on the center, radius, and angular distance given
        Vector3 pos;

        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;

        return pos;
    }

    void FindSource()
    {
        GameObject source = GameObject.FindWithTag("GW Source");
        SourceLocation = source.transform.position;
        SourceRotation = source.transform.rotation;


        //Take the radial component between the GW Source and Detector
        radialToSource = SourceLocation - transform.position;

        /**
         * Here I'm trying to take the angles phi, theta, and psi according to the spherical coordinate basis
         * There might be some issues regarding the difference in the coordinate systems of the theoretical functions and the Unity game development environment
         * , and it might take some renaming of the three angles
         * TODO: Test these out, and correct them if discrepancies occur
         * **/

        

        //Projects vector onto XZ Plane (normal vector being up (Y positive))
        Vector3 ProjectionInXZPlane = Vector3.ProjectOnPlane(radialToSource, Vector3.up);

        //Find angle phi (angle from XZ Plane projection of the vector and the x-axis
        phi = Vector3.Angle(ProjectionInXZPlane, Vector3.right);

        //Projects vector onto YZ Plane (normal vector being right (X positive))
        Vector3 ProjectionInYZPlane = Vector3.ProjectOnPlane(radialToSource, Vector3.right);

        //Find angle theta (angle from YZ Plane projection of the vector and the y-axis
        theta = Vector3.Angle(ProjectionInXZPlane, Vector3.up);

        //Find angle psi from the x-component of the source rotation
        psi = SourceRotation.x;


    }
}
