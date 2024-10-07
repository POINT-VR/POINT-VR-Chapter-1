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
    [SerializeField] private TMP_Text objectiveText;

    [SerializeField] private Vector3 position;

    private System.Collections.Generic.List<string> objectives;

    private void Start()
    {
        cameraObject = Camera.allCameras[0]; 
        // this.transform.SetParent(cameraObject.transform);             ***testing with detached camera***
        // this.transform.localPosition = position;            ***testing with detached camera***
        this.transform.localEulerAngles = new Vector3(0, 90, 0);
        objectiveText.text = "";
        objectives = new System.Collections.Generic.List<string>();
    }

    private void Update()
    { 
        // Updates Objective Menu to always face Camera
        this.transform.LookAt(cameraObject.transform);
        transform.Rotate(0.0f, 270.0f, 0.0f);
    }
    
    public int NewObjective(string newObjective) //Takes a new objective and appends it to the list of previous ones
    {
        objectives.Add(newObjective);
        ReloadObjectives();

        return objectives.Count - 1;
    }

    public void UpdateObjectiveLanguage(int idx, string s)
    {
        if (idx >= 0 && idx < objectives.Count)
        {
            objectives[idx] = s;
            ReloadObjectives();
        }
    }

    private void ReloadObjectives()
    {
        objectiveText.text = "";
        for (int i = 0; i < objectives.Count; i++)
        {
            string colorTag = (i == objectives.Count - 1) ? "<color=white>" : "<color=grey>";
            objectiveText.text += colorTag + objectives[i] + "</color>" + "\n";
        }
    }
}
