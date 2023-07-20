using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GW_Tube : MonoBehaviour
{

    [SerializeField] private GameObject RingMesh;
    [SerializeField] public int numberOfMeshes = 12;
    [SerializeField] public float radius = 5.0f;
    [SerializeField] private GameObject tube;
    [SerializeField] private float PercentOfPlusMode;
    [SerializeField] private float PercentOfCrossMode;
    [SerializeField] private float PercentOfBreathingMode;
    [SerializeField] private float phaseDifference = 10f;
    [SerializeField] private float ampStep;
    [SerializeField] private float distBetweenRings = 0.1f;

    private List<GameObject> ring_array;
    private List<float> phase_array;
    private List<float> ampsteparray;

    private Vector3 center;


    // Start is called before the first frame update
    void Start()
    {
        CreateTube();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < numberOfMeshes; i++)
        {
            //ring_array[i].phase = phase_array[i];
            //ring_array[i].ampIndex = ampsteparray[i];

            //GW_Ring ringScript = ring_array[i].GetComponent<GW_Ring>();
           // ringScript.phase = phase_array[i];
            //ringScript.ampIndex = ampsteparray[i];
        }
        
    }

    void CreateTube()
    {
        //Creates Tube on z axis

        float z = center.z;
        float phase = 0.0f;
        float ampIndex = 0.0f;

        for (int i = 0; i < numberOfMeshes; i++)
        {
            

            Vector3 pos = new Vector3(center.x, center.y, z);

            GameObject instance = Instantiate(RingMesh, pos, Quaternion.identity) as GameObject;

            ring_array.Add(instance);
            phase_array.Add(phase);
            ampsteparray.Add(ampIndex);

            z += distBetweenRings;
            phase += phaseDifference;
            ampIndex += ampStep;
            
        }
    }
}
