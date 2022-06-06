using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleScript : MonoBehaviour
{
    public string startingText;
    public string updatingText;
    [SerializeField]
    public Text objectToMutate;
    void Start()
    {
        objectToMutate.text = startingText + '\n';
    }
    /// <summary>
    /// Public function that can append a string to the console.
    /// </summary>

    public void Log(string s)
    {
        objectToMutate.text = objectToMutate.text.Clone() + s + '\n';
    }

    /// Appends the current controller input to the string s
    public void ControlInput()
    {
        string s;

        // Temp to see if it prints out to screen and how often
        s = "Hi";

        // Avoids multiple same texts from being printed to the screen
        if (updatingText == s)
            return;
        else
            updatingText = s;

        // Get Controller Input - TODO

        // Convert to readable string - TODO

        // Output to Log
        Log(updatingText);
    }

    void Update()
    {
        // Function prints out Controller Inputs to console
        ControlInput();
    }
}
