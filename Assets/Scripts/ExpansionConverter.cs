using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ExpansionConverter : MonoBehaviour
{
    public TextMeshProUGUI to;
    public EbelButton interFamilyButton;
    public TextMeshProUGUI from;
    public TextMeshProUGUI output;
    public ExpansionConversionData data;
    public Slider slider;

    string number;

    bool dont;

    string[][] measurements = new string[3][];
    float[][] conversions = new float[4][];

    int selectedMeasurementFamily = 0; //Imperial or metric

    /*int[] imperialConversion = new int[] { 12, 3, 1760 };
    int[] metricConversion = new int[] { 10, 10, 10, 1000 };
    float[] metricImperialConversion = new float[] { 0.03937F, 0.3937F, 0.32808F, 1.0936F, 0.621371192F };*/

    private void Awake()
    {
        measurements[0] = new string[] { "mm", "cm", "dm", "m", "km" };
        measurements[1] = new string[] { "inch", "feet", "yard", "mile"};
        measurements[2] = new string[] { "°C/°K", "°F"};
        conversions[1] = new float[] { 12, 3, 1760 };
        conversions[0] = new float[] { 10, 10, 10, 1000 };
        conversions[2] = new float[] { 0.03937F, 0.3937F, 0.32808F, 1.0936F, 0.621371192F };
    }

    public void SliderValueChange()
    {
        if (!dont)
        {
            to.text = measurements[selectedMeasurementFamily][(int)slider.value];
            output.text = Convert(data.currentMeasurement, double.Parse(number)).ToString();
        }
    }
    public void PageOpened(ExpansionConversionData ecd)
    {
        if (ecd.input.text != "" || (ecd.endText != null && ecd.endText.text != ""))
        {
            this.gameObject.SetActive(true);
            dont = true;
            data = ecd;
            selectedMeasurementFamily = data.measurementFamily;
            Debug.Log(selectedMeasurementFamily);
            if (data.input.text != "")
            {
                output.text = data.input.text;
                number = data.input.text;
            }
            else
            {
                output.text = data.endText.text;
                number = data.endText.text;
            }
            if (data.measurementFamily == 2)  
            slider.value = data.currentMeasurement;
            to.text = "";
            from.text = measurements[selectedMeasurementFamily][data.currentMeasurement];
            slider.maxValue = measurements[selectedMeasurementFamily].Length - 1;
            dont = false;
        }
    }
    public void ClosePage()
    {
        this.gameObject.SetActive(false);
    }
    public void ConvertInterFamily()
    {
        dont = true;
        int helpInt = (int)slider.value;
        Debug.Log("convertInterFamily: " + selectedMeasurementFamily);
        if (selectedMeasurementFamily == 0)
        {
            Debug.Log("Metric => imperial");
            selectedMeasurementFamily = 1;
            slider.maxValue = measurements[selectedMeasurementFamily].Length - 1;
            if (slider.maxValue > slider.value && slider.value > 1)
            {               
                slider.value -= 1;
                number = (double.Parse(output.text) * (double)conversions[2][helpInt]).ToString();                
            }
            else if (slider.maxValue == slider.value)
            {
                number = (double.Parse(output.text) * (double)conversions[2][helpInt]).ToString();
            }
            else 
            {
                slider.value = 0;
                number = (double.Parse(output.text) * (double)conversions[2][helpInt]).ToString();
            }
        }
        else
        {
            Debug.Log("Imperial => metric");
            selectedMeasurementFamily = 0;
            slider.maxValue = measurements[selectedMeasurementFamily].Length - 1;
            if ((int)slider.maxValue - 1 > slider.value && slider.value > 1)
            {                
                number = (double.Parse(output.text) / (double)conversions[2][helpInt]).ToString();
            }
            else if(slider.value <= 1)
            {
                slider.value = 1;
                number = (double.Parse(output.text) / (double)conversions[2][1]).ToString();
            }
            else
            {
                slider.value = slider.maxValue;
                number = (double.Parse(output.text) / (double)conversions[2][helpInt]).ToString();
            }
        }        
        dont = false;
        to.text = measurements[selectedMeasurementFamily][(int)slider.value];
        output.text = number;
    }
    public double Convert(int i, double number)
    {
        //Debug.Log(number);
        if (i == slider.value) return number * 1;
        else if (i > slider.value) return Convert(i-1, number * conversions[selectedMeasurementFamily][i - 1]);
        else return Convert(i + 1, number / conversions[selectedMeasurementFamily][i]);
    }
}
