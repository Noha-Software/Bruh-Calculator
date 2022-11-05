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
    public TextMeshProUGUI roundText;
    public ExpansionConversionData data;
    ThermalExpansion calculator;
    public Slider slider;
    public Slider roundSlider;

    string number;

    bool dont;

    int helpInt;

    static public string[][] measurements = new string[4][];
    static public decimal[][] conversions = new decimal[5][];

    int toFamily;
    int currentFamily;
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
        conversions[4] = new decimal[] { 0.039370078740192M, 0.39370078740192M, 0.328083989501M, 1.09361329834M, 0.621371192237M };
    }
    public void SliderValueChange()
    {        
        if (!dont)
        {
            to.text = measurements[toFamily][(int)slider.value];
            if (data.power != 1 && toFamily < 2) to.text += "<sup>" + data.power + "</sup>";
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
            
            toFamily = data.toFamily;
            currentFamily = data.currentFamily;
            currentMeasurement = data.currentMeasurement;
            slider.gameObject.SetActive(true);
            roundSlider.value = data.roundTo;
            slider.maxValue = measurements[toFamily].Length - 1;
            slider.value = data.toMeasurement;  
            if (data.input != null && data.trueNumber != "" && Round(decimal.Parse(data.trueNumber)) == decimal.Parse(data.input.text)) number = data.trueNumber;            
            else if (data.endText != null && data.trueNumber != "" && Round(decimal.Parse(data.trueNumber)) == decimal.Parse(data.endText.text)) number = data.trueNumber;           
            else
            {
                if (data.input != null) number = data.input.text;
                else number = data.endText.text;
                if (data.input != null) data.trueNumber = data.input.text;
                else data.trueNumber = data.endText.text;
                data.toFamily = toFamily;
                data.toMeasurement = (int)slider.value;
                currentFamily = data.toFamily;
                currentMeasurement = data.toMeasurement;
                data.currentFamily = currentFamily;
                data.currentMeasurement = currentMeasurement;
            }
            
            this.gameObject.SetActive(true);
            to.text = "";
            from.text = data.buttonText.text;
            output.text = Round(decimal.Parse(number)).ToString();
            if (toFamily < 2) interFamilyButton.disabled = false;
            else interFamilyButton.disabled = true;
            RemoveUnnecessaryChars(output);
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
        if (data.power != 1 && toFamily < 2) data.buttonText.text += "<sup>" + data.power + "</sup>";
        if (data.input != null)
        {
            data.input.text = output.text;
            RemoveUnnecessaryChars(data.input);
        }
        else
        {
            data.endText.text = output.text;
            RemoveUnnecessaryChars(data.endText);
        }
        data.roundTo = (int)roundSlider.value;
        this.gameObject.SetActive(false);
    }
    public void ConvertInterFamily()
    {
        helpInt = (int)slider.value;
        dont = true;        
        if(toFamily == 0)
        {           
            if (slider.value > 0) slider.value -= 1;
            slider.maxValue -= 1;
            toFamily = 1;
            
        }
        else
        {           
            slider.maxValue += 1;
            slider.value++;            
            toFamily = 0;
            
        }              
        dont = false;
        output.text = Round(decimal.Parse(number)).ToString();
        RemoveUnnecessaryChars(output);
        to.text = measurements[toFamily][(int)slider.value];
        if (data.power > 1) to.text += "<sup>" + data.power + "</sup>";
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
            roundText.text = LocalisationSystem.GetLocalisedValue("roundto") + ' ' + roundSlider.value + ' ' + LocalisationSystem.GetLocalisedValue("digits");
            return decimal.Parse(Math.Round(ThermalExpansion.Convert(currentFamily, currentMeasurement, toFamily, (int)slider.value, number, data.power), (int)roundSlider.value).ToString());
        }
        else
        {
            roundText.text = LocalisationSystem.GetLocalisedValue("noround");
            return ThermalExpansion.Convert(currentFamily, currentMeasurement, toFamily, (int)slider.value, number, data.power);
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
        data.toMeasurement = (int)slider.value;
        data.toFamily = toFamily;
        data.buttonText.text = measurements[toFamily][(int)slider.value];
    }
}
