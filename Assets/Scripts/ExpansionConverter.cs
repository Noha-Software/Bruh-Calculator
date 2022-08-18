using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ExpansionConverter : MonoBehaviour
{
    public TextMeshProUGUI to;
    public TextMeshProUGUI output;
    public ExpansionConversionData data;
    public Slider slider;
    string[][] measurements = new string[3][];
    float[][] conversions = new float[4][];

    int selectedMeasurementFamily = 0; //Imperial or metric

    /*int[] imperialConversion = new int[] { 12, 3, 1760 };
    int[] metricConversion = new int[] { 10, 10, 10, 1000 };
    float[] metricImperialConversion = new float[] { 0.03937F, 0.3937F, 0.32808F, 1.0936F, 0.621371192F };*/

    private void Start()
    {
        measurements[0] = new string[] { "mm", "cm", "dm", "m", "km" };
        measurements[1] = new string[] { "inch", "feet", "yard", "mile"};
        measurements[2] = new string[] { "°C/°K", "°F"};
        conversions[1] = new float[] { 12, 3, 1760 };
        conversions[0] = new float[] { 10, 10, 10, 1000 };
        conversions[2] = new float[] { };
        conversions[3] = new float[] { 0.03937F, 0.3937F, 0.32808F, 1.0936F, 0.621371192F };
    }

    public void SliderValueChange()
    {
        Debug.Log("End result: " + Convert(data.currentMeasurement, double.Parse(data.testDecimal)));
    }
    public void PageOpened(ExpansionConversionData data)
    {
        if(!data.isTemperature)
        {

        }
        else
        {

        }
    }
    /*public decimal ConvertInterFamily()
    {
        if(selectedMeasurementFamily == data.measurementFamily)
        {

        }
        else
        {

        }
    }*/
    public double Convert(int i, double number)
    {
        Debug.Log(number);
        if (i == slider.value) return number * 1;
        else if (i > slider.value) return Convert(i-1, number * conversions[selectedMeasurementFamily][i - 1]);
        else return Convert(i + 1, number / conversions[selectedMeasurementFamily][i]);
    }
}
