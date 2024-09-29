using System.Collections;
using System.Collections.Generic;
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
    private Camera currentCamera = null;
    private GameObject player = null;
    private GameObject examplePath = null;
    private GameObject objectiveClock = null;
    private GameObject secondPath = null;
    private GameObject continueButton = null;
    private static bool objectiveContinue = false;
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
        yield return WaitForPlayerSpawn();
        yield return ObjectiveOne();
        yield return ObjectiveTwo();
        yield return ObjectiveThree();
        yield return ObjectiveFour();
        yield break;
    }
    IEnumerator Setup() //Hides all the objects we don't want
    {
        dynamicAxis.HideAxes(); 

        plane.ShowPlane(); 

        massObject.HideMass();

        examplePath = GameObject.Find("ExamplePathObj");
        examplePath.SetActive(false);

        secondPath = GameObject.Find("SecondPathManager");
        secondPath.SetActive(false);

        objectiveClock = GameObject.Find("Clock and Timer");
        objectiveClock.SetActive(false);

        continueButton = GameObject.Find("Continue UI Container");
        continueButton.SetActive(false);
        //endPoint.Deactivate(); 
        yield break;
    }
    IEnumerator WaitForPlayerSpawn()
    {
        yield return new WaitUntil(() => Camera.current != null);

        currentCamera = Camera.current;
        player = currentCamera.transform.parent.gameObject;

        yield break;
    }
    IEnumerator ObjectiveOne()
    {
        floatingObjectives.NewObjective("Introduction to the 3D coordinate system");

        Debug.Log("We live in a 3 - dimensional space. Every day we interact with this 3D space. For example we can move up and down(that’s the first dimension)");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\1_3D_coordinate_system_1_1");
        yield return new WaitForSeconds(6);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\1_3D_coordinate_system_1_2");
        dynamicAxis.ShowAxes(1);
        yield return new WaitForSeconds(3);
        Debug.Log("left and right (that’s the second dimension)");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\1_3D_coordinate_system_2");
        dynamicAxis.ShowAxes(0);
        yield return new WaitForSeconds(3);
        Debug.Log("and forward and backward (that’s the third dimension).");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\1_3D_coordinate_system_3");
        dynamicAxis.ShowAxes(2);
        yield return new WaitForSeconds(4);
        Debug.Log("To help ourselves navigate this space, we use a mathematical tool called a coordinate system. It does not matter how the coordinate system is oriented. In front of you is one potential coordinate system, where the different colors represent different directions. Where the three lines meet is called the origin of the system. We can describe the location of any object in space relative to the origin with just 3 numbers.");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\1_3D_coordinate_system_4");
        yield return new WaitForSeconds(25);

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
        Debug.Log("This object is currently located at the origin, so its position is (0, 0, 0). Now, reach out and drag the object along the grid lines to get to the desired location. You may follow the example path or make your own path.");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\2_move_an_object_1");
        yield return new WaitForSeconds(15);
        //EndPoint stuff goes here
        massObject.transform.position = new Vector3(0, 0, 0);
        endPointManager.SetMass(massObject.gameObject);
        endPointManager.Activate();
        examplePath.SetActive(true);
        // Once the desired location is reached
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => endPointManager.PathStatus() == true);
        // Retrieves Previous Endpoint Path
        List<Vector3> savedPath = endPointManager.GetPath();
        yield return new WaitUntil(() => endPointManager.Status() == false);
        yield return new WaitForSeconds(1);

        
        // Object reset to origin
        massObject.transform.position = new Vector3(0, 0, 0);
        Debug.Log("Nice job.");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\2_move_an_object_2");
        yield return new WaitForSeconds(2);
        Debug.Log("Now, try getting the object to the same point by taking a different path.");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\2_move_an_object_3");
        yield return new WaitForSeconds(4);
        // Endpoint manager activated again, retrieved endpoint path given as example path to 
        massObject.transform.position = new Vector3(0, 0, 0);
        endPointManager.SetMass(massObject.gameObject);
        endPointManager.setComparisonPath(savedPath);
        endPointManager.Activate();
        // Creating yellow path that shows the player's first complete path
        for (int i = 0; i < savedPath.Count; i++)
        {
            // Diverting back to normal path if the player takes more than 12 steps
            if (endPointManager.limitReached() == true) {
                break;
            }
            Vector3 v = savedPath[i];
            if (v != new Vector3(3,1,2) * 1.0f)
            {
                var newPathPoint = Instantiate(secondPath.transform.GetChild(0).gameObject, v, Quaternion.identity, secondPath.transform);
            }
            // if first line (works by checking which axis has been travelled on and using that to set the rotation of the cylinder, simply placing it at the halfway point)
            if (i == 0) 
            {
                var newPathLine = Instantiate(secondPath.transform.GetChild(1).gameObject, new Vector3(v.x / 2, v.y / 2, v.z / 2), Quaternion.identity, secondPath.transform);
                // if the line will be along y axis then that is how it is originally
                // along x axis
                if (v.x > 0)
                {
                    newPathLine.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
                }
                // along z axis
                if (v.z > 0)
                {
                    newPathLine.transform.Rotate(90.0f, 0.0f, 0.0f, Space.Self);
                }
            } else { // (in this case, the point for the cylinder is obtained by taking the halfway point between the Vector v and the last vector)
                var newPathLine = Instantiate(secondPath.transform.GetChild(1).gameObject, new Vector3((v.x + savedPath[i-1].x) / 2, (v.y + savedPath[i-1].y) / 2, (v.z + savedPath[i-1].z) / 2), Quaternion.identity, secondPath.transform); 
                // works by seeing in what direction the last movement was by checking the last vector and subtracting, then using that to set rotation
                // if the line will be along y axis that is how it is originally
                // along x axis
                if (v.x - savedPath[i-1].x != 0)
                {
                    newPathLine.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
                }
                // along z axis
                if (v.z - savedPath[i-1].z != 0)
                {
                    newPathLine.transform.Rotate(90.0f, 0.0f, 0.0f, Space.Self);
                }
            }
        }
        // Diverting back to normal path if the player takes more than 12 steps
        if (endPointManager.limitReached() == false) {
            secondPath.SetActive(true);
        }
        for (int i = 0; i < 13; i++) 
        {
            // Diverting back to normal path if the player takes more than 12 steps
            if (endPointManager.limitReached() == true) {
                break;
            }
            if (i != 5 && i != 6) 
            {
                // disables everything but the final endpoint and final coordinates
                examplePath.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        // Waits for the desired location to be reached again
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => endPointManager.Status() == false);
        secondPath.SetActive(false);
        examplePath.SetActive(false);
        yield return new WaitForSeconds(1);
        // Continue once player reaches desired location again
        massObject.transform.position = new Vector3(0, 0, 0);
        Debug.Log("Nice job.");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\2_move_an_object_2");
        yield return new WaitForSeconds(2);
        yield return new WaitForSeconds(1);
        yield break;
    }

    IEnumerator ObjectiveThree()
    {
        // Temporary name for the objective until the objectives are implemented
        floatingObjectives.NewObjective("Introduction to the dimension of time");
        //Update graphic from previous objective to include 4 cords, add a clock from scene 2 with increasing time, t cord increases with increasing clock time
        Debug.Log("However, this spatial description is not enough. Let's say you want to meet up with a friend. You will have to choose where to meet, and also when to meet. To account for this new information, we need to add one more dimension to our coordinate system, time.");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\3_clock_appears_1");
        yield return new WaitForSeconds(15);
        massObject.ShowTime();
        massObject.SetTime(0);
        objectiveClock.SetActive(true);

        Debug.Log("Time is different from the other dimensions because we can only move forward in time. Notice how the time on the clock only ever increases."); 
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\3_clock_appears_2");
        yield return new WaitForSeconds(8);
        Debug.Log("Move the object as you like and observe how the description of its spatial location changes while time keeps moving forward. Press the 'continue' button when you are ready to move on."); 
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\3_clock_appears_3");
        yield return new WaitForSeconds(10);
        massObject.transform.position = new Vector3(0, 0, 0);
        // Player can move around the object and see how the coordinates change (time increases continuously)
        // Continue button appears to continue when ready
        continueButton.SetActive(true);
        yield return new WaitUntil(() => Scene1Manager.objectiveContinue == true);
        objectiveClock.SetActive(false);
        continueButton.SetActive(false);
        massObject.transform.position = new Vector3(0, 0, 0);
        massObject.HideMass();
        yield return new WaitForSeconds(3);
        yield break;
    }
    IEnumerator ObjectiveFour()
    {
        // Temporary name for the objective until the objectives are implemented
        floatingObjectives.NewObjective("Introduction to 4D Spacetime");
        Debug.Log("Together, the information of the 1D time and the 3D location of an event is what we call 4D spacetime. Spacetime is what makes up the very fabric of our universe. It's all around you, stretching out in every direction and forever into the future."); 
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\4_spacetime_is_everywhere_1");
        yield return new WaitForSeconds(16);
        // TODO: Turn axes into infinite grid
        Debug.Log("Spacetime is not a rigid or fixed object. It can curve.");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\4_spacetime_is_everywhere_2");
        yield return new WaitForSeconds(5);
        // TODO: Make spacetime curve toward yellow sphere
        Debug.Log("In fact, Einstein described gravity as the curvature of spacetime. Close to a very massive object, where gravity is strong, the duration of an event and the distance between two events can stretch. John Wheeler described this effect by saying 'Spacetime tells matter how to move; matter tells spacetime how to curve.'"); 
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\4_spacetime_is_everywhere_3");
        yield return new WaitForSeconds(20);
        Debug.Log("Now, let's look at how spacetime curves. Looking at this large grid is too much information at once, so we are going to show you only a small portion of the spacetime."); 
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\4_spacetime_is_everywhere_4");
        yield return new WaitForSeconds(10);
        yield return new WaitForSeconds(3);
        yield break;
    }
    public void ContinueObjective() {
        objectiveContinue = true;
    }

}