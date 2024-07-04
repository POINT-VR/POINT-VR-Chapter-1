using System.Collections;
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
        yield return ObjectiveThree();
        yield return ObjectiveFour();
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
        Debug.Log("This object is currently located at the origin, so its position is (0, 0, 0). Now, reach out and drag the object along the grid lines to get to the desired location. You may follow the example path or make your own path.");
        //EndPoint stuff goes here
        massObject.transform.position = new Vector3(0, 0, 0);
        endPointManager.SetMass(massObject.gameObject);
        endPointManager.Activate();
        // TODO: Once the desired location is reached
        yield return new WaitForSeconds(4);
        Debug.Log("Nice job.");
        yield return new WaitForSeconds(2);
        // TODO: Object needs to be reset to origin
        Debug.Log("Now, try getting the object to the same point by taking a different path.");
        yield return new WaitForSeconds(5);
        // TODO: Continue once player reaches desired location again
        yield break;
    }

    IEnumerator ObjectiveThree()
    {
        // Temporary name for the objective until the objectives are implemented
        floatingObjectives.NewObjective("Introduction to the dimension of time");
        // TODO: Update graphic from previous objective to include 4 cords, add a clock from scene 2 with increasing time, t cord increases with increasing clock time
        Debug.Log("However, this spatial description is not enough. Let's say you want to meet up with a friend. You will have to choose where to meet, and also when to meet. To account for this new information, we need to add one more dimension to our coordinate system, time.");
        yield return new WaitForSeconds(5);
        Debug.Log("Time is different from the other dimensions because we can only move forward in time. Notice how the time on the clock only ever increases."); 
        yield return new WaitForSeconds(5);
        Debug.Log("Move the object as you like and observe how the description of its spatial location changes while time keeps moving forward. Press the 'continue' button when you are ready to move on."); 
        yield return new WaitForSeconds(5);
        // TODO: Player can move around the object and see how the coordinates change (time increases continuously)
        // TODO: Continue button appears to continue when ready
        yield break;
    }
    IEnumerator ObjectiveFour()
    {
        // Temporary name for the objective until the objectives are implemented
        floatingObjectives.NewObjective("Introduction to 4D Spacetime");
        Debug.Log("Together, the information of the 1D time and the 3D location of an event is what we call 4D spacetime. Spacetime is what makes up the very fabric of our universe. It's all around you, stretching out in every direction and forever into the future."); 
        // TODO: Turn axes into infinite grid
        yield return new WaitForSeconds(5);
        Debug.Log("Spacetime is not a rigid or fixed object. It can curve."); 
        yield return new WaitForSeconds(2);
        // TODO: Make spacetime curve toward yellow sphere
        Debug.Log("In fact, Einstein described gravity as the curvature of spacetime. Close to a very massive object, where gravity is strong, the duration of an event and the distance between two events can stretch. John Wheeler described this effect by saying 'Spacetime tells matter how to move; matter tells spacetime how to curve.'"); 
        yield return new WaitForSeconds(5);
        Debug.Log("Now, let's look at how spacetime curves. Looking at this large grid is too much information at once, so we are going to show you only a small portion of the spacetime."); 
        yield return new WaitForSeconds(5);
        yield break;
    }

}