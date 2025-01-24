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
    Vector3[,] positionArray = new Vector3[8, 8];

    // Number of snapped masses
    private int numSnapped = 0;
    // List of what masses are snapped
    bool[] snapArray = new bool[8];
    GameObject[] massList = new GameObject[8];
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
        massList[0] = GameObject.Find("UnitMassOne");
        massList[1] = GameObject.Find("UnitMassTwo");
        massList[2] = GameObject.Find("UnitMassThree");
        massList[3] = GameObject.Find("UnitMassFour");
        massList[4] = GameObject.Find("UnitMassFive");
        massList[5] = GameObject.Find("UnitMassSix");
        massList[6] = GameObject.Find("UnitMassSeven");
        massList[7] = GameObject.Find("UnitMassEight");
        shell = GameObject.Find("Sphere");
        // Fill Snap List
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
        //
        snapArray[0] = false;
        snapArray[1] = false;
        snapArray[2] = false;
        snapArray[3] = false;
        snapArray[4] = false;
        snapArray[5] = false;
        snapArray[6] = false;
        snapArray[7] = false;
        //
        numSnapped = 0;
        StartCoroutine(StartScene());
    }
    void Update()
    {
        // Debug.Log(numSnapped);
        snapChecker();
        updateMass();
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
    private void updateMass() {
        shell.GetComponent<Rigidbody>().mass = 0.2f * numSnapped;
    }
    private void snapChecker() {
        // check the position of each mass
        int count = 0;
        for (int i = 0; i < 8; i++) {
            if (massList[i] != null) {
                // if the mass is marked as snapped, check if it has moved from its expected position
                if (snapArray[i] == true) {
                    if ((massList[i].transform.position - shell.transform.position - positionArray[numSnapped - 1, count]).magnitude > 0.001 && (massList[i].transform.parent == null || massList[i].transform.parent.gameObject.name != "Hand")) {
                        if (((massList[i].transform.position - shell.transform.position).magnitude < 2)  && (massList[i].transform.parent == null || massList[i].transform.parent.gameObject.name != "Hand")) {
                           int count2 = 0;
                           for (int j = 0; j < 8; j++) {
                                if (snapArray[j] == true) {
                                    massList[j].transform.position = shell.transform.position + positionArray[numSnapped - 1, count2];
                                    massList[j].GetComponent<Rigidbody>().isKinematic = true;
                                    count2 = count2 + 1;
                                }
                            } 
                        } else {
                            numSnapped = numSnapped - 1;
                            snapArray[i] = false;
                            massList[i].transform.SetParent(null);
                            int count2 = 0;
                            for (int j = 0; j < 8; j++) {
                                if (snapArray[j] == true) {
                                    massList[j].transform.position = shell.transform.position + positionArray[numSnapped -1, count2];
                                    massList[j].GetComponent<Rigidbody>().isKinematic = false;
                                    count2 = count2 + 1;
                                }
                            }
                        }
                        Debug.Log("hi");
                    }
                    count = count + 1;
                } else {
                    // check if mass is inside range
                    if (((massList[i].transform.position - shell.transform.position).magnitude  < 2)  && (massList[i].transform.parent == null || massList[i].transform.parent.gameObject.name != "Hand")) {
                        numSnapped = numSnapped + 1;
                        snapArray[i] = true;
                        massList[i].transform.SetParent(shell.transform);
                        int count2 = 0;
                        for (int j = 0; j < 8; j++) {
                            if (snapArray[j] == true) {
                                massList[j].transform.position = shell.transform.position + positionArray[numSnapped - 1, count2];
                                massList[j].GetComponent<Rigidbody>().isKinematic = true;
                                count2 = count2 + 1;
                            }
                        }
                    }
                }
            }
            
        }
    }
    int getSnapNumber() {
        return numSnapped;
    }
}
