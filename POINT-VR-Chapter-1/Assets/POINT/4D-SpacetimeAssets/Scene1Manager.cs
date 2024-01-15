﻿using System.Collections;
using UnityEngine;


public class Scene1Manager : MonoBehaviour
{
    [SerializeField]
    private DynamicAxis dynamicAxis;
    [SerializeField]
    private PlaneScript plane;
    [SerializeField]
    private EndPointManager endPointManager;
    [SerializeField]
    private CoordinateDisplay massObject;
    [SerializeField]
    private FloatingObjectives floatingObjectives;
    void Start()
    {
        StartCoroutine(RunScene());
    }

    void Update()
    {

    }

    IEnumerator RunScene()
    {
        yield return Setup();
        yield return ObjectiveOne();
        yield return ObjectiveTwo();
        yield break;
    }
    IEnumerator Setup() //Hides all the objects we don't want
    {
        dynamicAxis.HideAxes(); 

        plane.ShowPlane(); 

        massObject.HideMass();

        //endPoint.Deactivate(); 

        yield break;
    }
    IEnumerator ObjectiveOne()
    {
        floatingObjectives.NewObjective("Introduction to the 3D coordinate system");

        Debug.Log("We live in a 3 - dimensional space. Every day we interact with this 3D space. For example we can move up and down(that’s the first dimension)");
        yield return new WaitForSeconds(2);
        dynamicAxis.ShowAxes(1);
        yield return new WaitForSeconds(1);
        Debug.Log("left and right (that’s the second dimension)");
        yield return new WaitForSeconds(1);
        dynamicAxis.ShowAxes(2);
        yield return new WaitForSeconds(1);
        Debug.Log("and forward and backward (that’s the third dimension).");
        yield return new WaitForSeconds(2);
        dynamicAxis.ShowAxes(0);
        yield return new WaitForSeconds(1);
        Debug.Log("To help ourselves navigate this space, we use a mathematical tool called a coordinate system. It does not matter how the coordinate system is oriented. In front of you is one potential coordinate system, where the different colors represent different directions. Where the three lines meet is called the origin of the system. We can describe the location of any object in space relative to the origin with just 3 numbers.");
        yield return new WaitForSeconds(2);

        StartCoroutine(dynamicAxis.ExtendAxes(1, 35, 1));

        yield return new WaitForSeconds(5);

        yield break;
    }

    IEnumerator ObjectiveTwo()
    {
        floatingObjectives.NewObjective("Move an object in 3D space");

        massObject.transform.position = new Vector3(0, 0, 0);
        massObject.ShowMass(); //Shows mass object and coordinate displayer
        massObject.ShowText();
        massObject.HideTime();

        Debug.Log("Look! A mass just appeared in space.");
        yield return new WaitForSeconds(2);

        //EndPoint stuff goes here

        massObject.transform.position = new Vector3(0, 0, 0);
        endPointManager.SetMass(massObject.gameObject);
        endPointManager.Activate();

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