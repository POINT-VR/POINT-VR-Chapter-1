using TMPro;
using UnityEngine;

public class FloatingObjectives : MonoBehaviour
{
    /// <summary>
    /// Stores the camera which the floating objectives attaches itself too
    /// </summary>
    // private Camera cameraObject;           ***currently testing disattached from camera*** 

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
        // cameraObject = Camera.allCameras[0];              ***currently testing disattached from camera*** 
        // this.transform.SetParent(cameraObject.transform);                 ***currently testing disattached from camera*** 
        this.transform.localPosition = position;
        this.transform.localEulerAngles = new Vector3(0, 90, 0);
        objectiveText.text = "";
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
