using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpansionConversionData : MonoBehaviour
{
    public int power;
    public int toFamily;
    public int toMeasurement;
    public int currentFamily;
    public int currentMeasurement;
    public int roundTo;
    public string trueNumber;
    public TextMeshProUGUI endText;
    public TMP_InputField input;
    public TextMeshProUGUI buttonText;

    private void Start()
    {
        toFamily = currentFamily;
        toMeasurement = currentMeasurement;        
        buttonText.text = (ExpansionConverter.measurements[toFamily][toMeasurement]);            
        if (power != 1) buttonText.text += String.Format("<sup>" + power + "</sup>");            
              
    }
}
