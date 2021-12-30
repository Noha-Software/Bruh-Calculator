using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.IO;
using UnityEditor;
using System.Linq;

// (C) Game Dev Guide, https://youtu.be/c-dzg4M20wY
// TODO: Implement multi-file localisation
public class CSVLoader
{
	TextAsset csvFile;
    readonly char lineSeparator = '\n';
    readonly char surround = '"';
    readonly string[] fieldSeparator = { "\",\"" };

    public void LoadCSV()
	{
		csvFile = Resources.Load<TextAsset>("localisation");
	}

	public int GetLanguageIndex(LocalisationSystem.Language language)
	{
		string languageId = LocalisationSystem.GetLanguageID(language);

		string[] lines = csvFile.text.Split(lineSeparator);

		string[] headers = lines[0].Split(fieldSeparator, StringSplitOptions.None);

		for (int i = 0; i < headers.Length; i++)
		{
			if (headers[i].Contains(languageId))
			{
				return i;
			}
		}

		return -1;
	}

    public Dictionary<string, string> GetDictionaryValues(LocalisationSystem.Language language)
	{
		string languageId = LocalisationSystem.GetLanguageID(language);

        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        string[] lines = csvFile.text.Split(lineSeparator);

		int languageIndex = GetLanguageIndex(language);

        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

		for (int i = 1; i < lines.Length; i++)
		{
            string line = lines[i];

            string[] fields = CSVParser.Split(line);

			for (int f = 0; f < fields.Length; f++)
			{
				fields[f] = fields[f].TrimStart(' ', surround);
				fields[f] = fields[f].TrimEnd(surround);
			}

			if (fields.Length > languageIndex)
			{
				var key = fields[0];

				if (dictionary.ContainsKey(key)) { continue; }

				var value = fields[languageIndex];

				dictionary.Add(key, value);
			}
		}

		return dictionary;
	}

#if UNITY_EDITOR
	public void Edit(string key, string value, LocalisationSystem.Language language = LocalisationSystem.Language.English)
	{
		string[] args = new string[Enum.GetNames(typeof(LocalisationSystem.Language)).Length + 1];

		for (int i = 1; i < args.Length; i++)
		{
			args[i] = LocalisationSystem.GetLocalisedValue(key, i - 1);
		}
		args[0] = key;
		args[GetLanguageIndex(language)] = value;

		string[] result = new string[args.Length];

		for (int i = 0; i < args.Length; i++)
		{
			result[i] = args[i];
		}
		result[0] = "\n\"" + result[0];
		result[result.Length - 1] += "\"";

		Remove(key);
		string appended = string.Format(string.Join("\",\"", result));
		File.AppendAllText("Assets/Resources/localisation.csv", appended);

		AssetDatabase.Refresh();
	}

	public void Remove(string key)
	{
		string[] lines = csvFile.text.Split(lineSeparator);
		string[] keys = new string[lines.Length];

		for (int i = 0; i < lines.Length; i++)
		{
			string line = lines[i];
			keys[i] = line.Split(fieldSeparator, StringSplitOptions.None)[0];
		}

		int index = -1;

		for (int i = 0; i < keys.Length; i++)
		{
			if(keys[i].Contains(key))
			{
				index = i;
				break;
			}
		}

		if (index > -1)
		{
			string[] newLines;
			newLines = lines.Where(w => w != lines[index]).ToArray();

			string replaced = string.Join(lineSeparator.ToString(), newLines);
			File.WriteAllText("Assets/Resources/localisation.csv", replaced);
		}
	}
#endif
}