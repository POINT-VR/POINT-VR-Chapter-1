using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FloatingObjectives : MonoBehaviour
{
    private Camera cameraObject;
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

    public void NewObjective(string newObjective)
    {
        if (currentObjective != "")
        {
            pastObjectives += currentObjective + "\n";
        }
        currentObjective = newObjective;
        objectiveText.text = $"<color=grey>{pastObjectives}</color><color=white>{currentObjective}</color>"; //TMP_Text formatting shenanigenry
    }
}
