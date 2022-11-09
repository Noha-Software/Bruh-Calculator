using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// TODO: Replace old CSV system
public class LocalisationLoader
{
	readonly char lineSeparator = '\n';
	readonly char fieldSeparator = ':';

	string GetFilePath(LocalisationSystem.Language language)
	{
		string languageId = LocalisationSystem.GetLanguageID(language);
		return "Assets/Localisation/" + languageId + ".lang";
	}

	public Dictionary<string, string> GetDictionaryValues(LocalisationSystem.Language language)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();

		if (!File.Exists(GetFilePath(language)))
		{
			Debug.LogWarning("Language (" + language + ") does not have a localisation file");
			File.Create(GetFilePath(language));
			return dictionary;
		}

		string[] lines = File.ReadAllLines(GetFilePath(language));

		if (lines == null)
		{
			Debug.LogWarning("Language (" + language + ") does not have a localisation file");
			return null;
		}

		for (int i = 0; i < lines.Length; i++)
		{
			string line = lines[i];
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
	public void Edit(string key, string value, LocalisationSystem.Language language = LocalisationSystem.Language.English)
	{
		Debug.Log("Edit");
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
							lines[i] = key + fieldSeparator + value;
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
					File.AppendAllText(GetFilePath(lang), key + fieldSeparator + value + lineSeparator);
				}
				else
				{
					File.AppendAllText(GetFilePath(lang), key + fieldSeparator + lineSeparator);
				}
			}
		}
	}

	public void Remove(string key)
	{
		foreach (LocalisationSystem.Language lang in Enum.GetValues(typeof(LocalisationSystem.Language)))
		{
			string[] lines = File.ReadAllLines(GetFilePath(lang));
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