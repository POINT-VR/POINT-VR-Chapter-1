using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGridRange : MonoBehaviour
{
    [SerializeField] private GameObject freeGrid;
    [SerializeField] private GameObject constrainedGrid;

    [SerializeField] private GameObject allowTwoMassesToggle;


    // Start is called before the first frame update
    void Start()
    {
        ConstraintOn();
    }

    public void ConstraintOn()
    {
        constrainedGrid.SetActive(true);
        freeGrid.SetActive(false);
        allowTwoMassesToggle.SetActive(true);
    }

    public void ConstraintOff()
    {
        constrainedGrid.SetActive(false);
        freeGrid.SetActive(true);
        allowTwoMassesToggle.SetActive(false);
    }
}
