using UnityEngine;

public class SnapObject : MonoBehaviour
{
    public enum SnapTag
    {
        None = 0,
        MassSort = 1
    };

    public SnapTag snapTag = 0;
    [HideInInspector] public Collider currentAnchor = null;

    private void OnTriggerEnter(Collider other)
    {
        SnapAnchor anchor = other.GetComponent<SnapAnchor>();
        if (anchor && !currentAnchor && !anchor.heldObject && anchor.snapTag == snapTag)
        {
            currentAnchor = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<SnapAnchor>() && other == currentAnchor)
        {
            currentAnchor = null;
        }
    }
}
