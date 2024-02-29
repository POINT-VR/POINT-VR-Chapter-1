using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Variables not initialized with private getters in start block because this object is inactive by default.
/// </summary>
public class BinaryToggle : MonoBehaviour, ICollidableGraphic
{
    /// <summary>
    /// What should happen when this toggle is set to On?
    /// </summary>
    public UnityEvent turnedOn;
    /// <summary>
    /// What should happen when this toggle is set to Off?
    /// </summary>
    public UnityEvent turnedOff;
    /// <summary>
    /// Returns whether this variable is On (true) or Off (false).
    /// Setting this invokes the associated event.
    /// </summary>
    public bool IsOn
    {
        get { return _isOn; }
        set
        {
            if (value)
            {
                turnedOn.Invoke();
                _isOn = true;
            }
            else
            {
                turnedOn.Invoke();
                _isOn = false;
            }
        }
    }
    private bool _isOn;
    /// <summary>
    /// Sets this toggle to whichever value it doesn't currently have.
    /// </summary>
    public void OnCast()
    {
        IsOn = !_isOn;
    }
}