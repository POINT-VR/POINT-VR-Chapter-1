using UnityEngine;
using UnityEngine.UI;

public class ToggleImageComponent : MonoBehaviour
{
    /// <summary>
    /// Toggles this gameobject's Image component
    /// </summary>
    public void Toggle()
    {
        gameObject.GetComponent<Image>().enabled = !gameObject.GetComponent<Image>().enabled;
    }
}
