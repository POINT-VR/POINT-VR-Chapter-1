using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField]
    private DynamicAxis dynamicAxis;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ObjectiveOne());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Intro()
    {
        //incomplete
        yield break;
    }
    IEnumerator ObjectiveOne()
    {
        Debug.Log("We live in a 3 - dimensional space. Every day we interact with this 3D space. For example we can move up and down(that’s the first dimension)");
        yield return new WaitForSeconds(2);
        dynamicAxis.ShowAxes(1);
        yield return new WaitForSeconds(1);
        Debug.Log("left and right (that’s the second dimension)");
        yield return new WaitForSeconds(2);
        dynamicAxis.ShowAxes(2);
        yield return new WaitForSeconds(1);
        Debug.Log("and forward and backward (that’s the third dimension).");
        yield return new WaitForSeconds(2);
        dynamicAxis.ShowAxes(0);
        yield return new WaitForSeconds(1);
        Debug.Log("To help ourselves navigate this space, we use a mathematical tool called a coordinate system. It does not matter how the coordinate system is oriented. In front of you is one potential coordinate system, where the different colors represent different directions. Where the three lines meet is called the origin of the system. We can describe the location of any object in space relative to the origin with just 3 numbers.");
        yield return new WaitForSeconds(2);
        StartCoroutine(dynamicAxis.ExtendAxes(1, 35, 1));
        yield break;
    }

    IEnumerator ObjectiveTwo()
    {
        yield break;
    }

    IEnumerator ObjectiveThree()
    {
        yield break;
    }
    IEnumerator ObjectiveFour()
    {
        yield break;
    }
}