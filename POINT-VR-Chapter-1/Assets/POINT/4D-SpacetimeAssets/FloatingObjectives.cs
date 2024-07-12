using TMPro;
using UnityEngine;

public class FloatingObjectives : MonoBehaviour
{
    /// <summary>
    /// Stores the camera which the floating objectives attaches itself too
    /// </summary>
    private Camera cameraObject;

    /// <summary>
    /// The text object which displays the objectives
    /// </summary>
    [SerializeField]
    private TMP_Text objectiveText;

    [SerializeField]
    private Vector3 position;

    private string pastObjectives = "";
    private string currentObjective = "";

    // Start is called before the first frame update
    void Start()
    {
        cameraObject = Camera.allCameras[0]; 
        // this.transform.SetParent(cameraObject.transform);             ***testing with detached camera***
        // this.transform.localPosition = position;            ***testing with detached camera***
        this.transform.localEulerAngles = new Vector3(0, 90, 0);
        objectiveText.text = "";
    }
    void Update()
    { 
        // Updates Objective Menu to always face Camera
        this.transform.LookAt(cameraObject.transform);
        transform.Rotate(0.0f, 270.0f, 0.0f);
    }
    public void NewObjective(string newObjective) //Takes a new objective and appends it to the list of previous ones
    {
        if (currentObjective != "")
        {
            pastObjectives += currentObjective + "\n";
        }
        currentObjective = newObjective;
        objectiveText.text = $"<color=grey>{pastObjectives}</color><color=white>{currentObjective}</color>"; //TMP_Text formatting shenanigenry
    }
}
