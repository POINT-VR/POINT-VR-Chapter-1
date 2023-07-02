using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GW_Ring : MonoBehaviour
{

    [SerializeField] private GameObject SphereMesh;
    [SerializeField] public int numberOfMeshes = 12;
    [SerializeField] public float radius = 5.0f;
    [SerializeField] private GameObject ring;
    [SerializeField] private float PercentOfPlusMode;
    [SerializeField] private float PercentOfCrossMode;

    private List<GameObject> sphere_array;
    private List<float> angles_array;
    private List<Vector3> sphere_pos_array;
    private GW_GravityScript gw_gravity;


    // Start is called before the first frame update
    void Start()
    {
        gw_gravity = GetComponent<GW_GravityScript>();
        
        sphere_array = new List<GameObject>(numberOfMeshes);
        angles_array = new List<float>(numberOfMeshes);
        sphere_pos_array = new List<Vector3>(numberOfMeshes);

        Vector3 center = transform.position;

        float global_ang = 360.0f / numberOfMeshes;

        for(int i = 0; i < numberOfMeshes; i++)
        {
            float a = i * global_ang;
            Vector3 pos = SpawnCircle(center, radius, a);
            

            GameObject instance = Instantiate(SphereMesh, pos, Quaternion.identity) as GameObject;
            sphere_array.Add(instance);
            sphere_pos_array.Add(pos);
            instance.transform.parent = ring.transform;
            angles_array.Add(a);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < numberOfMeshes; i++)
        {

            //Vector3 pos = gw_gravity.MoveCrossMode(sphere_pos_array[i], ring.transform.position, angles_array[i]);

            Vector3 pos = PercentOfPlusMode/100.0f* gw_gravity.MovePlusMode(sphere_pos_array[i], ring.transform.position, angles_array[i]) 
                + PercentOfCrossMode / 100.0f* gw_gravity.MoveCrossMode(sphere_pos_array[i], ring.transform.position, angles_array[i]);
            pos.z = sphere_array[i].transform.position.z;

            sphere_array[i].transform.position = pos;
        }
    }

    Vector3 SpawnCircle(Vector3 center, float radius, float ang)
    {
        Vector3 pos;

        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;

        return pos;
    }
}
