using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Attached to the Player in the DensityExploration Scene
/// Takes care of updating the shell masses and mass-shell snapping interactions
/// </summary>
public class DensityExploration : MonoBehaviour
{
    // Serialized fields
    [Header("References")]
    [SerializeField] private GameObject floor;
    [SerializeField] private InputActionReference openMenuReference;
    private Camera currentCamera = null;
    private GameObject player = null;
    private GameObject menus = null;
    private GameObject buttons = null;
    private UIManager UIManagerScript = null;
    // Masses
    private GameObject shell = null;
    private GameObject shell2 = null;

    /// <summary>
    /// 2D Array of Snap Locations
    /// </summary>
    Vector3[,] positionArray = new Vector3[8, 8];

    /// <summary>
    /// Number of snapped masses (first index corresponds to first shell (larger) and second corresponds to second shell)
    /// </summary>
    private int[] numSnapped = new int[2];

    /// <summary>
    /// Lists of what masses are snapped to each shell
    /// </summary>
    bool[] snapArray = new bool[8];
    bool[] snapArray2 = new bool[8];

    /// <summary>
    /// List holding all mass GameObjects
    /// </summary>
    GameObject[] massList = new GameObject[8];

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForPlayerSpawn());
    }
    private IEnumerator WaitForPlayerSpawn()
    {
        yield return new WaitUntil(() => Camera.current != null);

        // Initializing camera and player
        currentCamera = Camera.current;
        player = currentCamera.transform.parent.gameObject;

        // Initializing mass list and shell references
        massList[0] = GameObject.Find("UnitMassOne");
        massList[1] = GameObject.Find("UnitMassTwo");
        massList[2] = GameObject.Find("UnitMassThree");
        massList[3] = GameObject.Find("UnitMassFour");
        massList[4] = GameObject.Find("UnitMassFive");
        massList[5] = GameObject.Find("UnitMassSix");
        massList[6] = GameObject.Find("UnitMassSeven");
        massList[7] = GameObject.Find("UnitMassEight");
        shell = GameObject.Find("Sphere");
        shell2 = GameObject.Find("Sphere2");

        // Filling Snap List
        positionArray[0, 0] = new Vector3(0f, 0f, 0f);
        positionArray[1, 0] = new Vector3(-0.75f, 0f, 0f);
        positionArray[1, 1] = new Vector3(0.75f, 0f, 0f);
        positionArray[2, 0] = new Vector3(-0.75f, -0.75f, 0f);
        positionArray[2, 1] = new Vector3(0.75f, -0.75f, 0f);
        positionArray[2, 2] = new Vector3(0f, 0.75f, 0f);
        positionArray[3, 0] = new Vector3(-0.7f, -0.7f, 0f);
        positionArray[3, 1] = new Vector3(0.7f, -0.7f, 0f);
        positionArray[3, 2] = new Vector3(0.7f, 0.7f, 0f);
        positionArray[3, 3] = new Vector3(-0.7f, 0.7f, 0f);
        positionArray[4, 0] = new Vector3(-0.7f, -0.7f, 0f);
        positionArray[4, 1] = new Vector3(-0.7f, 0.7f, 0f);
        positionArray[4, 2] = new Vector3(0.7f, -0.7f, 0f);
        positionArray[4, 3] = new Vector3(0.7f, 0.7f, 0f);
        positionArray[4, 4] = new Vector3(0f, 0f, 0f);
        positionArray[5, 0] = new Vector3(-0.7f, -0.7f, 0f);
        positionArray[5, 1] = new Vector3(-0.7f, 0.7f, 0f);
        positionArray[5, 2] = new Vector3(0.7f, -0.7f, 0f);
        positionArray[5, 3] = new Vector3(0.7f, 0.7f, 0f);
        positionArray[5, 4] = new Vector3(0f, 0f, -0.75f);
        positionArray[5, 5] = new Vector3(0f, 0f, 0.75f);
        positionArray[6, 0] = new Vector3(-0.7f, -0.7f, 0f);
        positionArray[6, 1] = new Vector3(-0.7f, 0.7f, 0f);
        positionArray[6, 2] = new Vector3(0.7f, -0.7f, 0f);
        positionArray[6, 3] = new Vector3(0.7f, 0.7f, 0f);
        positionArray[6, 4] = new Vector3(0f, 0f, -0.75f);
        positionArray[6, 5] = new Vector3(0f, 0f, 0.75f);
        positionArray[6, 6] = new Vector3(0f, 0f, 0f);
        positionArray[7, 0] = new Vector3(-0.7f, -0.7f, -0.75f);
        positionArray[7, 1] = new Vector3(-0.7f, 0.7f, -0.75f);
        positionArray[7, 2] = new Vector3(0.7f, -0.7f, -0.75f);
        positionArray[7, 3] = new Vector3(0.7f, 0.7f, -0.75f);
        positionArray[7, 4] = new Vector3(-0.7f, -0.7f, 0.75f);
        positionArray[7, 5] = new Vector3(-0.7f, 0.7f, 0.75f);
        positionArray[7, 6] = new Vector3(0.7f, -0.7f, 0.75f);
        positionArray[7, 7] = new Vector3(0.7f, 0.7f, 0.75f);

        // Filling Snap Arrays
        snapArray[0] = false;
        snapArray[1] = false;
        snapArray[2] = false;
        snapArray[3] = false;
        snapArray[4] = false;
        snapArray[5] = false;
        snapArray[6] = false;
        snapArray[7] = false;
        snapArray2[0] = false;
        snapArray2[1] = false;
        snapArray2[2] = false;
        snapArray2[3] = false;
        snapArray2[4] = false;
        snapArray2[5] = false;
        snapArray2[6] = false;
        snapArray2[7] = false;
        
        // Setting number of snapped masses for each shell to 0
        numSnapped[0] = 0;
        numSnapped[1] = 0;
        StartCoroutine(StartScene());
    }
    void Update()
    {
        snapChecker(shell, 1, snapArray); // handles snapping behavior for larger shell
        snapChecker(shell2, 2, snapArray2); // handles snapping behavior for smaller shell
        updateMass(shell, 1); // updates mass of larger shell
        updateMass(shell2, 2); // updates mass of smaller shell
    }
    IEnumerator StartScene()
    {
        //Instantiate menus from player prefab and buttons from player prefab as well
        GameObject mainCamera = player.transform.Find("Main Camera").gameObject;
        GameObject UIContainer = mainCamera.transform.Find("UI Container").gameObject;
        GameObject Menu = UIContainer.transform.Find("Menu").gameObject;
        GameObject menuScreens = Menu.transform.Find("MenuScreens").gameObject;

        menus = menuScreens;

        // Get UI Manager UIManagerScript
        UIManagerScript = Menu.GetComponent<UIManager>();

        // Get References to Unit Masses
        yield return new WaitForSecondsRealtime(100);
        
        yield break;
    }
    private void updateMass(GameObject shell, int factor) {
        if (shell != null)  {
            shell.GetComponent<Rigidbody>().mass = 0.2f * numSnapped[factor - 1]; // adds 0.2 in mass for each unit mass snapped to the shell
        }
    }
    /// <summary>
    /// Handles the snapping and unsnapping of masses from the density shells
    /// s - the reference to the shell
    /// factor - the size factor of the shell (larger shell is 1, smaller shell 2), used to divide by 2 on snap placements/radius/etc. for smaller shell
    /// snapArray - list of what masses are snapped to the current shell
    /// </summary>
    private void snapChecker(GameObject s, int factor, bool[] snapArray) {
        /*****************IMPORTANT*****************/
        // When using Emulator, make the following change at all places applicable:
        // !massList[i].GetComponentInParent<HandController>() -> (massList[i].transform.parent == null || massList[i].transform.parent.gameObject.name != "Hand")
        int count = 0;
        // iterating through each mass
        for (int i = 0; i < 8; i++) {
            if (massList[i] != null) { // necessary changes of isKinematic for masses to stay in place
                if ((massList[i].transform.parent == null || (massList[i].GetComponentInParent<HandController>() && shell.transform.parent == null & shell2.transform.parent == null))) { 
                    massList[i].GetComponent<Rigidbody>().isKinematic = false;
                } else {
                    massList[i].GetComponent<Rigidbody>().isKinematic = true;
                }
                if (snapArray[i] == true) { // if mass is marked as snapped
                    if ((massList[i].transform.position - s.transform.position - positionArray[numSnapped[factor - 1] - 1, count] / factor).magnitude > 0.001 && !massList[i].GetComponentInParent<HandController>()) {
                        // if mass is within snapping range and is not being held, snap it back into position
                        if (((massList[i].transform.position - s.transform.position).magnitude < 2 / factor)  && !massList[i].GetComponentInParent<HandController>()) {
                           int count2 = 0;
                           // rearranges the masses
                           for (int j = 0; j < 8; j++) {
                                if (snapArray[j] == true) {
                                    massList[j].transform.position = s.transform.position + positionArray[numSnapped[factor - 1] - 1, count2] / factor;
                                    count2 = count2 + 1;
                                }
                            } 
                        } else { // if mass is marked as snapped but is not within snapping range
                            numSnapped[factor - 1] = numSnapped[factor - 1] - 1; // decrement numSnapped
                            snapArray[i] = false; // set snapArray value to false for this mass
                            int count2 = 0;
                            if (massList[i].transform.parent == s.transform) {
                                massList[i].transform.SetParent(null); 
                            }
                            // rearranges the masses
                            for (int j = 0; j < 8; j++) {
                                if (snapArray[j] == true) {
                                    massList[j].transform.position = s.transform.position + positionArray[numSnapped[factor - 1] -1, count2] / factor;
                                    count2 = count2 + 1;
                                }
                            }
                        }
                    }
                    count = count + 1; // increments count so that we can count only through snapped masses
                } else {
                    // mass is not currently marked as snapped, check if mass is inside range and is not being held
                    if (((massList[i].transform.position - s.transform.position).magnitude  < 2 / factor)  && !massList[i].GetComponentInParent<HandController>() && (s.transform.parent == null)) {
                        numSnapped[factor - 1] = numSnapped[factor - 1] + 1; // increment numSnapped
                        snapArray[i] = true; // set snapArray value to true for this mass
                        massList[i].transform.SetParent(s.transform); // set the shell to be the parent of the mass
                        int count2 = 0;
                        // rearranges the masses
                        for (int j = 0; j < 8; j++) {
                            if (snapArray[j] == true) {
                                massList[j].transform.position = s.transform.position + positionArray[numSnapped[factor - 1] - 1, count2] / factor;
                                count2 = count2 + 1;
                            }
                        }
                    }
                }
            } 
        }
    }
}
