using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scene1Manager : MonoBehaviour
{
    [SerializeField] private DynamicAxis dynamicAxis;
    [SerializeField] private EndPointManager endPointManager;
    [SerializeField] private CoordinateDisplay massObject;
    [SerializeField] private FloatingObjectives floatingObjectives;
    [SerializeField] private GameObject nextSceneButton;

    [Header("Grid Animation references")]
    [Tooltip("The floor of the simulation")]
    [SerializeField] private GameObject floor = null;

    [Tooltip("A large static grid that cannot deform")]
    [SerializeField] private GameObject staticGrid = null;

    [Tooltip("The grid that deforms around a mass sphere")]
    [SerializeField] private GridScript deformationGrid = null;

    [Tooltip("The mass spheres that deform the grid")]
    [SerializeField] private Rigidbody[] massSpheres = null;

    [Header("Grid Animation properties")]
    [Tooltip("Material for the static grid")]
    [SerializeField] private Material gridMaterial = null;

    [Tooltip("The intended mass of the mass sphere")]
    [SerializeField] private float mass = 0.25f;

    [Tooltip("The position where the mass starts to spawn in, ending in the center of the deformation grid")]
    [SerializeField] private Vector3 massSpawnOffset = Vector3.zero;

    [Tooltip("Speed at which masses orbit")]
    [SerializeField] private float orbitSpeed = 20.0f;

    private Camera currentCamera = null;
    private GameObject player = null;
    private GameObject examplePath = null;
    private GameObject objectiveClock = null;
    private GameObject secondPath = null;
    private GameObject continueButton = null;
    private GameObject floatingObjectivesMenu = null;
    private static bool objectiveContinue = false;

    /// <summary>
    /// A faraway point for the grid cube to spawn (for grid animation)
    /// </summary>
    private Vector3 gridCubeSpawnPoint = 10000.0f * Vector3.one;

    private void Start()
    {
        StartCoroutine(RunScene());
    }

    private void Update()
    {
        if (currentCamera != null)
        {
            Shader.SetGlobalVector("Grid_Player_Position", currentCamera.transform.position);
        }
    }

    private IEnumerator RunScene()
    {
        yield return Setup();
        yield return WaitForPlayerSpawn();
        yield return ObjectiveOne();
        yield return ObjectiveTwo();
        yield return ObjectiveThree();
        yield return ObjectiveFour();
        yield break;
    }
    private IEnumerator Setup()
    {
        // Hide all objects we do not want
        dynamicAxis.HideAxes(); 
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

        nextSceneButton.SetActive(false);

        floatingObjectivesMenu = GameObject.Find("FloatingObjectives");

        // Set up for grid animation
        Shader.SetGlobalFloat("Grid_RevealRadius", 0.0f);
        Shader.SetGlobalFloat("Grid_OpaqueRadius", 2.0f);
        Shader.SetGlobalFloat("Grid_ExponentialConstant", 0.2f);
        Shader.SetGlobalFloat("Grid_Player_HideRadius", 0.5f);
        Shader.SetGlobalFloat("Grid_Player_FadeRadius", 1.0f);

        deformationGrid.transform.position = gridCubeSpawnPoint;
        deformationGrid.GetComponent<MeshRenderer>().material = gridMaterial;
        deformationGrid.gameObject.SetActive(true);

        for (int i = 0; i < massSpheres.Length; ++i)
        {
            Rigidbody massSphere = massSpheres[i];
            massSphere.gameObject.SetActive(true);
            Color originalMassColor = massSphere.GetComponent<MeshRenderer>().material.color;
            massSphere.GetComponent<MeshRenderer>().material.color = new Color(originalMassColor.r, originalMassColor.g, originalMassColor.b, 0.0f);
            massSphere.gameObject.SetActive(false);
        }

        yield break;
    }
    private IEnumerator WaitForPlayerSpawn()
    {
        yield return new WaitUntil(() => Camera.current != null);

        currentCamera = Camera.current;
        player = currentCamera.transform.parent.gameObject;

        yield break;
    }
    private IEnumerator ObjectiveOne()
    {
        floatingObjectives.NewObjective("Introduction to the 3D coordinate system");

        Debug.Log("We live in a 3 - dimensional space. Every day we interact with this 3D space. For example we can move up and down(that’s the first dimension)");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\1_3D_coordinate_system_1_1");
        yield return new WaitForSecondsRealtime(6);
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\1_3D_coordinate_system_1_2");
        dynamicAxis.ShowAxes(1);
        yield return new WaitForSecondsRealtime(3);
        Debug.Log("left and right (that’s the second dimension)");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\1_3D_coordinate_system_2");
        dynamicAxis.ShowAxes(0);
        yield return new WaitForSecondsRealtime(3);
        Debug.Log("and forward and backward (that’s the third dimension).");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\1_3D_coordinate_system_3");
        dynamicAxis.ShowAxes(2);
        yield return new WaitForSecondsRealtime(4);
        Debug.Log("To help ourselves navigate this space, we use a mathematical tool called a coordinate system. It does not matter how the coordinate system is oriented. In front of you is one potential coordinate system, where the different colors represent different directions. Where the three lines meet is called the origin of the system. We can describe the location of any object in space relative to the origin with just 3 numbers.");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\1_3D_coordinate_system_4");
        yield return new WaitForSecondsRealtime(8.3f);

        StartCoroutine(dynamicAxis.ExtendAxes(1, 35, 1));
        yield return new WaitForSecondsRealtime(16.7f);

        yield break;
    }

    private IEnumerator ObjectiveTwo()
    {
        floatingObjectives.NewObjective("Move an object in 3D space");

        massObject.transform.position = new Vector3(0, 0, 0);
        massObject.ShowMass(); //Shows mass object and coordinate displayer
        massObject.ShowText();
        massObject.HideTime();
        Debug.Log("Look! A mass just appeared in space.");
        yield return new WaitForSecondsRealtime(1);
        Debug.Log("This object is currently located at the origin, so its position is (0, 0, 0). Now, reach out and drag the object along the grid lines to get to the desired location. You may follow the example path or make your own path.");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\2_move_an_object_1");
        yield return new WaitForSecondsRealtime(15);
        //EndPoint stuff goes here
        massObject.transform.position = new Vector3(0, 0, 0);
        endPointManager.SetMass(massObject.gameObject);
        endPointManager.Activate();
        examplePath.SetActive(true);
        // Once the desired location is reached
        yield return new WaitForSecondsRealtime(1);
        yield return new WaitUntil(() => endPointManager.PathStatus() == true);
        // Retrieves Previous Endpoint Path
        List<Vector3> savedPath = endPointManager.GetPath();
        yield return new WaitUntil(() => endPointManager.Status() == false);
        yield return new WaitForSecondsRealtime(1);

        
        // Object reset to origin
        massObject.transform.position = new Vector3(0, 0, 0);
        Debug.Log("Nice job.");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\2_move_an_object_2");
        yield return new WaitForSecondsRealtime(2);
        Debug.Log("Now, try getting the object to the same point by taking a different path.");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\2_move_an_object_3");
        yield return new WaitForSecondsRealtime(4);
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
        yield return new WaitForSecondsRealtime(1);
        yield return new WaitUntil(() => endPointManager.Status() == false);
        secondPath.SetActive(false);
        examplePath.SetActive(false);
        yield return new WaitForSecondsRealtime(1);
        // Continue once player reaches desired location again
        massObject.transform.position = new Vector3(0, 0, 0);
        Debug.Log("Nice job.");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\2_move_an_object_2");
        yield return new WaitForSecondsRealtime(2);
        yield break;
    }

    private IEnumerator ObjectiveThree()
    {
        // Temporary name for the objective until the objectives are implemented
        floatingObjectives.NewObjective("Introduction to the dimension of time");
        //Update graphic from previous objective to include 4 cords, add a clock from scene 2 with increasing time, t cord increases with increasing clock time
        Debug.Log("However, this spatial description is not enough. Let's say you want to meet up with a friend. You will have to choose where to meet, and also when to meet. To account for this new information, we need to add one more dimension to our coordinate system, time.");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\3_clock_appears_1");
        yield return new WaitForSecondsRealtime(15);
        massObject.ShowTime();
        massObject.SetTime(0);
        objectiveClock.SetActive(true);

        Debug.Log("Time is different from the other dimensions because we can only move forward in time. Notice how the time on the clock only ever increases."); 
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\3_clock_appears_2");
        yield return new WaitForSecondsRealtime(8);
        Debug.Log("Move the object as you like and observe how the description of its spatial location changes while time keeps moving forward. Press the 'continue' button when you are ready to move on."); 
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\3_clock_appears_3");
        yield return new WaitForSecondsRealtime(10);
        massObject.transform.position = new Vector3(0, 0, 0);
        // Player can move around the object and see how the coordinates change (time increases continuously)
        // Continue button appears to continue when ready
        continueButton.SetActive(true);
        yield return new WaitUntil(() => Scene1Manager.objectiveContinue == true);
        objectiveClock.SetActive(false);
        continueButton.SetActive(false);
        massObject.transform.position = new Vector3(0, 0, 0);
        massObject.HideMass();
        yield return new WaitForSecondsRealtime(3);
        yield break;
    }
    private IEnumerator ObjectiveFour()
    {
        // Temporary name for the objective until the objectives are implemented
        floatingObjectives.NewObjective("Introduction to 4D Spacetime");

        // Reset position of relevant objects as deformed grid requires origin to be at its center
        Vector3 originPosition = (deformationGrid.gridSize - Vector3.one) / 2.0f;
        Shader.SetGlobalVector("Grid_OriginPosition", new Vector4(originPosition.x, originPosition.y, originPosition.z, 0.0f));
        Vector3 translate = originPosition - dynamicAxis.transform.position;
        dynamicAxis.transform.position = originPosition;
        floor.transform.position += translate;
        floatingObjectives.transform.position += translate;
        player.transform.parent.position += translate; // PlayerBase

        Bounds staticGridBounds = staticGrid.GetComponent<MeshRenderer>().bounds;
        staticGrid.transform.position = originPosition + (new Vector3(0.5f, -0.5f, 0.5f) * (int)staticGridBounds.size.x);
        staticGrid.SetActive(true);

        for (int i = 0; i < massSpheres.Length; ++i)
        {
            Rigidbody massSphere = massSpheres[i];
            massSphere.transform.position = originPosition + (massSpawnOffset * Mathf.Pow(-1, i));
        }

        floatingObjectivesMenu.SetActive(false); // Hide Objectives as grid grows
        Debug.Log("Together, the information of the 1D time and the 3D location of an event is what we call 4D spacetime. Spacetime is what makes up the very fabric of our universe. It's all around you, stretching out in every direction and forever into the future."); 
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\4_spacetime_is_everywhere_1");
        // Sum of yield returns should be 16s
        StartCoroutine(dynamicAxis.TransitionAxisColor(Color.white, 4.0f));
        yield return dynamicAxis.TransitionAxisThickness(0.02f, 4.0f);
        yield return RevealGrid(16.0f, 12.0f);

        Debug.Log("Spacetime is not a rigid or fixed object. It can curve.");
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\4_spacetime_is_everywhere_2");
        // Sum of yield returns should be 5s
        dynamicAxis.SetAxisMaterial(gridMaterial);
        yield return ShrinkGrid(1.8f, 3.0f, 2.0f);
        dynamicAxis.gameObject.SetActive(false);
        staticGrid.SetActive(false);
        floatingObjectivesMenu.SetActive(true); // Bring back Objectives as grid shrinks
        foreach (Rigidbody massSphere in massSpheres)
        {
            massSphere.gameObject.SetActive(true);
        }
        deformationGrid.transform.position = Vector3.zero;

        foreach (Rigidbody massSphere in massSpheres)
        {
            // Reveal mass and start orbit
            massSphere.constraints = RigidbodyConstraints.FreezePositionY;
            Color massColor = massSphere.GetComponent<MeshRenderer>().material.color;
            massSphere.mass = mass;
            massSphere.GetComponent<MeshRenderer>().material.color = new Color(massColor.r, massColor.g, massColor.b, 1.0f);
            StartCoroutine(AnimateSphereCycle(massSphere, originPosition, orbitSpeed));
        }
        yield return new WaitForSecondsRealtime(3);
        
        Debug.Log("In fact, Einstein described gravity as the curvature of spacetime. Close to a very massive object, where gravity is strong, the duration of an event and the distance between two events can stretch. John Wheeler described this effect by saying 'Spacetime tells matter how to move; matter tells spacetime how to curve.'"); 
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\4_spacetime_is_everywhere_3");
        yield return new WaitForSecondsRealtime(20);
        nextSceneButton.SetActive(true); // End of Scene, tell player to continue
        Debug.Log("Now, let's look at how spacetime curves. Looking at this large grid is too much information at once, so we are going to show you only a small portion of the spacetime."); 
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("Chapter1Scene1\\4_spacetime_is_everywhere_4");
        yield return new WaitForSeconds(9.6f);
        
        Debug.Log("Press the 'continue' button when you are ready to move on."); 
        player.GetComponent<NarrationManager>().PlayClipWithSubtitles("continue");
        yield break;
    }

    public void ContinueObjective() {
        objectiveContinue = true;
    }

    #region Grid Animation helper functions
    private IEnumerator RevealGrid(float maxRadius, float duration)
    {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            yield return null;
            float t = timeElapsed / duration;
            float lerpPoint = Mathf.Lerp(0.0f, maxRadius, t);
            Shader.SetGlobalFloat("Grid_RevealRadius", lerpPoint);
            timeElapsed += Time.deltaTime;
        }
        Shader.SetGlobalFloat("Grid_RevealRadius", maxRadius); //snaps to final value after last loop
        yield break;
    }

    private IEnumerator ShrinkGrid(float endOpaqueRadius, float endExponentialConstant, float duration)
    {
        float timeElapsed = 0;

        float startOpaqueRadius = Shader.GetGlobalFloat("Grid_OpaqueRadius");
        float startExponentialConstant = Shader.GetGlobalFloat("Grid_ExponentialConstant");

        while (timeElapsed < duration)
        {
            yield return null;
            float t = timeElapsed / duration;

            float opaqueRadiusLerp = Mathf.Lerp(startOpaqueRadius, endOpaqueRadius, t);
            Shader.SetGlobalFloat("Grid_OpaqueRadius", opaqueRadiusLerp);

            float exponentialConstantRadius = Mathf.Lerp(startExponentialConstant, endExponentialConstant, t);
            Shader.SetGlobalFloat("Grid_ExponentialConstant", exponentialConstantRadius);

            timeElapsed += Time.deltaTime;
        }

        Shader.SetGlobalFloat("Grid_OpaqueRadius", endOpaqueRadius);
        Shader.SetGlobalFloat("Grid_ExponentialConstant", endExponentialConstant);

        yield break;
    }

    private IEnumerator AnimateSphereCycle(Rigidbody massSphere, Vector3 originPosition, float speed)
    {
        while (true)
        {
            massSphere.transform.RotateAround(originPosition, Vector3.up, speed * Time.deltaTime);
            yield return null;
        }
    }
    #endregion
}