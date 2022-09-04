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

    bool endGameAlpha;

    int toFamily, toMeasurement;

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
    public void WriteOutput()
    {
        tabs.output.text = Calculate(toFamily, toMeasurement).ToString();
    }    
    //tabs.output.text = Convert.ToString((b - a) / (a * c * alphaMultiplier));    
    public decimal Calculate(int toFamily, int toMeasurement)
    {
        this.toFamily = toFamily;
        this.toMeasurement = toMeasurement;
        switch(typeOfCalculation)
        {
            case 0:
                return Convert(tabs.regInput1.measurementFamily, tabs.regInput1.currentMeasurement, toFamily, toMeasurement, decimal.Parse(tabs.regInput1.input.text)) * (1 + tabs.regInput1.power* Convert(tabs.regInput3.measurementFamily, tabs.regInput3.currentMeasurement, toFamily, toMeasurement, decimal.Parse(tabs.regInput3.input.text)) * Convert(tabs.regInput2.measurementFamily, tabs.regInput2.currentMeasurement, toFamily, toMeasurement, decimal.Parse(tabs.regInput2.input.text)));                
            case 1:
                return ((Convert(tabs.regInput2.measurementFamily, tabs.regInput2.currentMeasurement, toFamily, toMeasurement, decimal.Parse(tabs.regInput2.input.text)) - Convert(tabs.regInput1.measurementFamily, tabs.regInput1.currentMeasurement, toFamily, toMeasurement, decimal.Parse(tabs.regInput1.input.text))) / (Convert(tabs.regInput1.measurementFamily, tabs.regInput1.currentMeasurement, toFamily, toMeasurement, decimal.Parse(tabs.regInput1.input.text)) * Convert(tabs.regInput3.measurementFamily, tabs.regInput3.currentMeasurement, toFamily, toMeasurement, decimal.Parse(tabs.regInput3.input.text)))) / tabs.regInput1.power;
            case 2:
                return (Convert(tabs.regInput2.measurementFamily, tabs.regInput2.currentMeasurement, toFamily, toMeasurement, decimal.Parse(tabs.regInput2.input.text)) - Convert(tabs.regInput1.measurementFamily, tabs.regInput1.currentMeasurement, toFamily, toMeasurement, decimal.Parse(tabs.regInput1.input.text))) / (Convert(tabs.regInput1.measurementFamily, tabs.regInput1.currentMeasurement, toFamily, toMeasurement, decimal.Parse(tabs.regInput1.input.text)) * (Convert(tabs.regInput3.measurementFamily, tabs.regInput3.currentMeasurement, toFamily, toMeasurement, decimal.Parse(tabs.regInput3.input.text)) / tabs.regInput1.power));
            default:
                return 0;
        }
    }
    decimal Convert(int currentFamily, int currentMeasurement,int toFamily,int toMeasurement, decimal number)
    {
        Debug.Log("numbir: " + number);
        if (currentFamily < 2 && toFamily < 2 && currentFamily != toFamily)
        {                        
            if (currentFamily == 0)
            {
                number *= ExpansionConverter.conversions[3][currentMeasurement];
                if (currentMeasurement > 0) currentMeasurement -= 1;
                return Convert(1, currentMeasurement, toFamily, toMeasurement, number);
            }
            else
            {
                number /= ExpansionConverter.conversions[3][currentMeasurement + 1];
                return Convert(0, currentMeasurement + 1, toFamily, toMeasurement, number);
            }
        }
        else
        {
            int bruh = currentFamily;
            if (currentFamily == 3) bruh--;
            if(bruh == 2)
            {
                Debug.Log(toFamily);
                toMeasurement = toFamily;
                if (toMeasurement == 0 && currentMeasurement != toMeasurement) number -= 32;
            }            
            if (currentMeasurement == toMeasurement)
            {
                if (toMeasurement == 1 && currentFamily == 2 && currentMeasurement == 1) number += 32;
                Debug.Log("nombar: " + number);
                return number;
            }
            else if (currentMeasurement > toMeasurement) return Convert(currentFamily, currentMeasurement - 1, bruh, toMeasurement, number * ExpansionConverter.conversions[bruh][currentMeasurement - 1]);
            else return Convert(currentFamily, currentMeasurement + 1, bruh, toMeasurement, number / ExpansionConverter.conversions[bruh][currentMeasurement]);
        }                      
    }
}
