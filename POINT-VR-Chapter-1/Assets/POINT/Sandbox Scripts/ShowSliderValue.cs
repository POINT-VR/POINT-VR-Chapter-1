using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.UIElements;

public class ShowSliderValue : MonoBehaviour
{
    private TextMeshProUGUI title;

    [SerializeField] private string description;
    [SerializeField] private string unit;

    void Start()
    {
        title = gameObject.GetComponent<TextMeshProUGUI>();
        title.text = description;
    }

    public void UpdateSliderValue(float normalizedVal)
    {
        title.text = description + ": " + Convert.ToString(normalizedVal) + " " + unit;
    }
}
