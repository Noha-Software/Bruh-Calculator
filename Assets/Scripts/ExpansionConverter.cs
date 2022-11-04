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
    ThermalExpansion calculator;
    public Slider slider;
    public Slider roundSlider;

    string number;

    bool dont;

    int helpInt;
    static public int pifcm = -1; //Previous Inter Family Conversion Family

    static public string[][] measurements = new string[4][];
    static public decimal[][] conversions = new decimal[5][];

    int selectedMeasurementFamily = 0;
    int currentMeasurement;
    public void GetCalculator(ThermalExpansion te)
    {
        calculator = te;
    }
    public static void Define()
    {
        measurements[0] = new string[] { "mm", "cm", "dm", "m", "km" };
        measurements[1] = new string[] { "inch", "feet", "yard", "mile"};
        measurements[2] = new string[] { "°C", "°F" };
        measurements[3] = new string[] { "1/°C", "°F<sup>-1</sup>" };
        conversions[1] = new decimal[] { 12, 3, 1760 };
        conversions[0] = new decimal[] { 10, 10, 10, 1000 };
        conversions[2] = new decimal[] { 1 , 1};
        conversions[3] = new decimal[] { (decimal)9 / 5, (decimal)9 / 5 };
        conversions[4] = new decimal[] { 0.039370078740192M, 0.39370078740192M, 0.328083989501M, 1.09361329834M, 0.62137119224M };
    }
    public void SliderValueChange()
    {        
        if (!dont)
        {
            to.text = measurements[selectedMeasurementFamily][(int)slider.value];
            if (data.power != 1 && selectedMeasurementFamily < 2) to.text += "<sup>" + data.power + "</sup>";
            output.text = Round(decimal.Parse(number)).ToString();
            RemoveUnnecessaryChars(output);
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
            Debug.Log(number);
            Debug.Log(Round(decimal.Parse(number)).ToString());
            output.text = Round(decimal.Parse(number)).ToString();
            if (selectedMeasurementFamily < 2) interFamilyButton.disabled = false;
            else interFamilyButton.disabled = true;
            dont = false;
        }
    }
    public void ClosePage()
    {
        SaveSettings();
        this.gameObject.SetActive(false);
    }
    public void SaveConversion()
    {
        SaveSettings();
        if (data.power != 1 && selectedMeasurementFamily < 2) data.buttonText.text += "<sup>" + data.power + "</sup>";
        if (data.input != null)
        {
            data.input.text = Round(decimal.Parse(number)).ToString();
            RemoveUnnecessaryChars(data.input);
        }
        else
        {
            data.endText.text = Round(decimal.Parse(number)).ToString();
            RemoveUnnecessaryChars(data.input);
        }
        data.trueNumber = Convert((int)slider.value, currentMeasurement, decimal.Parse(number)).ToString();
        data.roundTo = (int)roundSlider.value;
        this.gameObject.SetActive(false);
    }
    public void ConvertInterFamily()
    {
        helpInt = (int)slider.value;
        dont = true;
        if (pifcm == -1) pifcm = helpInt;
        Debug.Log("----------------------------------------------------------");
        Debug.Log(String.Format("pifcm at start of conversion: {0}, family: {1}, measurement: {2}", pifcm, selectedMeasurementFamily, currentMeasurement));
        if(selectedMeasurementFamily == 0)
        {
            number = ((Convert(pifcm, currentMeasurement, decimal.Parse(number)) * Pow(conversions[4][pifcm], data.power, selectedMeasurementFamily))).ToString();
            if (slider.value > 0) slider.value -= 1;
            slider.maxValue -= 1;
            selectedMeasurementFamily = 1;
            if(pifcm != 0)pifcm--;
        }
        else
        {
            number = ((Convert(pifcm, currentMeasurement, decimal.Parse(number)) / Pow(conversions[4][pifcm + 1], data.power, selectedMeasurementFamily))).ToString();
            slider.maxValue += 1;
            slider.value++;            
            selectedMeasurementFamily = 0;
            pifcm++;
        }
        Debug.Log(number);
        currentMeasurement = (int)slider.value;        
        dont = false;
        number = Convert(currentMeasurement, pifcm, decimal.Parse(number)).ToString();
        output.text = Round(decimal.Parse(number)).ToString();
        RemoveUnnecessaryChars(output);
        to.text = measurements[selectedMeasurementFamily][currentMeasurement];
        if (data.power > 1) to.text += "<sup>" + data.power + "</sup>";
        Debug.Log(String.Format("pifcm at end of conversion: {0}, family: {1}, measurement: {2}", pifcm, selectedMeasurementFamily, currentMeasurement));
    }
    decimal Convert(int i,int y, decimal number)
    {
        if (data.input != null)
        {
            if (y == i)
            {
                if (selectedMeasurementFamily == 2 && dont == false && y != currentMeasurement)
                {
                    if (y == 1)
                    {
                        number /= 5;
                        number *= 9;
                    }
                    else
                    {
                        number /= 9;
                        number *= 5;
                    }
                }
                return number;
            }
            else if (y > i) return Convert(i, y - 1, number * ExpansionConverter.Pow(conversions[selectedMeasurementFamily][y - 1], data.power, selectedMeasurementFamily));
            else return Convert(i, y + 1, number / ExpansionConverter.Pow(conversions[selectedMeasurementFamily][y], data.power, selectedMeasurementFamily));
        }
        else return calculator.Calculate(selectedMeasurementFamily, (int)slider.value);
    }
    void RoundSldierValueChanged()
    {
        {
            if (!dont)
            {
                output.text = Round(decimal.Parse(number)).ToString();
                RemoveUnnecessaryChars(output);
            }
        }
    }
    decimal Round(decimal number)
    {
        
        if (roundSlider.value != -1)
        {
            roundText.text = "Round to " + roundSlider.value + " digits";
            return decimal.Parse((Math.Round(Convert((int)slider.value, currentMeasurement, number), (int)roundSlider.value, MidpointRounding.AwayFromZero)).ToString());
        }
        else
        {
            roundText.text = "Do not round";
            return decimal.Parse(Convert((int)slider.value, currentMeasurement, number).ToString());
        }
    }
    static public decimal Pow(decimal x, decimal y, int z)
    {
        if (z > 1)
        {
            return x;
        }
        else
        {
            if (y == 1) return x * 1;
            else return x * Pow(x, y - 1, 0);
        }       
    }      
    static public void RemoveUnnecessaryChars(Component c)
    {
        if(c is TMP_InputField)
        {
            TMP_InputField ipf = (TMP_InputField)c;
            if (ipf.text.Contains(Calculator.decimalPoint.ToString())) ipf.text = ipf.text.TrimEnd('0');
            if (ipf.text.EndsWith(Calculator.decimalPoint.ToString())) ipf.text = ipf.text.TrimEnd(Calculator.decimalPoint);
        }
        else
        {
            TextMeshProUGUI et = (TextMeshProUGUI)c;
            if (et.text.Contains(Calculator.decimalPoint.ToString())) et.text = et.text.TrimEnd('0');
            if (et.text.EndsWith(Calculator.decimalPoint.ToString())) et.text = et.text.TrimEnd(Calculator.decimalPoint);
        }
    }
    void SaveSettings()
    {
        data.currentMeasurement = (int)slider.value;
        data.measurementFamily = selectedMeasurementFamily;
        data.buttonText.text = measurements[selectedMeasurementFamily][(int)slider.value];
    }
}
