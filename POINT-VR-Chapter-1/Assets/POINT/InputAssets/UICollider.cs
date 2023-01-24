using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// A special component that stores a UnityEvent to be used when the hand's raycast finds the GameObject this is attached to.
/// </summary>
public class UICollider : MonoBehaviour
{
    /// <summary>
    /// What should happen when this UI element is clicked via the raycaster?
    /// </summary>
    public UnityEvent OnCast { get; private set; }
    /// <summary>
    /// What should happen when this UI element is clicked via the raycaster?
    /// </summary>
    [SerializeField] UnityEvent onCast;
    private void Awake()
    {
        OnCast = onCast;
    }
}
