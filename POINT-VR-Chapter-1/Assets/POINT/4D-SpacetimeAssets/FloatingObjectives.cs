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

    private string pastObjectives = "";
    private string currentObjective = "";

    // Start is called before the first frame update
    void Start()
    {
        cameraObject = Camera.allCameras[0];
        this.transform.SetParent(cameraObject.transform);
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
