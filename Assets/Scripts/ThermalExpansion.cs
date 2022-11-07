using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class ThermalExpansion : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] int alphaMultiplier;
    [SerializeField] string equationText;
    public int typeOfCalculation; 

    public ThermalExpansionTabs tabs;

    bool realFamily;
    bool fuckHowINameDis;
    static bool doo = false;

    decimal result;

    Color switchColor;

    public UnityEvent buttonColorChange;
    public UnityEvent onClick;

    public void ChangeEquationText(TMP_Text tmp)
    {
        tmp.text = equationText;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        switchColor = new Color(1.0F, 0.73F, 0.0F, 1.0F);
        buttonColorChange.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        switchColor = Color.white;
        buttonColorChange.Invoke();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        onClick.Invoke();
    }
    public void ButtonColorChange(Image image)
    {
        image.color = switchColor;           
    }    
    public void WriteOutput(ExpansionConversionData data)
    {
        if (data.roundTo != -1) data.endText.text = Math.Round(Calculate(data), data.roundTo, MidpointRounding.AwayFromZero).ToString();
        else data.endText.text = Calculate(data).ToString();
        ExpansionConverter.RemoveUnnecessaryChars(data.endText);
    }    
    public decimal Calculate(ExpansionConversionData data)
    {
        switch(typeOfCalculation)
        {
            case 0:
                return Convert(tabs.regInput1.toFamily, tabs.regInput1.toMeasurement, data.toFamily, data.toMeasurement, decimal.Parse(tabs.regInput1.input.text), data.power) * (1 + tabs.regInput1.power* Convert(tabs.regInput3.toFamily, tabs.regInput3.toMeasurement, data.toFamily, data.toMeasurement, decimal.Parse(tabs.regInput3.input.text), data.power) * Convert(tabs.regInput2.toFamily, tabs.regInput2.toMeasurement, data.toFamily, data.toMeasurement, decimal.Parse(tabs.regInput2.input.text), data.power));                
            case 1:
                return ((Convert(tabs.regInput2.toFamily, tabs.regInput2.toMeasurement, data.toFamily, data.toMeasurement, decimal.Parse(tabs.regInput2.input.text), data.power) - Convert(tabs.regInput1.toFamily, tabs.regInput1.toMeasurement, data.toFamily, data.toMeasurement, decimal.Parse(tabs.regInput1.input.text), data.power)) / (Convert(tabs.regInput1.toFamily, tabs.regInput1.toMeasurement, data.toFamily, data.toMeasurement, decimal.Parse(tabs.regInput1.input.text), data.power) * Convert(tabs.regInput3.toFamily, tabs.regInput3.toMeasurement, data.toFamily, data.toMeasurement, decimal.Parse(tabs.regInput3.input.text), data.power))) / tabs.regInput1.power;
            case 2:
                return (Convert(tabs.regInput2.toFamily, tabs.regInput2.toMeasurement, data.toFamily, data.toMeasurement, decimal.Parse(tabs.regInput2.input.text), data.power) - Convert(tabs.regInput1.toFamily, tabs.regInput1.toMeasurement, data.toFamily, data.toMeasurement, decimal.Parse(tabs.regInput1.input.text), data.power)) / (Convert(tabs.regInput1.toFamily, tabs.regInput1.toMeasurement, data.toFamily, data.toMeasurement, decimal.Parse(tabs.regInput1.input.text), data.power) * (Convert(tabs.regInput3.toFamily, tabs.regInput3.toMeasurement, data.toFamily, data.toMeasurement, decimal.Parse(tabs.regInput3.input.text), data.power) / tabs.regInput1.power));
            default:
                return 0;
        }
    }
    public decimal Calculate(int family, int measurement, int power)
    {
        switch (typeOfCalculation)
        {
            case 0:
                return Convert(tabs.regInput1.toFamily, tabs.regInput1.toMeasurement, family, measurement, decimal.Parse(tabs.regInput1.input.text), power) * (1 + tabs.regInput1.power * Convert(tabs.regInput3.toFamily, tabs.regInput3.toMeasurement, family, measurement, decimal.Parse(tabs.regInput3.input.text), power) * Convert(tabs.regInput2.toFamily, tabs.regInput2.toMeasurement, family,measurement , decimal.Parse(tabs.regInput2.input.text), power));
            case 1:
                return (Convert(tabs.regInput2.toFamily, tabs.regInput2.toMeasurement, family, measurement, decimal.Parse(tabs.regInput2.input.text), power) - Convert(tabs.regInput1.toFamily, tabs.regInput1.toMeasurement, family, measurement, decimal.Parse(tabs.regInput1.input.text), power)) / (Convert(tabs.regInput1.toFamily, tabs.regInput1.toMeasurement, family, measurement, decimal.Parse(tabs.regInput1.input.text), power) * Convert(tabs.regInput3.toFamily, tabs.regInput3.toMeasurement, family, measurement, decimal.Parse(tabs.regInput3.input.text), power)) / tabs.regInput1.power;
            case 2:
                return (Convert(tabs.regInput2.toFamily, tabs.regInput2.toMeasurement, family, measurement, decimal.Parse(tabs.regInput2.input.text), power) - Convert(tabs.regInput1.toFamily, tabs.regInput1.toMeasurement, family, measurement, decimal.Parse(tabs.regInput1.input.text), power)) / (Convert(tabs.regInput1.toFamily, tabs.regInput1.toMeasurement, family, measurement, decimal.Parse(tabs.regInput1.input.text), power) * (Convert(tabs.regInput3.toFamily, tabs.regInput3.toMeasurement, family, measurement, decimal.Parse(tabs.regInput3.input.text), power) / tabs.regInput1.power));
            default:
                return 0;
        }
    }
    static public decimal Convert(int currentFamily, int currentMeasurement,int toFamily,int toMeasurement, decimal number, int power)
    {
        //Debug.Log("-------------------------------------------");
        //Debug.Log(number);        
        //Debug.Log(String.Format("Current Family/Measurement: {0}, {1}; To Family, Measurement {2}, {3}", currentFamily, currentMeasurement, toFamily, toMeasurement));
        if(currentFamily < 2 && toFamily > 1)
        {               
            toFamily = toMeasurement;
            if (toFamily == 0) toMeasurement = 3;
            else toMeasurement = 2;
        }
        else if(currentFamily > 1 && toFamily < 2)
        {                               
            toMeasurement = toFamily;
            toFamily = currentFamily;
        }
       // Debug.Log(String.Format("Current Family/Measurement: {0}, {1}; To Family, Measurement {2}, {3}", currentFamily, currentMeasurement, toFamily, toMeasurement));
        if (currentFamily < 2 && toFamily < 2 && currentFamily != toFamily)
        {                        
            if (currentFamily == 0)
            {
                number *= ExpansionConverter.Pow(ExpansionConverter.conversions[4][currentMeasurement], power, currentFamily);
                if (currentMeasurement > 0) currentMeasurement -= 1;
                return Convert(1, currentMeasurement, toFamily, toMeasurement, number, power);
            }
            else
            {
                number /= ExpansionConverter.Pow(ExpansionConverter.conversions[4][currentMeasurement + 1], power, currentFamily);
                currentMeasurement += 1;
                return Convert(0, currentMeasurement, toFamily, toMeasurement, number, power);
            }
        }
        else
        {
            if (currentMeasurement == toMeasurement)
            {
                if (currentFamily == 2 && doo == true)
                {

                    if (currentMeasurement == 1)
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
                doo = false;
                return number;
            }
            else
            {
                doo = true;
                if (currentMeasurement > toMeasurement) return Convert(currentFamily, currentMeasurement - 1, toFamily, toMeasurement, number * ExpansionConverter.Pow(ExpansionConverter.conversions[currentFamily][currentMeasurement - 1], power, currentFamily), power);
                else return Convert(currentFamily, currentMeasurement + 1, toFamily, toMeasurement, number / ExpansionConverter.Pow(ExpansionConverter.conversions[currentFamily][currentMeasurement], power, currentFamily), power);
            }           
        }
    }
}
