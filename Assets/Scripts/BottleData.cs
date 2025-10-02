using System.Collections;
using UnityEngine;

public class BottleData : MonoBehaviour
{
    public Color[] bottleColors;

    [Range(0, 4)]
    public int numberOfColorsInBottle = 4;

    public Color topColor;
    public int numberOfTopColorLayers = 1;

    public Color previousTopColor;

    public void IncreaseNumberOfColorsInBottle(int number)
    {
        numberOfColorsInBottle += number;
    }
    public void DecreaseNumberOfColorsInBottle(int number)
    {
        numberOfColorsInBottle -= number;
    }
    public void UpdatePreviousTopColor()
    {
        previousTopColor = topColor;
    }
}
