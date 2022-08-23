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

    public ExpansionConversionData a;
    public ExpansionConversionData b;
    public ExpansionConversionData c;
    public TMP_InputField alpha;

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
    public void CalculateLength(ThermalExpansionTabs tabs)
    {
        c = tabs.regInput3;
        tabs.output.text = (Convert(tabs.regInput1.measurementFamily,tabs.regInput1.currentMeasurement,decimal.Parse(tabs.regInput1.input.text)) * (1 + decimal.Parse(tabs.alphaInput.text) * Convert(tabs.regInput2.measurementFamily,tabs.regInput2.currentMeasurement,decimal.Parse(tabs.regInput2.input.text)))).ToString();
    }
    public void CalculateDeltaT(ThermalExpansionTabs tabs)
    {
        //tabs.output.text = Convert.ToString((b - a) / (a * c * alphaMultiplier));
    }
    public void CaluclateAlpha(ThermalExpansionTabs tabs)
    {

    }
    decimal Convert(int currentFamily, int currentMeasurement, decimal number)
    {
        Debug.Log(number + ": family " + currentFamily + ", measeurement " + currentMeasurement);
        if (!endGameAlpha)
        {
            if (currentFamily != 2)
            {
                if (currentFamily == c.measurementFamily)
                {
                    if (currentMeasurement == c.currentMeasurement) return number * 1;
                    else if (currentMeasurement > c.currentMeasurement) return Convert(currentFamily, currentMeasurement - 1, number * ExpansionConverter.conversions[c.measurementFamily][currentMeasurement - 1]);
                    else return Convert(currentFamily, currentMeasurement + 1, number / ExpansionConverter.conversions[c.measurementFamily][currentMeasurement]);
                }
                else
                {
                    if (currentFamily == 0)
                    {
                        number *= ExpansionConverter.conversions[2][currentMeasurement];
                        if (currentMeasurement > 0) currentMeasurement -= 1;
                        return Convert(1, currentMeasurement, number);
                    }
                    else
                    {
                        number /= ExpansionConverter.conversions[2][currentMeasurement + 1];
                        return Convert(0, currentMeasurement + 1, number);
                    }
                }
            }
            else
            {
                if (currentMeasurement != c.measurementFamily && currentMeasurement == 0) return number * 9 / 5 + 32;
                else if (currentMeasurement != c.measurementFamily && currentMeasurement == 1) return (number - 32) * 5 / 9;
                else return number;
            }
        }
        else return 0;
    }
}
