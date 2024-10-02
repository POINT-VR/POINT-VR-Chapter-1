using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Directional_Arrow_Script : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private int correctAngle;

    [SerializeField] public Material correctMaterial;

    [SerializeField] public Material incorrectMaterial;
    // Start is called before the first frame update
    void Start()
    {   
        MeshRenderer gameObjectRenderer = arrow.GetComponent<MeshRenderer>();
        if(arrow.transform.eulerAngles.z == correctAngle)
        {
            gameObjectRenderer.material = correctMaterial;
        } else {
            gameObjectRenderer.material = incorrectMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void test()
    {
        arrow.transform.eulerAngles = new Vector3(
            arrow.transform.eulerAngles.x,
            arrow.transform.eulerAngles.y,
            arrow.transform.eulerAngles.z + 90
        );

        MeshRenderer gameObjectRenderer = arrow.GetComponent<MeshRenderer>();
        if(arrow.transform.eulerAngles.z == correctAngle)
        {
            gameObjectRenderer.material = correctMaterial;
        } else {
            gameObjectRenderer.material = incorrectMaterial;
        }
    }
}
