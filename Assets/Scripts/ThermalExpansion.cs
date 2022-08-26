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
        tabs.output.text = Calculate(tabs.regInput3.measurementFamily, tabs.regInput3.currentMeasurement).ToString();
    }    
    //tabs.output.text = Convert.ToString((b - a) / (a * c * alphaMultiplier));    
    public decimal Calculate(int toFamily, int toMeasurement)
    {
        return Convert(tabs.regInput1.measurementFamily, tabs.regInput1.currentMeasurement,toFamily,toMeasurement, decimal.Parse(tabs.regInput1.input.text)) * (1 + decimal.Parse(tabs.alphaInput.text) * Convert(tabs.regInput2.measurementFamily, tabs.regInput2.currentMeasurement,toFamily,toMeasurement, decimal.Parse(tabs.regInput2.input.text)));             
    }
    decimal Convert(int currentFamily, int currentMeasurement,int toFamily,int toMeasurement, decimal number)
    {
        //Debug.Log(number + ": family " + currentFamily + ", measeurement " + currentMeasurement);
        if (!endGameAlpha)
        {
            if (currentFamily != 2)
            {
                if (currentFamily == toFamily)
                {
                    if (currentMeasurement == toMeasurement) return number * 1;
                    else if (currentMeasurement > toMeasurement) return Convert(currentFamily, currentMeasurement - 1,toFamily,toMeasurement, number * ExpansionConverter.conversions[toFamily][currentMeasurement - 1]);
                    else return Convert(currentFamily, currentMeasurement + 1,toFamily,toMeasurement, number / ExpansionConverter.conversions[toFamily][currentMeasurement]);
                }
                else
                {
                    if (currentFamily == 0)
                    {
                        number *= ExpansionConverter.conversions[2][currentMeasurement];
                        if (currentMeasurement > 0) currentMeasurement -= 1;
                        return Convert(1, currentMeasurement,toFamily,toMeasurement, number);
                    }
                    else
                    {
                        number /= ExpansionConverter.conversions[2][currentMeasurement + 1];
                        return Convert(0, currentMeasurement + 1,toFamily,toMeasurement, number);
                    }
                }
            }
            else
            {
                if (currentMeasurement != toFamily && currentMeasurement == 0)  
                {                   
                    Debug.Log("BRUH MOMENT!");
                    return number * 9 / 5 + 32;
                }
                else if (currentMeasurement != toFamily && currentMeasurement == 1) return (number - 32) * 5 / 9;
                else return number;
            }
        }
        else return 0;
    }
}
