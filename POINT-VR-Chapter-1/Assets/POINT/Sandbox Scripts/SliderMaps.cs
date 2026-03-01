using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class SliderMaps : MonoBehaviour
{
    [SerializeField] private ShowSliderValue ssv;
    [SerializeField] private AdjustMassAndRadius connectedMass;


    // Map from slider value (0 to 1) to simulated radius (1 to 5 in these discrete steps: 1, 1.2, 1.4, ... 5).
    // This is then converted to (0.5 to 2.5) in-game.

    public void MapSliderToRadiusInput(float sliderValue)
    {
        int discreteValue = Mathf.RoundToInt(sliderValue * 21);

        float mappedValue = 5f; // maximum value (if discreteValue is 21 exactly)

        if (discreteValue == 0)
            mappedValue = 1f;
        else if (discreteValue != 21)
            mappedValue = 1f + (discreteValue - 1) * 0.2f;

        ssv.UpdateSliderValue(mappedValue);
        connectedMass.SetRadius(mappedValue / 2f);
    }

    // Map from slider value (0 to 1) to discrete value (0 to 31) to simulated mass (0.25 to 0.5, 0.5 to 1, 1 to 5).
    // This simulated mass is then cut to 1/4 its displayed value.


    /* MAP FROM DISCRETE SLIDER VALUE TO DISPLAYED MASS VALUE:
    Value of 0 --> 0.25
    1 to 7 --> 0.25 to 0.5, incrementing by 0.05
    8 to 14 --> 0.5 to 1, incrementing by 0.1
    15 to 25 --> 1 to 3, incrementing by 0.2
    26 to 30 --> 3 to 5, incrementing by 0.5
    Value of 31 --> 5.
    */

    public void MapSliderToMassInput(float sliderValue)
    {
        int discreteValue = Mathf.RoundToInt(sliderValue * 31);

        float mappedValue = 5f; // maximum value

        if (discreteValue == 0)
            mappedValue = 0.25f;
        else if (discreteValue <= 7)
            mappedValue = 0.25f + (discreteValue - 1) * 0.05f;
        else if (discreteValue <= 14)
            mappedValue = 0.5f + (discreteValue - 8) * 0.1f;
        else if (discreteValue <= 25)
            mappedValue = 1f + (discreteValue - 15) * 0.2f;
        else if (discreteValue <= 30)
            mappedValue = 3f + (discreteValue - 26) * 0.5f;

        ssv.UpdateSliderValue(mappedValue);
        connectedMass.SetMass(mappedValue / 4f);
    }
}
