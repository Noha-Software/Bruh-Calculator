using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLocaliserUI : MonoBehaviour
{
    TextMeshProUGUI textField;

    [Tooltip("Key of field in localisation file")]
    public LocalisedString localisedString;

    void Start()
    {
        LocalisationSystem.current.onLanguageChange += OnLanguageChange;
        textField = GetComponent<TextMeshProUGUI>();
        textField.text = localisedString.value;
    }

    void OnLanguageChange()
	{
        textField.text = localisedString.value;
    }
}
