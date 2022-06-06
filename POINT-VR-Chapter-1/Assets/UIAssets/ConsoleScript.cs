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
        Clear();
    }
    /// <summary>
    /// Public function that can append a string to the console.
    /// </summary>
    public void Log(string s)
    {
        objectToMutate.text = objectToMutate.text.Clone() + s + '\n';
    }

    /// <summary>
    /// Public function that clears the console.
    /// </summary>
    public void Clear()
    {
        objectToMutate.text = startingText + '\n';
    }

}
