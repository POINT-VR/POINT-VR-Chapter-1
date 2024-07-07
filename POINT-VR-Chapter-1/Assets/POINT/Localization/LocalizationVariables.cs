using UnityEngine;

public class LocalizationVariables : MonoBehaviour
{
    [HideInInspector]
    public string version = "";
    
    void Start()
    {
        version = Application.version;
    }
}
