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
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForPlayerSpawn());
    }
    private IEnumerator WaitForPlayerSpawn()
    {
        yield return new WaitUntil(() => Camera.current != null);

        // Start menu initialization
        currentCamera = Camera.current;
        player = currentCamera.transform.parent.gameObject;

        StartCoroutine(StartScene());
    }
    void Update()
    {
        
    }
    IEnumerator StartScene()
    {
        //Instantiate menus from player prefab and buttons from player prefab as well
        GameObject mainCamera = player.transform.Find("Main Camera").gameObject;
        GameObject UIContainer = mainCamera.transform.Find("UI Container").gameObject;
        GameObject Menu = UIContainer.transform.Find("Menu").gameObject;
        GameObject HeaderButtons = Menu.transform.Find("HeaderButtons").gameObject;
        // GameObject HeaderButtons = Menu.transform.Find("Buttons").gameObject; //if testing with emulator use this
        GameObject menuScreens = Menu.transform.Find("MenuScreens").gameObject;

        menus = menuScreens;
        buttons = HeaderButtons;

        // Get UI Manager UIManagerScript
        UIManagerScript = Menu.GetComponent<UIManager>();
        
        yield return new WaitForSecondsRealtime(100);
        
        yield break;
    }

}
