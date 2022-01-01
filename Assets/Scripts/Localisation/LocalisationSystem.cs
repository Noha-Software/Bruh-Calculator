using System;
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
		Hungarian = 1
	}

	/// <summary>
	/// Currently selected language.
	/// </summary>
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

	/// <summary>
	/// Set the currently selected language
	/// </summary>
	public static void SetLanguage(Language newLanguage)
	{
		language = newLanguage;
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

	/// <summary>
	/// Get the localised entry database for editor use.
	/// </summary>
	/// <returns>Returns a dictionary with a key and value of strings.</returns>
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

	/// <summary>
	/// Get the localised value of an entry in the currently selected language.
	/// </summary>
	/// <param name="key">Key of entry to localise.</param>
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
	/// <summary>
	/// Get the localised value of an entry in given language.
	/// </summary>
	/// <param name="key">Key of entry to localise.</param>
	/// <param name="language">Language to look for entry in.</param>
	public static string GetLocalisedValue(string key, Language language)
	{
		if (!isInit) { Init(); }
		if (csvLoader == null) { csvLoader = new CSVLoader(); }
		return csvLoader.GetDictionaryValues(language)[key];
	}
	/// <summary>
	/// Get the localised value of an entry in given language.
	/// </summary>
	/// <param name="key">Key of entry to localise.</param>
	/// <param name="languageIndex">Index of language to look for entry in.</param>
	public static string GetLocalisedValue(string key, int languageIndex)
	{
		if (!isInit) { Init(); }
		if (csvLoader == null) { csvLoader = new CSVLoader(); }
		return csvLoader.GetDictionaryValues((Language)languageIndex)[key];
	}

	/// <summary>
	/// Edit an existent localisation entry or create a new one.
	/// </summary>
	/// <param name="key">Key of entry to edit.</param>
	/// <param name="value">Value of entry to edit.</param>
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

		if (csvLoader == null)
		{
			csvLoader = new CSVLoader();
		}

		csvLoader.LoadCSV();
		csvLoader.Edit(key, value, language);
		csvLoader.LoadCSV();

		UpdateDictionaries();
	}
	public static void EditKey(string oldKey, string newKey)
	{
		if (newKey.Contains("\""))
		{
			newKey.Replace('"', '\"');
		}

		if (csvLoader == null)
		{
			csvLoader = new CSVLoader();
		}

		List<string> values = new List<string>();

		csvLoader.LoadCSV();

		for (int i = 0; i < Enum.GetNames(typeof(Language)).Length; i++)
		{
			csvLoader.Edit(newKey, csvLoader.GetDictionaryValues((Language)i)[oldKey], (Language)i);
		}
		csvLoader.Remove(oldKey);

		csvLoader.LoadCSV();

		UpdateDictionaries();
	}

	/// <summary>
	/// Removes entry from localisation database.
	/// </summary>
	/// <param name="key">Key of entry to remove.</param>
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
	/// <summary>
	/// Removes entry from localisation database in a language. Removes key entry if empty in all languages.
	/// </summary>
	/// <param name="key">Key of entry to remove.</param>
	/// <param name="language">Language to remove entry from.</param>
	public static void Remove(string key, Language language)
	{
		bool[] isEmpty = new bool[GetNumberOfLanguages()];
		if (csvLoader == null)
		{
			csvLoader = new CSVLoader();
		}

		csvLoader.LoadCSV();

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
			csvLoader.Remove(key);
		}
		else
		{
			csvLoader.Edit(key, "", language);
		}
		csvLoader.LoadCSV();

		UpdateDictionaries();
	}
}