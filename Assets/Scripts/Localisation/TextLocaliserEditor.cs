using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class TextLocaliserEditWindow : EditorWindow
{
	static bool close;
	public static void Open(string key, bool closeOnEdit = false)
	{
		close = closeOnEdit;
		var window = EditorWindow.GetWindow<TextLocaliserEditWindow>();
		window.titleContent = new GUIContent("Localiser Window", (Texture)Resources.Load("plus"), "Edit window for easy access to manipulate localisation");
		window.ShowUtility();
		window.key = key;
	}
	[MenuItem("Window/Localisation/Text Localiser Edit Window")]
	public static void Open()
	{
		close = false;
		var window = EditorWindow.GetWindow<TextLocaliserEditWindow>();
		window.titleContent = new GUIContent("Localiser Window", (Texture)Resources.Load("plus"), "Edit window for easy access to manipulate localisation");
		window.ShowUtility();
		window.key = "";
	}

	public string key;
	public string value;
	LocalisationSystem.Language selectedLanguage = LocalisationSystem.CurrentLanguage;

	public void OnGUI()
	{
		string buttonText = "Add | Edit";

		EditorGUILayout.BeginHorizontal();
		if (close)
		{
			EditorGUILayout.LabelField("Key:", key);
			buttonText = "Edit";
		}
		else
		{
			key = EditorGUILayout.TextField("Key:", key);

			if (LocalisationSystem.GetDictionaryForEditor().ContainsKey(key))
			{
				buttonText = "Edit";
			}
			else
			{
				buttonText = "Add";
			}
		}
		LocalisationSystem.SetLanguage(selectedLanguage = (LocalisationSystem.Language)EditorGUILayout.EnumPopup(selectedLanguage));
		EditorGUILayout.EndHorizontal();

		if (LocalisationSystem.GetDictionaryForEditor().ContainsKey(key)) { EditorGUILayout.LabelField("Current Value: ", LocalisationSystem.GetLocalisedValue(key)); }
		else { EditorGUILayout.LabelField("Current Value: ", ""); }

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Value:", GUILayout.MaxWidth(50));
		EditorStyles.textArea.wordWrap = true;

		if (value == null || value == string.Empty) { value = ""; }
		value = EditorGUILayout.TextArea(value, EditorStyles.textArea, GUILayout.Height(100), GUILayout.Width(400)); 
		EditorGUILayout.EndHorizontal();

		if (GUILayout.Button(buttonText))
		{
			if (value != string.Empty && value != null && key != string.Empty && key != null)
			{
				LocalisationSystem.Edit(key, value);
			}
			if (close) { EditorWindow.GetWindow<TextLocaliserEditWindow>().Close(); }
		}

		minSize = new Vector2(460, 250);
		maxSize = minSize;
	}
}

public class TextLocaliserSearchWindow : EditorWindow
{
	static string value; // search value

	[MenuItem("Window/Localisation/Text Localiser Search Window")]
	public static void Open()
	{
		var window = EditorWindow.GetWindow<TextLocaliserSearchWindow>();
		window.titleContent = new GUIContent("Localisation Search", (Texture)Resources.Load("magnify"), "Search window for easy access to localisation fields");
		value = "";
		window.Show();
	}
	public static void Open(string searchFor)
	{
		var window = EditorWindow.GetWindow<TextLocaliserSearchWindow>();
		window.titleContent = new GUIContent("Localisation Search", (Texture)Resources.Load("magnify"), "Search window for easy access to localisation fields");
		value = searchFor;
		window.Show();
	}

	Vector2 scroll;
	Dictionary<string, string> dictionary;
	LocalisationSystem.Language selectedLanguage = LocalisationSystem.CurrentLanguage;

	private void OnEnable()
	{
		dictionary = LocalisationSystem.GetDictionaryForEditor();
	}

	public void OnGUI()
	{
		EditorGUILayout.BeginHorizontal("Box");
		EditorGUILayout.LabelField("Search: ", EditorStyles.boldLabel);
		value = EditorGUILayout.TextField(value);
		LocalisationSystem.SetLanguage(selectedLanguage = (LocalisationSystem.Language)EditorGUILayout.EnumPopup(selectedLanguage));
		dictionary = LocalisationSystem.GetDictionaryForEditor();
		EditorGUILayout.EndHorizontal();

		GetSearchResults();
	}

	private void GetSearchResults()
	{
		if (value == null || value == string.Empty)
		{
			foreach (KeyValuePair<string, string> element in dictionary)
			{
				EditorGUILayout.BeginHorizontal("Box");

				if (GUILayout.Button((Texture)Resources.Load("close"), GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
				{
					if (EditorUtility.DisplayDialog("Remove Key " + element.Key + "?", "This will remove the element from localisation, are you sure?", "Do it", "Nevermind"))
					{
						LocalisationSystem.Remove(element.Key);
						AssetDatabase.Refresh();
						LocalisationSystem.Init();
						dictionary = LocalisationSystem.GetDictionaryForEditor();
					}
				}

				EditorGUILayout.LabelField(element.Key);
				EditorGUILayout.LabelField(element.Value);

				if (GUILayout.Button((Texture)Resources.Load("edit"), GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
				{
					TextLocaliserEditWindow.Open(element.Key, true);
				}

				EditorGUILayout.EndHorizontal();
			}
			return;
		}

		EditorGUILayout.BeginVertical();
		scroll = EditorGUILayout.BeginScrollView(scroll);

		foreach (KeyValuePair<string, string> element in dictionary)
		{
			if (element.Key.ToLower().Contains(value.ToLower()) || element.Value.ToLower().Contains(value.ToLower()))
			{
				EditorGUILayout.BeginHorizontal("Box");

				if (GUILayout.Button((Texture)Resources.Load("close"), GUILayout.MaxWidth(20),GUILayout.MaxHeight(20)))
				{
					if (EditorUtility.DisplayDialog("Remove Key " + element.Key + "?", "This will remove the element from localisation, are you sure?", "Do it", "Nevermind"))
					{
						LocalisationSystem.Remove(element.Key);
						AssetDatabase.Refresh();
						LocalisationSystem.Init();
						dictionary = LocalisationSystem.GetDictionaryForEditor();
					}
				}

				EditorGUILayout.LabelField(element.Key);
				EditorGUILayout.LabelField(element.Value);

				if (GUILayout.Button((Texture)Resources.Load("edit"), GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
				{
					TextLocaliserEditWindow.Open(element.Key, true);
				}

				EditorGUILayout.EndHorizontal();
			}
		}
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
}
#endif