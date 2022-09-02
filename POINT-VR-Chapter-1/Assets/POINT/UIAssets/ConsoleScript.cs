using UnityEngine;
using UnityEngine.UI;

public class ConsoleScript : MonoBehaviour
{
    /// <summary>
    /// What the console will reset to when it is cleared
    /// </summary>
    public string startingText;
    /// <summary>
    /// Number of lines printed until the console is wiped
    /// </summary>
    [SerializeField] int lineCap;
    private Text text;
    private int lines;
    void Start()
    {
        text = GetComponent<Text>();
        Clear();
    }
    /// <summary>
    /// Public function that can append a string to the console. Clears every lineCap lines.
    /// </summary>
    public void Log(string s)
    {
        lines++;
        if (lines % lineCap == 0)
        {
            Clear();
        }
        text.text = text.text.Clone() + s + '\n';
    }

    /// <summary>
    /// Public function that clears the console.
    /// </summary>
    public void Clear()
    {
        lines = 1;
        text.text = startingText + '\n';
    }

}
