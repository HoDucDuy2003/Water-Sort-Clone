using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BottleColorController : MonoBehaviour
{
    private SpriteRenderer _bottleMaskSR;


    private void Awake()
    {
        _bottleMaskSR = GetComponent<BottleController>().BottleMaskSR;
    }
    public void SetFillAmount(float _fillAmount)
    {
        _bottleMaskSR.material.SetFloat(ShaderValueNames.FillAmount, _fillAmount);
    }
    public void SetSARM(float _value)
    {
        _bottleMaskSR.material.SetFloat(ShaderValueNames.SARM, _value);
    }
    public void UpdateColorsOnShader(BottleData bottleData)
    {
        var bottleColors = bottleData.bottleColors;
        _bottleMaskSR.material.SetColor(ShaderValueNames.Color1, bottleColors[0]);
        _bottleMaskSR.material.SetColor(ShaderValueNames.Color2, bottleColors[1]);
        _bottleMaskSR.material.SetColor(ShaderValueNames.Color3, bottleColors[2]);
        _bottleMaskSR.material.SetColor(ShaderValueNames.Color4, bottleColors[3]);
    }
    public void UpdateTopColorValues(BottleData bottleData)
    {
        var bottleColors = bottleData.bottleColors;
        var searchColorLength = bottleColors.Length - (4 - bottleData.numberOfTopColorLayers);
        FindNumberOfTopLayers(searchColorLength, bottleData);
        FindTopColor(searchColorLength, bottleData);
        UpdateColorsOnShader(bottleData);
    }
    public void FindNumberOfTopLayers(int searchColorLength, BottleData bottleData)
    {
        int numberOfTopColorLayers;
        var bottleColors = bottleData.bottleColors;

        if(searchColorLength <= 0)
        {
            numberOfTopColorLayers = 0;
            bottleData.numberOfTopColorLayers = numberOfTopColorLayers;
            return;
        }

        numberOfTopColorLayers = 1;

        for (var i = searchColorLength - 1; i >= 1; i--)
        {
            if (string.Equals(ColorUtility.ToHtmlStringRGB(bottleColors[i]),
                    ColorUtility.ToHtmlStringRGB(bottleColors[i - 1])))
            {
                numberOfTopColorLayers++;
            }
            else
            {
                break;
            }
        }

        bottleData.numberOfTopColorLayers = numberOfTopColorLayers;

    }
    private void FindTopColor(int searchColorLength, BottleData bottleData)
    {
        var assignValue = Math.Clamp(searchColorLength - 1, 0, int.MaxValue);
        bottleData.topColor = bottleData.bottleColors[assignValue];
    }

}
