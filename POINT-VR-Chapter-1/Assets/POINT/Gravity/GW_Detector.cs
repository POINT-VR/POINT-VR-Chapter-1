using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GW_Detector : MonoBehaviour
{

    [SerializeField] private GameObject SphereMesh;
    
    [SerializeField] public float LengthOfArm1 = 5.0f;
    [SerializeField] public float LengthOfArm2 = 5.0f;
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


    // Start is called before the first frame update
    void Start()
    {
        gw_gravity = GetComponent<GW_GravityScript>();

        //Initialize mesh and angles array, to create our coordinate system which we use to translate particles
        sphere_array = new List<GameObject>(numberOfMeshes);
        sphere_pos_array = new List<Vector3>(numberOfMeshes);
        arms_length_array = new List<float>(numberOfMeshes);
        angles_array = new List<float>(numberOfMeshes);

        arms_length_array.Add(LengthOfArm1);
        arms_length_array.Add(LengthOfArm2);


        //Center position of the GameObject this script is attached to, used to spawn particles around it
        Vector3 center = transform.position;

        Instantiate(SphereMesh, center, Quaternion.identity);

        float global_ang = 90.0f;

        for (int i = 0; i < numberOfMeshes; i++)
        {
            //Set up angular distance of each particle in the ring
            float a = i * global_ang;

            //Arranges the system of particles and gives a coordinate of the particle based on a circle with a center and radius given, definition below
            Vector3 pos = SpawnCircle(center, arms_length_array[i], a);

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


    }
}
