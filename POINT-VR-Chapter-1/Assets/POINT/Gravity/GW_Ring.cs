using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GW_Ring : MonoBehaviour
{

    [SerializeField] private GameObject SphereMesh;
    [SerializeField] public int numberOfMeshes = 12;
    [SerializeField] public float radius = 5.0f;
    private List<GameObject> sphere_array;


    // Start is called before the first frame update
    void Start()
    {
        sphere_array = new List<GameObject>(numberOfMeshes);

        Vector3 center = transform.position;

        float global_ang = 360.0f / numberOfMeshes;

        for(int i = 0; i < numberOfMeshes; i++)
        {
            float a = i * global_ang;
            Vector3 pos = SpawnCircle(center, radius, a);

            sphere_array.Add(Instantiate(SphereMesh, pos, Quaternion.identity) as GameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 SpawnCircle(Vector3 center, float radius, float ang)
    {
        Vector3 pos;

        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.x + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;

        return pos;
    }
}
