using UnityEngine;
using UnityEngine.InputSystem;
public class EditorPlayerRotator : MonoBehaviour
{
    [SerializeField] InputActionReference editorPlayerRotatationOverride;
    [SerializeField] float sensY;
    [SerializeField] float sensX;
#if UNITY_EDITOR //This test script is only for the editor
    private void OnEnable()
    {
        editorPlayerRotatationOverride.action.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnDisable()
    {
        editorPlayerRotatationOverride.action.Disable();
        Cursor.lockState = CursorLockMode.None;
    }
    void LateUpdate() //One thing to keep in mind: this does rotate the whole playerbase. While not elegant, it is simple for editor testing.
    {
        Vector2 angle = editorPlayerRotatationOverride.action.ReadValue<Vector2>().normalized;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - angle.y * sensY, transform.rotation.eulerAngles.y + angle.x * sensX, 0f);
    }
#endif
}
