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
    [SerializeField] private float phaseDifference;
    [SerializeField] private float ampStep;

    private List<GameObject> ring_array;
    private List<float> phase_array;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
