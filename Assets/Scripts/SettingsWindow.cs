using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SettingsWindow : MonoBehaviour
{
	[Header("Language Selector")]
	public TMP_Dropdown languageDropdown;
	public LocalisationSystem.Language language;

	private void Awake()
	{
		int languageCache = PlayerPrefs.GetInt("language");
		Debug.Log("PlayerPref: " + PlayerPrefs.GetInt("language"));
		Debug.Log("CurrentLanguage: " + LocalisationSystem.CurrentLanguage.ToString());

		List<TMP_Dropdown.OptionData> optionDataList = new List<TMP_Dropdown.OptionData>();
		languageDropdown.ClearOptions();

		for (int i = 0; i < Enum.GetNames(typeof(LocalisationSystem.Language)).Length; i++)
		{
			optionDataList.Add(new TMP_Dropdown.OptionData(Enum.GetName(typeof(LocalisationSystem.Language), (LocalisationSystem.Language)i)));
		}

		languageDropdown.options = optionDataList;

		languageDropdown.value = languageCache;
		PlayerPrefs.SetInt("language", languageCache);
	}

	public void OnLanguageValueChanged()
	{
		language = (LocalisationSystem.Language)languageDropdown.value;
		LocalisationSystem.SetLanguage(language);
	}
}
