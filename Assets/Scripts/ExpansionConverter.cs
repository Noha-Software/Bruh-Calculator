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
    static public decimal[][] conversions = new decimal[5][];

    int selectedMeasurementFamily = 0;
    int currentMeasurement;

    public static void Define()
    {
        measurements[0] = new string[] { "mm", "cm", "dm", "m", "km" };
        measurements[1] = new string[] { "inch", "feet", "yard", "mile"};
        measurements[2] = new string[] { "°C", "°F" };
        measurements[3] = new string[] { "1/°C", "°F<sup>-1</sup>" };
        conversions[1] = new decimal[] { 12, 3, 1760 };
        conversions[0] = new decimal[] { 10, 10, 10, 1000 };
        conversions[2] = new decimal[] { (decimal)5/9 , (decimal)5/9};
        conversions[3] = new decimal[] { (decimal)9 / 5, (decimal)9 / 5 };
        conversions[4] = new decimal[] { 0.0393700787M, 0.393700787M, 0.32808399M, 1.0936133M, 0.621371192M };
    }
    public void SliderValueChange()
    {        
        if (!dont)
        {
            to.text = measurements[selectedMeasurementFamily][(int)slider.value];
            if (data.power != 1 && selectedMeasurementFamily < 2) to.text += "<sup>" + data.power + "</sup>";
            output.text = Round(decimal.Parse(number)).ToString();
            if (output.text.Contains(",")) output.text = output.text.TrimEnd('0');
        }
    }
    public void PageOpened(ExpansionConversionData ecd)
    {
        if (ecd != null && ((ecd.input != null && ecd.input.text != "") || (ecd.endText != null && ecd.endText.text != "")))
        {
            data = ecd;
            dont = true;
            selectedMeasurementFamily = data.measurementFamily;
            currentMeasurement = data.currentMeasurement;
            slider.gameObject.SetActive(true);
            roundSlider.value = data.roundTo;
            slider.maxValue = measurements[selectedMeasurementFamily].Length - 1;
            slider.value = data.currentMeasurement;
            dont = false;
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
            output.text = Round(decimal.Parse(number)).ToString();
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
            number = ((Convert(currentMeasurement, decimal.Parse(number)) * Pow(conversions[4][helpInt], data.power, selectedMeasurementFamily))).ToString();
            if (slider.value > 0) slider.value -= 1;
            slider.maxValue -= 1;
            selectedMeasurementFamily = 1;
        }
        else
        {
            number = ((Convert(currentMeasurement, decimal.Parse(number)) / Pow(conversions[4][helpInt + 1], data.power, selectedMeasurementFamily))).ToString();
            slider.maxValue += 1;
            slider.value++;            
            selectedMeasurementFamily = 0;
        }
        currentMeasurement = (int)slider.value;        
        dont = false;
        if(data.input == null) number = calculator.Calculate(selectedMeasurementFamily, (int)slider.value, calculator.typeOfCalculation).ToString();
        output.text = Round(decimal.Parse(number)).ToString();
        to.text = measurements[selectedMeasurementFamily][currentMeasurement];
        if (data.power > 1) to.text += "<sup>" + data.power + "</sup>";
    }
    public decimal Convert(int i, decimal number)
    {
        Debug.Log("KID NAMED FINGER: " + selectedMeasurementFamily + i + slider.value);
        if (i == slider.value)
        {
            return number;
        }
        else if (i > slider.value) return Convert(i - 1, number * ExpansionConverter.Pow(conversions[selectedMeasurementFamily][i - 1], data.power, selectedMeasurementFamily));
        else return Convert(i + 1, number / ExpansionConverter.Pow(conversions[selectedMeasurementFamily][i], data.power, selectedMeasurementFamily));
    }
    public void SetCalculator(ThermalExpansion te)
    {
        calculator = te;
    }
    public void RoundSldierValueChanged()
    {
        {
            if (!dont)
            output.text = Round(decimal.Parse(number)).ToString();
        }
    }
    public decimal Round(decimal number)
    {        
        if (roundSlider.value != -1)
        {
            roundText.text = "Round to " + roundSlider.value + " digits";
            return decimal.Parse((Math.Round(Convert(currentMeasurement, number), (int)roundSlider.value, MidpointRounding.AwayFromZero)).ToString());
        }
        else
        {
            roundText.text = "Do not round";
            return decimal.Parse(Convert(currentMeasurement, number).ToString());
        }
    }
    static public decimal Pow(decimal x, decimal y, int z)
    {
        if (z > 1)
        {
            Debug.Log("PUSSY");
            return x;
        }
        else
        {
            if (y == 1) return x * 1;
            else return x * Pow(x, y - 1, 0);
        }       
    }
}
