using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GW_Sphere : MonoBehaviour
{
    [SerializeField] private GameObject RingMesh;
    [SerializeField] public int numberOfMeshes = 12;
    [SerializeField] public float radius = 5.0f;
    [SerializeField] private GameObject tube;
    [SerializeField] private int maxNumberOfMeshes = 100;

    [SerializeField] private float PercentOfPlusMode;
    [SerializeField] private float PercentOfCrossMode;
    [SerializeField] private float PercentOfBreathingMode;
    [SerializeField] private float PercentOfLongitudinalMode;
    [SerializeField] private float PercentOfXMode;
    [SerializeField] private float PercentOfYMode;

    [SerializeField] private float phaseDifference = 10f;
    [SerializeField] private float ampStep;
    [SerializeField] private float distBetweenRings = 0.1f;

    private List<GameObject> ring_array;
    private List<float> phase_array;
    private List<float> ampsteparray;

    private int numOfSpheres;
    private int localRadius;
    private int ringSpheresStep = 1;
    private int numOfRings;

    private Vector3 center;

    private bool doneSpawning = false;
    // Start is called before the first frame update
    void Start()
    {
        ring_array = new List<GameObject>(numberOfMeshes);
        phase_array = new List<float>(numberOfMeshes);
        ampsteparray = new List<float>(numberOfMeshes);

        CreateTube();
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

    //Reused from GW_Tube
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
            ringScript.SpawnCircle();

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

    void GenSphereRadii()
    {
        //Generate Radii and Number of Particles for each Ring such that a sphere is formed

        numOfSpheres = maxNumberOfMeshes;
        localRadius = radius;

        while(numOfSpheres > 1 && localRadius >= 0)
        {
            //Linear step for z position of rings, quadratic decrease for both radius of the rings and the number of meshes for each ring.
            numOfSpheres -= (ringSpheresStep * ringSpheresStep);
            localRadius -= (ringSpheresStep * ringSpheresStep);
            numOfRings++;
            ringSpheresStep++;
        }
    }
}
