using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToggleMassLabels : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> massLabels = new List<TextMeshProUGUI>(6);

    // Start is called before the first frame update
    void Start()
    {
        LabelsOn();
    }

    public void LabelsOn()
    {
        foreach (TextMeshProUGUI label in massLabels)
        {
            label.enabled = true;
        }
    }

    public void LabelsOff()
    {
        foreach (TextMeshProUGUI label in massLabels)
        {
            label.enabled = false;
        }
    }
}

