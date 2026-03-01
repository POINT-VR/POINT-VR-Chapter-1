using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignSandboxCamera : MonoBehaviour
{
    [SerializeField] private List<Canvas> worldCanvasList;

    private void Awake()
    {
        Camera playerCamera = GetComponentInChildren<Camera>();

        if (playerCamera != null)
        {
            foreach (Canvas wc in worldCanvasList)
            {
                wc.worldCamera = playerCamera;
            }
        }
    }
}
