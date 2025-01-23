using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class DensityExploration : MonoBehaviour
{
    // Serialized fields
    [Header("References")]
    [SerializeField] private GameObject floor;
    [SerializeField] private InputActionReference openMenuReference;
    // Cache
    private Camera currentCamera = null;
    private GameObject player = null;
    private GameObject menus = null;
    private GameObject buttons = null;
    private UIManager UIManagerScript = null;
    // Masses
    private GameObject shell = null;
    private GameObject mass1 = null;
    private GameObject mass2 = null;
    private GameObject mass3 = null;
    // Array of Snap Locations
    Vector3[,] positionArray = new Vector3[3, 3];

    // Number of snapped masses
    private int numSnapped = 0;
    // List of what masses are snapped
    bool[] snapArray = new bool[3];
    GameObject[] massList = new GameObject[3];
    private
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForPlayerSpawn());
    }
    // When using emulator do the following change:
    // !mass1.GetComponentInParent<HandController>() -> (mass1.transform.parent == null || mass1.transform.parent.gameObject.name != "Hand")
    private IEnumerator WaitForPlayerSpawn()
    {
        yield return new WaitUntil(() => Camera.current != null);

        // Start menu initialization
        currentCamera = Camera.current;
        player = currentCamera.transform.parent.gameObject;
        mass1 = GameObject.Find("UnitMassOne");
        mass2 = GameObject.Find("UnitMassTwo");
        mass3 = GameObject.Find("UnitMassThree");
        massList[0] = GameObject.Find("UnitMassOne");
        massList[1] = GameObject.Find("UnitMassTwo");
        massList[2] = GameObject.Find("UnitMassThree");
        shell = GameObject.Find("Sphere");
        // Fill Snap List
        positionArray[0, 0] = new Vector3(0f, 0f, 0f);
        positionArray[1, 0] = new Vector3(-0.75f, 0f, 0f);
        positionArray[1, 1] = new Vector3(0.75f, 0f, 0f);
        positionArray[2, 0] = new Vector3(-0.75f, -0.75f, 0f);
        positionArray[2, 1] = new Vector3(0.75f, -0.75f, 0f);
        positionArray[2, 2] = new Vector3(0f, 0.75f, 0f);
        //
        snapArray[0] = false;
        snapArray[1] = false;
        snapArray[2] = false;
        //
        numSnapped = 0;
        StartCoroutine(StartScene());
    }
    void Update()
    {
        // Debug.Log(numSnapped);
        snapChecker();
    }
    IEnumerator StartScene()
    {
        //Instantiate menus from player prefab and buttons from player prefab as well
        GameObject mainCamera = player.transform.Find("Main Camera").gameObject;
        GameObject UIContainer = mainCamera.transform.Find("UI Container").gameObject;
        GameObject Menu = UIContainer.transform.Find("Menu").gameObject;
        // GameObject HeaderButtons = Menu.transform.Find("HeaderButtons").gameObject;
        // GameObject HeaderButtons = Menu.transform.Find("Buttons").gameObject; //if testing with emulator use this
        GameObject menuScreens = Menu.transform.Find("MenuScreens").gameObject;

        menus = menuScreens;
        // buttons = HeaderButtons;

        // Get UI Manager UIManagerScript
        UIManagerScript = Menu.GetComponent<UIManager>();

        // Get References to Unit Masses
        yield return new WaitForSecondsRealtime(100);
        
        yield break;
    }
    private void snapChecker() {
        // check the position of each mass
        int count = 0;
        for (int i = 0; i < 3; i++) {
            if (massList[i] != null) {
                // if the mass is marked as snapped, check if it has moved from its expected position
                if (snapArray[i] == true) {
                    if ((massList[i].transform.position != positionArray[numSnapped - 1, count]) && (massList[i].transform.parent == null || massList[i].transform.parent.gameObject.name != "Hand")) {
                        // if mass is not in expected position
                        numSnapped = numSnapped - 1;
                        snapArray[i] = false;
                        massList[i].transform.SetParent(null);
                        int count2 = 0;
                        for (int j = 0; j < 3; j++) {
                            if (snapArray[j] == true) {
                                massList[j].transform.position = positionArray[numSnapped -1, count2];
                                massList[j].GetComponent<Rigidbody>().isKinematic = false;
                                count2 = count2 + 1;
                            }
                        }
                    }
                    count = count + 1;
                } else {
                    // check if mass is inside range
                    if ((massList[i].transform.position.magnitude < 1.5)  && (massList[i].transform.parent == null || massList[i].transform.parent.gameObject.name != "Hand")) {
                        numSnapped = numSnapped + 1;
                        snapArray[i] = true;
                        massList[i].transform.SetParent(shell.transform);
                        int count2 = 0;
                        for (int j = 0; j < 3; j++) {
                            Debug.Log(numSnapped);
                            Debug.Log(snapArray[j]);
                            Debug.Log(j);
                            if (snapArray[j] == true) {
                                massList[j].transform.position = positionArray[numSnapped - 1, count2];
                                massList[j].GetComponent<Rigidbody>().isKinematic = true;
                                count2 = count2 + 1;
                            }
                        }
                    }
                }
            }
            
        }
    }
}
