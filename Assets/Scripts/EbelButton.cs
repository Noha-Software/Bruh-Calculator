using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class EbelButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent onClick;

    public Color originalColor;
    public Color hoverColor;
    public bool disabled;

    public void OnPointerClick(PointerEventData data)
    {
        if (!disabled) onClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if(!disabled)this.gameObject.GetComponent<Image>().color = hoverColor;
    }

    public void OnPointerExit(PointerEventData data)
    {
        this.gameObject.GetComponent<Image>().color = originalColor;
    }
    public void ResetColor()
    {
        this.gameObject.GetComponent<Image>().color = originalColor;
    }
}
