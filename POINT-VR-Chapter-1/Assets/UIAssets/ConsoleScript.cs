using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleScript : MonoBehaviour
{
    public string startingText;
    [SerializeField]
    public Text objectToMutate;
    void Start()
    {
        objectToMutate.text = startingText + '\n';
    }
    public void Log(string s)
    {
        objectToMutate.text = objectToMutate.text.Clone() + s + '\n';
    }
}
