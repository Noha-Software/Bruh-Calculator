using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LocalisationSystem : MonoBehaviour
{
    public enum Language
	{
		English = 0,
		Hungarian = 1
	}

	public static Language CurrentLanguage
	{
		get { return language; }
		set { language = value; }
	}
	static Language language = Language.English;

	private static Dictionary<string, string> localisedEN;
	private static Dictionary<string, string> localisedHU;

	public static bool isInit;

	public static CSVLoader csvLoader;

	public static void Init()
	{
		csvLoader = new CSVLoader();
		csvLoader.LoadCSV();

		UpdateDictionaries();

		isInit = true;
	}

	public static void UpdateDictionaries()
	{
		localisedEN = csvLoader.GetDictionaryValues(Language.English);
		localisedHU = csvLoader.GetDictionaryValues(Language.Hungarian);
	}

	public static void SetLanguage(Language newLanguage)
	{
		language = newLanguage;
	}

	public static string GetLanguageID(Language language)
	{
		string id;
		switch (language)
		{
			case Language.English:
				id = "en";
				break;
			case Language.Hungarian:
				id = "hu";
				break;
			default:
				id = "en";
				break;
		}
		return id;
	}

	public static Dictionary<string, string> GetDictionaryForEditor()
	{
		if(!isInit) { Init(); }
		switch (language)
		{
			default:
				return localisedEN;
			case Language.English:
				return localisedEN;
			case Language.Hungarian:
				return localisedHU;
		}
	}

	public static string GetLocalisedValue(string key)
	{
		if(!isInit) { Init(); }

		string value = key;

		switch (language)
		{
			default:
				localisedEN.TryGetValue(key, out value);
				break;
			case Language.English:
				localisedEN.TryGetValue(key, out value);
				break;
			case Language.Hungarian:
				localisedHU.TryGetValue(key, out value);
				break;
		}
		return value;
	}
	public static string GetLocalisedValue(string key, Language language)
	{
		if (!isInit) { Init(); }
		if (csvLoader == null) { csvLoader = new CSVLoader(); }

		return csvLoader.GetDictionaryValues(language)[key];
	}

	public static string GetLocalisedValue(string key, int languageIndex)
	{
		if (!isInit) { Init(); }
		if (csvLoader == null) { csvLoader = new CSVLoader(); }

		return csvLoader.GetDictionaryValues((Language)languageIndex)[key];
	}

	public static void Edit(string key, string value)
	{
		if (value.Contains("\""))
		{
			value.Replace('"', '\"');
		}

		if (csvLoader == null)
		{
			csvLoader = new CSVLoader();
		}

		csvLoader.LoadCSV();
		csvLoader.Edit(key, value, language);
		csvLoader.LoadCSV();

		UpdateDictionaries();
	}

	public static void Remove(string key)
	{
		if (csvLoader == null)
		{
			csvLoader = new CSVLoader();
		}

		csvLoader.Remove(key);
		csvLoader.LoadCSV();

		UpdateDictionaries();
	}
	public static void Remove(string key, Language language)
	{
		if (csvLoader == null)
		{
			csvLoader = new CSVLoader();
		}

		csvLoader.LoadCSV();
		csvLoader.Edit(key, "", language);
		csvLoader.LoadCSV();

		UpdateDictionaries();
	}
}
