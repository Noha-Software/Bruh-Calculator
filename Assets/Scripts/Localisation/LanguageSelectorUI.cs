using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LanguageSelectorUI : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public int value = 0;
    List<TMP_Dropdown.OptionData> optionDataList = new List<TMP_Dropdown.OptionData>();
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();

		for (int i = 0; i < Enum.GetNames(typeof(LocalisationSystem.Language)).Length; i++)
		{
            optionDataList.Add(new TMP_Dropdown.OptionData(Enum.GetName(typeof(LocalisationSystem.Language), (LocalisationSystem.Language)i)));
		}

        dropdown.options = optionDataList;
    }

    public void OnValueChanged()
	{
        value = dropdown.value;
        LocalisationSystem.SetLanguage((LocalisationSystem.Language)value);
	}
}
