using UnityEngine;
using UnityEngine.Localization;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DisplayVersion : MonoBehaviour
{
    [SerializeField] private LocalizedString versionDisplayString;

    private TextMeshProUGUI textComponent;
    private string version;

    private void OnEnable()
    {
        textComponent = this.GetComponent<TextMeshProUGUI>();
        versionDisplayString.Arguments = new object[] { version };
        versionDisplayString.StringChanged += UpdateText;
        GetVersionNumber();
    }

    private void OnDisable()
    {
        versionDisplayString.StringChanged -= UpdateText;
    }

    private void UpdateText(string value)
    {
        textComponent.text = value;
    }

    private void GetVersionNumber()
    {
        version = Application.version;
        versionDisplayString.Arguments[0] = version;
        versionDisplayString.RefreshString();
    }
}
