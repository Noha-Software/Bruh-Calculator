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
                return Convert(tabs.regInput1.measurementFamily, tabs.regInput1.currentMeasurement, data.measurementFamily, data.currentMeasurement, decimal.Parse(tabs.regInput1.input.text)) * (1 + tabs.regInput1.power* Convert(tabs.regInput3.measurementFamily, tabs.regInput3.currentMeasurement, data.measurementFamily, data.currentMeasurement, decimal.Parse(tabs.regInput3.input.text)) * Convert(tabs.regInput2.measurementFamily, tabs.regInput2.currentMeasurement, data.measurementFamily, data.currentMeasurement, decimal.Parse(tabs.regInput2.input.text)));                
            case 1:
                return ((Convert(tabs.regInput2.measurementFamily, tabs.regInput2.currentMeasurement, data.measurementFamily, data.currentMeasurement, decimal.Parse(tabs.regInput2.input.text)) - Convert(tabs.regInput1.measurementFamily, tabs.regInput1.currentMeasurement, data.measurementFamily, data.currentMeasurement, decimal.Parse(tabs.regInput1.input.text))) / (Convert(tabs.regInput1.measurementFamily, tabs.regInput1.currentMeasurement, data.measurementFamily, data.currentMeasurement, decimal.Parse(tabs.regInput1.input.text)) * Convert(tabs.regInput3.measurementFamily, tabs.regInput3.currentMeasurement, data.measurementFamily, data.currentMeasurement, decimal.Parse(tabs.regInput3.input.text)))) / tabs.regInput1.power;
            case 2:
                return (Convert(tabs.regInput2.measurementFamily, tabs.regInput2.currentMeasurement, data.measurementFamily, data.currentMeasurement, decimal.Parse(tabs.regInput2.input.text)) - Convert(tabs.regInput1.measurementFamily, tabs.regInput1.currentMeasurement, data.measurementFamily, data.currentMeasurement, decimal.Parse(tabs.regInput1.input.text))) / (Convert(tabs.regInput1.measurementFamily, tabs.regInput1.currentMeasurement, data.measurementFamily, data.currentMeasurement, decimal.Parse(tabs.regInput1.input.text)) * (Convert(tabs.regInput3.measurementFamily, tabs.regInput3.currentMeasurement, data.measurementFamily, data.currentMeasurement, decimal.Parse(tabs.regInput3.input.text)) / tabs.regInput1.power));
            default:
                return 0;
        }
    }
    public decimal Calculate(int family, int measurement)
    {
        switch (typeOfCalculation)
        {
            case 0:
                return Convert(tabs.regInput1.measurementFamily, tabs.regInput1.currentMeasurement, family, measurement, decimal.Parse(tabs.regInput1.input.text)) * (1 + tabs.regInput1.power * Convert(tabs.regInput3.measurementFamily, tabs.regInput3.currentMeasurement, family, measurement, decimal.Parse(tabs.regInput3.input.text)) * Convert(tabs.regInput2.measurementFamily, tabs.regInput2.currentMeasurement, family,measurement , decimal.Parse(tabs.regInput2.input.text)));
            case 1:
                return (Convert(tabs.regInput2.measurementFamily, tabs.regInput2.currentMeasurement, family, measurement, decimal.Parse(tabs.regInput2.input.text)) - Convert(tabs.regInput1.measurementFamily, tabs.regInput1.currentMeasurement, family, measurement, decimal.Parse(tabs.regInput1.input.text))) / (Convert(tabs.regInput1.measurementFamily, tabs.regInput1.currentMeasurement, family, measurement, decimal.Parse(tabs.regInput1.input.text)) * Convert(tabs.regInput3.measurementFamily, tabs.regInput3.currentMeasurement, family, measurement, decimal.Parse(tabs.regInput3.input.text))) / tabs.regInput1.power;
            case 2:
                return (Convert(tabs.regInput2.measurementFamily, tabs.regInput2.currentMeasurement, family, measurement, decimal.Parse(tabs.regInput2.input.text)) - Convert(tabs.regInput1.measurementFamily, tabs.regInput1.currentMeasurement, family, measurement, decimal.Parse(tabs.regInput1.input.text))) / (Convert(tabs.regInput1.measurementFamily, tabs.regInput1.currentMeasurement, family, measurement, decimal.Parse(tabs.regInput1.input.text)) * (Convert(tabs.regInput3.measurementFamily, tabs.regInput3.currentMeasurement, family, measurement, decimal.Parse(tabs.regInput3.input.text)) / tabs.regInput1.power));
            default:
                return 0;
        }
    }
    decimal Convert(int currentFamily, int currentMeasurement,int toFamily,int toMeasurement, decimal number)
    {
        if (!realFamily)
        {
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
        }
        if (currentFamily < 2 && toFamily < 2 && currentFamily != toFamily)
        {                        
            if (currentFamily == 0)
            {
                number *= ExpansionConverter.Pow(ExpansionConverter.conversions[4][currentMeasurement], alphaMultiplier, currentFamily);
                if (currentMeasurement > 0 && !fuckHowINameDis) currentMeasurement -= 1;
                fuckHowINameDis = false;
                return Convert(1, currentMeasurement, toFamily, toMeasurement, number);
            }
            else
            {
                number /= ExpansionConverter.Pow(ExpansionConverter.conversions[4][currentMeasurement + 1], alphaMultiplier, currentFamily);
                if (!fuckHowINameDis) currentMeasurement += 1;
                fuckHowINameDis = false;
                return Convert(0, currentMeasurement, toFamily, toMeasurement, number);
            }
        }
        else
        {
            if (currentMeasurement == toMeasurement) return number;
            else if (currentMeasurement > toMeasurement) return Convert(currentFamily, currentMeasurement - 1, toFamily, toMeasurement, number * ExpansionConverter.Pow(ExpansionConverter.conversions[currentFamily][currentMeasurement - 1], alphaMultiplier, currentFamily));
            else return Convert(currentFamily, currentMeasurement + 1, toFamily, toMeasurement, number / ExpansionConverter.Pow(ExpansionConverter.conversions[currentFamily][currentMeasurement], alphaMultiplier, currentFamily));
        }
    }
}
