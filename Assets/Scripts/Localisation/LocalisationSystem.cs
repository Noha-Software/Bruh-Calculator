using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalisationSystem : MonoBehaviour
{
    public enum Language
	{
		English,
		Hungarian
	}

	public static Language language = Language.English;

	private static Dictionary<string, string> localisedEN;
	private static Dictionary<string, string> localisedHU;

	public static bool isInit;

	public static void Init()
	{
		CSVLoader csvLoader = new CSVLoader();
		csvLoader.LoadCSV();

		localisedEN = csvLoader.GetDictionaryValues("en");
		localisedHU = csvLoader.GetDictionaryValues("hu");

		isInit = true;
	}

	public static string GetLocalisedValue(string key)
	{
		if(!isInit) { Init(); }

		string value = key;

		switch (language)
		{
			case Language.English:
				localisedEN.TryGetValue(key, out value);
				break;
			case Language.Hungarian:
				localisedHU.TryGetValue(key, out value);
				break;
		}

		return value;
	}
}
