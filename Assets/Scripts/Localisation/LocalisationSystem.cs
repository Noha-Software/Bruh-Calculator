﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class LocalisationSystem
{
	public static LocalisationSystem current = new LocalisationSystem();
	public event Action onLanguageChange;
	public void LanguageChanged() => onLanguageChange?.Invoke();

	public enum Language
	{
		English = 0,
		Hungarian = 1,
	}

	/// <summary>
	/// Currently selected language.
	/// </summary>
	public static Language CurrentLanguage
	{
		get { return language; }
		set { SetLanguage(value); }
	}
	static Language language;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void InitializeLanguage()
	{
		language = (Language)PlayerPrefs.GetInt("language");
	}

	private static Dictionary<string, string> localisedEN;
	private static Dictionary<string, string> localisedHU;

	public static bool isInit;
	public static LocalisationLoader locLoader;

	public static void Init()
	{
		language = (Language)PlayerPrefs.GetInt("language");

		locLoader = new LocalisationLoader();
		//locLoader.LoadCSV();
		UpdateDictionaries();
		isInit = true;
	}

	public static void UpdateDictionaries()
	{
		localisedEN = locLoader.GetDictionaryValues(Language.English);
		localisedHU = locLoader.GetDictionaryValues(Language.Hungarian);
	}

	/// <summary>
	/// Set the currently selected language
	/// </summary>
	public static void SetLanguage(Language newLanguage)
	{
		language = newLanguage;
		PlayerPrefs.SetInt("language", (int)newLanguage);
		PlayerPrefs.Save();
		current.LanguageChanged();
	}

	public static int GetNumberOfLanguages()
	{
		return Enum.GetNames(typeof(Language)).Length;
	}

	/// <summary>
	/// Get ID of language
	/// </summary>
	/// <returns>Returns the language's two-letter identifier code.</returns>
	public static string GetLanguageID(Language language)
	{
		switch (language)
		{
			case Language.English:
				return "en";
			case Language.Hungarian:
				return "hu";
		}
		return "en";
	}

	/// <summary>
	/// Returns local name of the language (i.e. Hungarian => Magyar)
	/// </summary>
	public static string GetLocalName(Language language)
	{
		string locname = null;
		switch (language)
		{
			case Language.English:
				locname = "English";
				break;
			case Language.Hungarian:
				locname = "Magyar";
				break;
			default:
				locname = "-";
				break;
		}
		return locname;
	}

	/// <summary>
	/// Get the localised entry database for editor use.
	/// </summary>
	/// <returns>Returns a dictionary with a key and value of strings.</returns>
	public static Dictionary<string, string> GetDictionaryForEditor()
	{
		Init();
		switch (language)
		{
			case Language.English:
				return localisedEN;
			case Language.Hungarian:
				return localisedHU;
		}
		return localisedEN;
	}
	public static Dictionary<string, string> GetDictionaryForEditor(Language language = Language.English)
	{
		Init();
		switch (language)
		{
			case Language.English:
				return localisedEN;
			case Language.Hungarian:
				return localisedHU;
		}
		return localisedEN;
	}

	/// <summary>
	/// Get the localised value of an entry in the currently selected language.
	/// </summary>
	/// <param name="key">Key of entry to localise.</param>
	public static string GetLocalisedValue(string key)
	{
		if(!isInit) { Init(); }

		string value;

		switch (language)
		{
			case Language.English:
				localisedEN.TryGetValue(key, out value);
				break;
			case Language.Hungarian:
				localisedHU.TryGetValue(key, out value);
				break;
			default:
				localisedEN.TryGetValue(key, out value);
				break;
		}
		return value;
	}
	/// <summary>
	/// Get the localised value of an entry in given language.
	/// </summary>
	/// <param name="key">Key of entry to localise.</param>
	/// <param name="language">Language to look for entry in.</param>
	public static string GetLocalisedValue(string key, Language language)
	{
		if (!isInit) { Init(); }
		if (locLoader == null) { locLoader = new LocalisationLoader(); }
		locLoader.GetDictionaryValues(language).TryGetValue(key, out string value);
		return value;
	}
	/// <summary>
	/// Get the localised value of an entry in given language.
	/// </summary>
	/// <param name="key">Key of entry to localise.</param>
	/// <param name="languageIndex">Index of language to look for entry in.</param>
	public static string GetLocalisedValue(string key, int languageIndex)
	{
		if (!isInit) { Init(); }
		if (locLoader == null) { locLoader = new LocalisationLoader(); }
		locLoader.GetDictionaryValues((Language)languageIndex).TryGetValue(key, out string value);
		return value;
	}

#if UNITY_EDITOR
	/// <summary>
	/// Edit an existent localisation entry or create a new one in the current version.
	/// </summary>
	/// <param name="key">Key of entry to edit.</param>
	/// <param name="value">Value of entry to edit.</param>
	public static void Edit(string key, string value)
	{
		if (value.Contains("\""))
		{
			value.Replace('"', '\"');
		}

		if (locLoader == null)
		{
			locLoader = new LocalisationLoader();
		}

		//locLoader.LoadCSV();
		locLoader.Edit(key, value, language);
		//locLoader.LoadCSV();

		UpdateDictionaries();
	}
	/// <summary>
	/// Edit an existent localisation entry or create a new one in a given language.
	/// </summary>
	/// <param name="key">Key of entry to edit.</param>
	/// <param name="value">Value of entry to edit.</param>
	/// <param name="language">Language to edit entry in.</param>
	public static void Edit(string key, string value, Language language)
	{
		if (value.Contains("\""))
		{
			value.Replace('"', '\"');
		}

		if (locLoader == null)
		{
			locLoader = new LocalisationLoader();
		}

		//locLoader.LoadCSV();
		locLoader.Edit(key, value, language);
		//locLoader.LoadCSV();

		UpdateDictionaries();
	}

	/// <summary>
	/// Removes entry from localisation database.
	/// </summary>
	/// <param name="key">Key of entry to remove.</param>
	public static void Remove(string key)
	{
		if (locLoader == null)
		{
			locLoader = new LocalisationLoader();
		}

		locLoader.Remove(key);
		//locLoader.LoadCSV();

		UpdateDictionaries();
	}
	/// <summary>
	/// Removes entry from localisation database in a language. Removes key entry if empty in all languages.
	/// </summary>
	/// <param name="key">Key of entry to remove.</param>
	/// <param name="language">Language to remove entry from.</param>
	[Obsolete]
	public static void Remove(string key, Language language)
	{
		bool[] isEmpty = new bool[GetNumberOfLanguages()];
		if (locLoader == null)
		{
			locLoader = new LocalisationLoader();
		}

		//locLoader.LoadCSV();

		for (int i = 0; i < GetNumberOfLanguages(); i++)
		{
			if ((Language)i != language)
			{
				if (GetLocalisedValue(key, (Language)i) == null || GetLocalisedValue(key, (Language)i) == string.Empty)
				{
					isEmpty[i] = true;
				}
				else
				{
					isEmpty[i] = false;
				}
			}
			else
			{
				isEmpty[i] = true;
			}
		}

		if (isEmpty.All(x => x))
		{
			locLoader.Remove(key);
		}
		else
		{
			locLoader.Edit(key, "", language);
		}
		//locLoader.LoadCSV();

		UpdateDictionaries();
	}
#endif
}