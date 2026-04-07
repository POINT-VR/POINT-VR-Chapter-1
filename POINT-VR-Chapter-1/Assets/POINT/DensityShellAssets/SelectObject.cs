using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour
{
    // This will be the checkbox visible in the Inspector
    public bool Selected = false;

    // You can add methods to interact with this variable
    public void SetSelected(bool value)
    {
        Selected = value;
    }
    public bool GetSelected()
    {
        return Selected;
    }
}
