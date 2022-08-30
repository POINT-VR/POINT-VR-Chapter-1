using UnityEngine;
using UnityEngine.Events;

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
