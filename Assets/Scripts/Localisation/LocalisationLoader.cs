using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class LocalisationLoader
{
	readonly char lineSeparator = '\n';
	readonly char fieldSeparator = ':';

	/// <summary>
	/// Get localisation file's path for language
	/// </summary>
	string GetFilePath(LocalisationSystem.Language language)
	{
		string languageId = LocalisationSystem.GetLanguageID(language);
		return Path.Combine(Application.streamingAssetsPath + "/Localisation/", languageId + ".lang");
	}

	// triple_why's code from Unity Forums
	// https://forum.unity.com/threads/cant-use-json-file-from-streamingassets-on-android-and-ios.472164/
	string[] GetLines(LocalisationSystem.Language language)
	{
		string filePath = GetFilePath(language);
		string text;
		if (Application.platform == RuntimePlatform.Android)
		{
			UnityWebRequest www = UnityWebRequest.Get(filePath);
			www.SendWebRequest();
			while (!www.isDone) ;
			text = www.downloadHandler.text;
		}
		else text = File.ReadAllText(filePath);
		return text.Split(lineSeparator);	// split into lines
	}

	/// <summary>
	/// Get localised entry values for given language
	/// </summary>
	public Dictionary<string, string> GetDictionaryValues(LocalisationSystem.Language language)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();

		#if UNITY_EDITOR
		if (!File.Exists(GetFilePath(language)))
		{
			Debug.Log("Language (" + language + ") does not have a localisation file - creating one...");
			File.Create(GetFilePath(language));
			return dictionary;
		}
		#endif

		//string[] lines = File.ReadAllLines(GetFilePath(language));
		string[] lines = GetLines(language);

		if (lines == null)
		{
			Debug.LogWarning("Language (" + language + ") does not have a localisation file");
			return null;
		}

		for (int i = 0; i < lines.Length; i++)
		{
			string line = lines[i];
			if (string.IsNullOrWhiteSpace(line))
			{
				continue;
			}
			int separator = line.IndexOf(fieldSeparator);
			string key = line.Substring(0, separator);
			string value = line.Substring(separator+1);

			key.Trim(' ', lineSeparator, fieldSeparator);
			value.Trim(' ', lineSeparator, fieldSeparator);

			if (string.IsNullOrWhiteSpace(key))
			{
				continue;
			}
			else
			{
				dictionary.Add(key, value);
			}
		}

		return dictionary;
	}

#if UNITY_EDITOR
	/// <summary>
	/// Edit/add localisation entry
	/// </summary>
	/// <param name="key">Entry's key</param>
	/// <param name="value">New value</param>
	public void Edit(string key, string value, LocalisationSystem.Language language = LocalisationSystem.Language.English)
	{
		foreach (LocalisationSystem.Language lang in Enum.GetValues(typeof(LocalisationSystem.Language)))
		{
			if (!File.Exists(GetFilePath(lang)))
			{
				Debug.LogWarning("Language (" + lang + ") does not have a localisation file");
				File.Create(GetFilePath(lang));
				return;
			}
			Dictionary<string, string> dictionary = LocalisationSystem.GetDictionaryForEditor(lang);
			if (dictionary.ContainsKey(key))
			{
				if (lang == language)
				{
					string[] lines = File.ReadAllLines(GetFilePath(lang));
					for (int i = 0; i < lines.Length; i++)
					{
						if (lines[i].Substring(0, lines[i].IndexOf(fieldSeparator)) == key)
						{
							lines[i] = key.ToLower() + fieldSeparator + value;
							File.WriteAllLines(GetFilePath(lang), lines);
							return;
						}
					}
				}
			}
			else
			{
				if (lang == language)
				{
					File.AppendAllText(GetFilePath(lang), key.ToLower() + fieldSeparator + value + lineSeparator);
				}
				else
				{
					File.AppendAllText(GetFilePath(lang), key.ToLower() + fieldSeparator + lineSeparator);
				}
			}
		}
	}

	/// <summary>
	/// Remove entry from all languages
	/// </summary>
	/// <param name="key">Entry's key</param>
	public void Remove(string key)
	{
		foreach (LocalisationSystem.Language lang in Enum.GetValues(typeof(LocalisationSystem.Language)))
		{
			string[] lines = GetLines(lang);
			string[] keys = new string[lines.Length];

			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];
				keys[i] = line.Substring(0, lines[i].IndexOf(fieldSeparator));
			}

			int index = -1;

			for (int i = 0; i < keys.Length; i++)
			{
				if (keys[i].Contains(key))
				{
					index = i;
					break;
				}
			}

			if (index > -1)
			{
				string[] newLines;
				newLines = lines.Where(w => w != lines[index]).ToArray();

				File.WriteAllLines(GetFilePath(lang), newLines);
			}
		}
	}
#endif
}