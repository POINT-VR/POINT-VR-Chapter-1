using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GW_Tube : MonoBehaviour
{

    [SerializeField] private GameObject RingMesh;
    [SerializeField] public int numberOfMeshes = 12;
    [SerializeField] public float radius = 5.0f;
    [SerializeField] private GameObject tube;

    private float PercentOfPlusMode;
    private float PercentOfCrossMode;
    private float PercentOfBreathingMode;
    private float PercentOfLongitudinalMode;
    private float PercentOfXMode;
    private float PercentOfYMode;

    [SerializeField] private float phaseDifference = 10f;
    [SerializeField] private float ampStep;
    [SerializeField] private float distBetweenRings = 0.1f;

    private List<GameObject> ring_array;
    private List<float> phase_array;
    private List<float> ampsteparray;

    private Vector3 center;

    private bool doneSpawning = false;


    // Start is called before the first frame update
    void Start()
    {
        ring_array = new List<GameObject>(numberOfMeshes);
        phase_array = new List<float>(numberOfMeshes);
        ampsteparray = new List<float>(numberOfMeshes);

        //ampStep = ampStep * Mathf.PI;

        CreateTube();

        PercentOfPlusMode = 0;
        PercentOfCrossMode = 0;
        PercentOfBreathingMode = 0;
        PercentOfLongitudinalMode = 0;
        PercentOfXMode = 0;
        PercentOfYMode = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (doneSpawning)
        {
            for (int i = 0; i < numberOfMeshes; i++)
            {
                //ring_array[i].phase = phase_array[i];
                //ring_array[i].ampIndex = ampsteparray[i];

                GW_Ring ringScript = ring_array[i].GetComponent<GW_Ring>();
                ringScript.phase = phase_array[i];
                ringScript.ampIndex = ampsteparray[i];
                ringScript.PercentOfPlusMode = PercentOfPlusMode;
                ringScript.PercentOfCrossMode = PercentOfCrossMode;
                ringScript.PercentOfBreathingMode = PercentOfBreathingMode;
                ringScript.PercentOfLongitudinalMode = PercentOfLongitudinalMode;
                ringScript.PercentOfXMode = PercentOfXMode;
                ringScript.PercentOfYMode = PercentOfYMode;
            }
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

            GW_Ring ringScript = instance.GetComponent<GW_Ring>();
            ringScript.SpawnCircle(radius);

            ring_array.Add(instance);
            phase_array.Add(phase);
            ampsteparray.Add(ampIndex);

            

            z += distBetweenRings;
            phase += phaseDifference;
            ampIndex += ampStep;

            instance.transform.parent = tube.transform;

        }

        doneSpawning = true;
    }

    public void SetPlusMode(float percent) { PercentOfPlusMode = percent; }
    public void SetCrossMode(float percent) { PercentOfCrossMode = percent; }
    public void SetBreathingMode(float percent) { PercentOfBreathingMode = percent; }
    public void SetLongitudinalMode(float percent) { PercentOfLongitudinalMode = percent; }
    public void SetXMode(float percent) { PercentOfXMode = percent; }
    public void SetYMode(float percent) { PercentOfYMode = percent; }

    public float GetPlusMode() { return PercentOfPlusMode; }
    public float GetCrossMode() { return PercentOfCrossMode; }
    public float GetBreathingMode() { return PercentOfBreathingMode; }
    public float GetLongitudinalMode() { return PercentOfLongitudinalMode; }
    public float GetXMode() { return PercentOfXMode; }
    public float GetYMode() { return PercentOfYMode; }
}
