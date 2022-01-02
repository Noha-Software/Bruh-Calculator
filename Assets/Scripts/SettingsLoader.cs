using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class SettingsLoader
{
	private static Settings settings;

	public static void LoadSettings(string fileName = "settings")
	{
		if (string.IsNullOrEmpty(fileName))
		{
			throw new System.ArgumentException("message", nameof(fileName));
		}

		if (File.Exists("Resources/" + fileName))
		{
			settings = Resources.Load<Settings>(fileName);
		}
		else
		{
			settings = (Settings)ScriptableObject.CreateInstance(typeof(Settings));
		}
	}

	public static Settings GetSettings(string fileName = "settings")
	{
		LoadSettings(fileName);
		return settings;
	}

	public static void SaveSettings(Settings settings, string fileName = "settings")
	{
		if (File.Exists("Assets/Resources/" + fileName))
		{
			AssetDatabase.DeleteAsset("Assets/Resources/" + fileName + ".asset");
			AssetDatabase.CreateAsset(settings, "Assets/Resources/" + fileName + ".asset");
		}
		else
		{
			AssetDatabase.CreateAsset(settings, "Assets/Resources/" + fileName + ".asset");
		}
	}
}
