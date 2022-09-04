using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpansionConversionData : MonoBehaviour
{
    public int power;
    public int measurementFamily;
    public int currentMeasurement;
    public int roundTo;
    public string trueNumber;
    public TextMeshProUGUI endText;
    public TMP_InputField input;
    public TextMeshProUGUI buttonText;

    private void Start()
    {
        if (buttonText != null && measurementFamily == 2) if (currentMeasurement == 0) buttonText.text = "°C/°K"; else buttonText.text = "°F";
        else if (buttonText != null)
        {
            if (measurementFamily < 2) buttonText.text = (ExpansionConverter.measurements[measurementFamily][currentMeasurement]);
            else buttonText.text = (ExpansionConverter.measurements[measurementFamily - 3][currentMeasurement]);
            if (power != 1) buttonText.text += String.Format("<sup>" + power + "</sup>");
            if (measurementFamily == 3)
            {
                if(currentMeasurement == 0) buttonText.text = "1/°K";
                else buttonText.text = "1/°F";
            }
        }       
    }
}
