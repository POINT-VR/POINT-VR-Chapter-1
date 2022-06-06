using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit; // Don't know if we need this package
using UnityEngine.InputSystem;

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
        // TODO: Need a way to clear the console text. If there is too much text it overfills
    }

    /// Appends the current controller input to the string s
    public void ControlInput()
    {
        var gamepad = Gamepad.current;
        string s;

        // Temp to see if it prints out to screen and how often
        s = "Hi";

        /// Get Controller Input - TODO

        // Breaks the system
        //if (gamepad.rightTrigger.wasPressedThisFrame)
        //    s = "rightTrigger";

        /// Convert to readable string - TODO

        /// Avoids multiple same texts from being printed to the screen
        if (updatingText == s)
            return;
        else
            updatingText = s;

        // Output to Log
        Log(updatingText);
    }

    void Update()
    {
        // Function prints out Controller Inputs to console
        ControlInput();
    }
}
