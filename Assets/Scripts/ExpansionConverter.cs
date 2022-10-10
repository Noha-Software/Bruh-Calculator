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
    bool dont2;

    int helpInt;

    static public string[][] measurements = new string[4][];
    static public decimal[][] conversions = new decimal[4][];

    int selectedMeasurementFamily = 0;
    int currentMeasurement;

    public static void Define()
    {
        measurements[0] = new string[] { "mm", "cm", "dm", "m", "km" };
        measurements[1] = new string[] { "inch", "feet", "yard", "mile"};
        measurements[2] = new string[] { "°C", "°F" };
        measurements[3] = new string[] { "1/°C", "1/°F" };
        conversions[1] = new decimal[] { 12, 3, 1760 };
        conversions[0] = new decimal[] { 10, 10, 10, 1000 };
        conversions[2] = new decimal[] { (decimal)5/9 , (decimal)5/9};
        conversions[3] = new decimal[] { 0.0393700787M, 0.393700787M, 0.32808399M, 1.0936133M, 0.621371192M };
    }
    public void SliderValueChange()
    {        
        if (!dont)
        {
            to.text = measurements[selectedMeasurementFamily][(int)slider.value];
            if (data.power != 1 && selectedMeasurementFamily < 2) to.text += "<sup>" + data.power + "</sup>";
            if (data.input != null) output.text = Round(decimal.Parse(number)).ToString();            
            else output.text = Round(calculator.Calculate(selectedMeasurementFamily, (int)slider.value, calculator.typeOfCalculation)).ToString();
        }
    }
    public void PageOpened(ExpansionConversionData ecd)
    {
        if (ecd != null && ((ecd.input != null && ecd.input.text != "") || (ecd.endText != null && ecd.endText.text != "")))
        {
            data = ecd;
            roundSlider.value = data.roundTo;
            selectedMeasurementFamily = data.measurementFamily;
            currentMeasurement = data.currentMeasurement;            
            if (data.input != null && data.trueNumber != "" && Round(decimal.Parse(data.trueNumber)) == decimal.Parse(data.input.text)) number = data.trueNumber;            
            else if (data.endText != null && data.trueNumber != "" && Round(decimal.Parse(data.trueNumber)) == decimal.Parse(data.endText.text)) number = data.trueNumber;           
            else
            {
                if (data.input != null) number = data.input.text;
                else number = data.endText.text;
            }                       
            this.gameObject.SetActive(true);
            to.text = "";
            from.text = data.buttonText.text;                       
            dont = true;
            slider.gameObject.SetActive(true);                               
            slider.maxValue = measurements[selectedMeasurementFamily].Length - 1;
            slider.value = data.currentMeasurement;
            output.text = Round(decimal.Parse(number)).ToString();
            dont = false;
            if (selectedMeasurementFamily < 2) interFamilyButton.disabled = false;
            else interFamilyButton.disabled = true;
            Debug.Log("number: " + number);
            Debug.Log("Bruh? : " + Round(decimal.Parse(number)));
        }
    }
    public void ClosePage()
    {
        this.gameObject.SetActive(false);
    }
    public void SaveConversion()
    {
        data.currentMeasurement = (int)slider.value;
        data.measurementFamily = selectedMeasurementFamily;
        data.buttonText.text = measurements[selectedMeasurementFamily][(int)slider.value];
        if (data.power != 1 && selectedMeasurementFamily < 2) data.buttonText.text += "<sup>" + data.power + "</sup>";
        if (data.input != null) data.input.text = Round(decimal.Parse(number)).ToString();
        else if(data.measurementFamily == 3) data.endText.text = Round(decimal.Parse(number)).ToString();
        else data.endText.text = Round(decimal.Parse(number)).ToString();
        data.trueNumber = Convert(currentMeasurement, decimal.Parse(number)).ToString();
        data.roundTo = (int)roundSlider.value;
        dont2 = false;
    }
    public void ConvertInterFamily()
    {
        helpInt = (int)slider.value;
        dont = true;      
        if(selectedMeasurementFamily == 0)
        {
            number = ((Convert(currentMeasurement, decimal.Parse(number)) * Pow(conversions[3][helpInt], data.power))).ToString();
            if (slider.value > 0) slider.value -= 1;
            slider.maxValue -= 1;
            selectedMeasurementFamily = 1;
        }
        else
        {
            number = ((Convert(currentMeasurement, decimal.Parse(number)) / Pow(conversions[3][helpInt + 1], data.power))).ToString();
            slider.maxValue += 1;
            slider.value++;            
            selectedMeasurementFamily = 0;
        }
        currentMeasurement = (int)slider.value;        
        dont = false;
        if(data.input == null) number = calculator.Calculate(selectedMeasurementFamily, (int)slider.value, calculator.typeOfCalculation).ToString();
        output.text = Round(decimal.Parse(number)).ToString();
        to.text = measurements[selectedMeasurementFamily][currentMeasurement];
    }
    public decimal Convert(int i, decimal number)
    {
        int bruh = selectedMeasurementFamily;
        if (selectedMeasurementFamily == 3) bruh--;
        if (selectedMeasurementFamily == 2 && slider.value == 0 && i != slider.value) number -= 32;
        Debug.Log("KID NAMED FINGER: " + bruh + i);
        if (i == -1) i = 1;
        if (i == slider.value)
        {
            if (slider.value == 1 && selectedMeasurementFamily == 2 && currentMeasurement == 0) number += 32;
            return number;
        }
        else if ((selectedMeasurementFamily != 3 && i > slider.value) || (selectedMeasurementFamily == 3 && slider.value > i)) return Convert(i - 1, number * Pow(conversions[bruh][Math.Abs(i - 1)], data.power));
        else if ((selectedMeasurementFamily != 3 && i < slider.value) || (selectedMeasurementFamily == 3 && slider.value < i)) return Convert(i + 1, number / Pow(conversions[bruh][i], data.power));
        else return 69.420M;
    }
    public void SetCalculator(ThermalExpansion te)
    {
        calculator = te;
    }
    public void RoundSldierValueChanged()
    {
        if (!dont)
        {
            output.text = Round(decimal.Parse(number)).ToString();
        }
    }
    public decimal Round(decimal number)
    {        
        if (roundSlider.value != -1)
        {
            roundText.text = "Round to " + roundSlider.value + " digits";
            return Math.Round(Convert(currentMeasurement, number), (int)roundSlider.value, MidpointRounding.AwayFromZero);
        }
        else
        {
            roundText.text = "Do not round";
            return Convert(currentMeasurement, number);
        }
    }
    static decimal Pow(decimal x, decimal y)
    {
        if (y == 1) return x * 1;   
        else return x * Pow(x, y - 1);
    }
}
