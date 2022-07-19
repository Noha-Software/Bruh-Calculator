using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ThermalExpansion : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    decimal a;
    decimal b;
    decimal c;

    Color switchColor;

    public UnityEvent buttonColorChange;
    public UnityEvent onClick;

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
    public void Calculate(ThermalExpansionTabs tabs)
    {
        a = Decimal.Parse(tabs.row1Input.text);
        b = Decimal.Parse(tabs.row2Input.text);
        c = Decimal.Parse(tabs.row3Input.text);
        tabs.output.text = Convert.ToString(a + a * b * c);
    }
}
