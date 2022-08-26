using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpansionConversionData : MonoBehaviour
{
    public int power;
    public int measurementFamily;
    public int currentMeasurement;
    public TextMeshProUGUI endText;
    public TMP_InputField input;
    public TextMeshProUGUI buttonText;

    private void Start()
    {
        if (buttonText != null && measurementFamily == 2) if (currentMeasurement == 0) buttonText.text = "°C/°K"; else buttonText.text = "°F";
        else if (buttonText != null) buttonText.text = ExpansionConverter.measurements[measurementFamily][currentMeasurement];
    }
}
