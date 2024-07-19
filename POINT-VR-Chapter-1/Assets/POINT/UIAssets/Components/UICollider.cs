using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// A special component that stores a UnityEvent to be used when the hand's raycast finds the GameObject this is attached to.
/// </summary>
public class UICollider : MonoBehaviour, ICollidableGraphic
{
    /// <summary>
    /// What should happen when this UI element is clicked via the raycaster?
    /// </summary>
    [SerializeField] UnityEvent onCast;
    /// <summary>
    /// Invokes the UI onCast event
    /// </summary>
    public void OnCast()
    {
        onCast.Invoke();
    }
}
