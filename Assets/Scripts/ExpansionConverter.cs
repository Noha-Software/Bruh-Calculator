using System;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ExpansionConverter : MonoBehaviour
{
    public TextMeshProUGUI to;
    public EbelButton interFamilyButton;
    public TextMeshProUGUI from;
    public TextMeshProUGUI output;
    public TextMeshProUGUI roundText;
    public ExpansionConversionData data;
    public Slider slider;
    public Slider roundSlider;

    ThermalExpansion calculator;
    string number;

    bool dont;

    int helpInt;

    static public string[][] measurements = new string[2][];
    static public decimal[][] conversions = new decimal[3][];

    int selectedMeasurementFamily = 0;
    int currentMeasurement;

    public static void Define()
    {
        measurements[0] = new string[] { "mm", "cm", "dm", "m", "km" };
        measurements[1] = new string[] { "inch", "feet", "yard", "mile"};
        conversions[1] = new decimal[] { 12, 3, 1760 };
        conversions[0] = new decimal[] { 10, 10, 10, 1000 };
        conversions[2] = new decimal[] { 0.0393700787M, 0.393700787M, 0.32808399M, 1.0936133M, 0.621371192M };
    }
    public void SliderValueChange()
    {        
        if (!dont)
        {
            to.text = measurements[selectedMeasurementFamily][(int)slider.value] + "<sup>" + data.power + "</sup>";
            if (data.input != null) output.text = Convert(currentMeasurement, decimal.Parse(number)).ToString();            
            else output.text = calculator.Calculate(selectedMeasurementFamily, (int)slider.value).ToString();            
        }
    }
    public void PageOpened(ExpansionConversionData ecd)
    {
        if (ecd != null && ((ecd.input != null && ecd.input.text != "") || (ecd.endText != null && ecd.endText.text != "")))
        {
            data = ecd;
            if (data.input != null) number = data.trueNumber;
            else number = data.trueNumber;
            if (data.trueNumber != "") number = data.trueNumber;
            else
            {
                if (data.input != null) number = data.input.text;
                else number = data.endText.text;
            }
            roundSlider.value = data.roundTo;
            selectedMeasurementFamily = data.measurementFamily;
            currentMeasurement = data.currentMeasurement;
            if (ecd.measurementFamily != 2)
            {
                this.gameObject.SetActive(true);
                dont = true;
                to.text = "";
                from.text = measurements[selectedMeasurementFamily][data.currentMeasurement];
                slider.maxValue = measurements[selectedMeasurementFamily].Length - 1;
                slider.value = data.currentMeasurement;
                output.text = Round().ToString();
                dont = false;
            }
            else
            {
                if(data.currentMeasurement == 0)
                {
                    if (data.input.text != "") data.input.text = (decimal.Parse(data.input.text)*9/5 + 32).ToString();
                    else if (data.endText.text != "") data.input.text = (decimal.Parse(data.input.text)* 9/5 + 32).ToString();
                    else return;
                    data.currentMeasurement = 1;
                    data.buttonText.text = "°F";
                }
                else
                {
                    if (data.input.text != "") data.input.text = ((decimal.Parse(data.input.text) - 32) * 5 / 9).ToString();
                    else if (data.endText.text != "") data.input.text = ((decimal.Parse(data.input.text) - 32) * 5 / 9).ToString();
                    else return;
                    data.currentMeasurement = 0;
                    data.buttonText.text = "°C/°K";
                }
            }   
        }
    }
    public void ClosePage()
    {
        this.gameObject.SetActive(false);
    }
    public void SaveConversion()
    {
        data.measurementFamily = selectedMeasurementFamily;
        data.currentMeasurement = (int)slider.value;
        data.buttonText.text = measurements[selectedMeasurementFamily][(int)slider.value] + "<sup>"+data.power+"</sup>";
        if (data.input != null) data.input.text = Round().ToString();
        else data.endText.text = Round().ToString();
        data.trueNumber = number;
        data.roundTo = (int)roundSlider.value;
    }
    public void ConvertInterFamily()
    {
        helpInt = (int)slider.value;
        dont = true;
        if(selectedMeasurementFamily == 0)
        {
            number = ((Convert(currentMeasurement ,decimal.Parse(number)) * Pow(conversions[2][helpInt], data.power))).ToString();
            if (slider.value > 0) slider.value -= 1;           
            if (currentMeasurement > 0) currentMeasurement -= 1;
            slider.maxValue -= 1;
            selectedMeasurementFamily = 1;
            currentMeasurement = (int)slider.value;
        }   
        else
        {
            number = (Convert(currentMeasurement, decimal.Parse(number)) / Pow(conversions[2][helpInt + 1], data.power)).ToString();
            slider.maxValue += 1;
            slider.value += 1;
            currentMeasurement = (int)slider.value;
            selectedMeasurementFamily = 0;
        }
        dont = false;
        if(data.input == null) number = calculator.Calculate(selectedMeasurementFamily, (int)slider.value).ToString();
        output.text = Round().ToString();
        to.text = measurements[selectedMeasurementFamily][(int)slider.value];
    }
    public decimal Convert(int i, decimal number)
    {
        if (selectedMeasurementFamily != 2)
        {
            if (i == slider.value) return number * 1;
            else if (i > slider.value) return Convert(i - 1, number * Pow(conversions[selectedMeasurementFamily][i - 1], data.power));
            else return Convert(i + 1, number / Pow(conversions[selectedMeasurementFamily][i], data.power));
        }
        else
        {
            if (data.currentMeasurement == 0)return decimal.Parse(data.input.text) * 9 / 5 + 32;
            else return (decimal.Parse(data.input.text) - 32) * 5 / 9;
        }
    }
    public void SetCalculator(ThermalExpansion te)
    {
        calculator = te;
    }
    public void RoundSldierValueChanged()
    {
        if (!dont)
        {
            output.text = Round().ToString();
        }
    }
    public decimal Round()
    {        
        if (roundSlider.value != -1)
        {
            roundText.text = "Round to " + roundSlider.value + " digits";
            return Math.Round(Convert(currentMeasurement, decimal.Parse(number)), (int)roundSlider.value, MidpointRounding.AwayFromZero);
        }
        else
        {
            roundText.text = "Do not round";
            return Convert(currentMeasurement, decimal.Parse(number));
        }
    }
    static decimal Pow(decimal x, decimal y)
    {
        if (y == 1) return x * 1;   
        else return x * Pow(x, y - 1);
    }
}
