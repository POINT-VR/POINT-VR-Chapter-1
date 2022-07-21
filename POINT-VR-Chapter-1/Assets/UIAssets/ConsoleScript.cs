using UnityEngine;
using UnityEngine.UI;

public class ConsoleScript : MonoBehaviour
{
    public string startingText;
    public Text objectToMutate;
    private int lines;
    void Start()
    {
        Clear();
    }
    /// <summary>
    /// Public function that can append a string to the console. Clears every 7 lines.
    /// </summary>
    public void Log(string s)
    {
        lines++;
        if (lines % 7 == 0)
        {
            Clear();
        }
        objectToMutate.text = objectToMutate.text.Clone() + s + '\n';
    }

    /// <summary>
    /// Public function that clears the console.
    /// </summary>
    public void Clear()
    {
        lines = 1;
        objectToMutate.text = startingText + '\n';
    }

}
