using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TextLocaliserEditWindow : EditorWindow
{
	public static void Open(string key)
	{
		var window = EditorWindow.GetWindow<TextLocaliserEditWindow>();
		window.titleContent = new GUIContent("Localiser Window", (Texture)Resources.Load("plus"), "Edit window for easy access to manipulate localisation");
		window.ShowUtility();
		window.key = key;
	}
	[MenuItem("Window/Localisation/Text Localiser Edit Window")]
	public static void Open()
	{
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
		EditorGUILayout.BeginHorizontal();
		key = EditorGUILayout.TextField("Key:", key);
		LocalisationSystem.SetLanguage(selectedLanguage = (LocalisationSystem.Language)EditorGUILayout.EnumPopup(selectedLanguage));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.LabelField("Current Value: ", LocalisationSystem.GetLocalisedValue(key));
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Value:", GUILayout.MaxWidth(50));
		EditorStyles.textArea.wordWrap = true;
		value = EditorGUILayout.TextArea(value, EditorStyles.textArea, GUILayout.Height(100), GUILayout.Width(400)); 
		EditorGUILayout.EndHorizontal();

		if(GUILayout.Button("Add"))
		{
			if(LocalisationSystem.GetLocalisedValue(key, selectedLanguage) != string.Empty || LocalisationSystem.GetLocalisedValue(key, selectedLanguage) != string.Empty)
			{
				if (value != string.Empty && value != null && key != string.Empty && key != null) { LocalisationSystem.Edit(key, value); }
			}
			else
			{
				if (value != string.Empty && value != null && key != string.Empty && key != null) { LocalisationSystem.Edit(key, value); }
			}
			//EditorWindow.GetWindow<TextLocaliserEditWindow>().Close();
		}

		minSize = new Vector2(460, 250);
		maxSize = minSize;
	}
}

public class TextLocaliserSearchWindow : EditorWindow
{
	[MenuItem("Window/Localisation/Text Localiser Search Window")]
	public static void Open()
	{
		var window = EditorWindow.GetWindow<TextLocaliserSearchWindow>();
		window.titleContent = new GUIContent("Localisation Search", (Texture)Resources.Load("magnify"), "Search window for easy access to localisation fields");

		Vector2 mouse = GUIUtility.ScreenToGUIPoint(Input.mousePosition);
		Rect r = new Rect(mouse.x - 450, mouse.y + 10, 10, 10);
		window.ShowAsDropDown(r, new Vector2(500, 300));
	}
	public static void Open(TextLocaliserUI textLocaliserUI)
	{
		var window = EditorWindow.GetWindow<TextLocaliserSearchWindow>();
		window.titleContent = new GUIContent("Localisation Search", (Texture)Resources.Load("magnify"), "Search window for easy access to localisation fields");

		Vector2 mouse = GUIUtility.ScreenToGUIPoint(Input.mousePosition);
		Rect r = new Rect(mouse.x, mouse.y, 10, 10);
		window.ShowAsDropDown(r, new Vector2(500, 300));
	}

	public string value;
	public Vector2 scroll;
	public Dictionary<string, string> dictionary;
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
		if (value == null) { return; }

		EditorGUILayout.BeginVertical();
		scroll = EditorGUILayout.BeginScrollView(scroll);

		foreach (KeyValuePair<string, string> element in dictionary)
		{
			/*if (element.Key.ToLower().Contains("key"))
			{
				continue;
			}*/
			if (element.Key.ToLower().Contains(value.ToLower()) || element.Value.ToLower().Contains(value.ToLower()))
			{
				EditorGUILayout.BeginHorizontal("Box");
				Texture closeIcon = (Texture)Resources.Load("close");

				GUIContent content = new GUIContent(closeIcon);

				if (GUILayout.Button(content,GUILayout.MaxWidth(20),GUILayout.MaxHeight(20)))
				{
					if (EditorUtility.DisplayDialog("Remove Key " + element.Key + "?", "This will remove the element from localisation, are you sure?", "Do it", "Nevermind"))
					{
						LocalisationSystem.Remove(element.Key, selectedLanguage);
						AssetDatabase.Refresh();
						LocalisationSystem.Init();
						dictionary = LocalisationSystem.GetDictionaryForEditor();
					}
				}

				EditorGUILayout.LabelField(element.Key);
				EditorGUILayout.LabelField(element.Value);
				EditorGUILayout.EndHorizontal();
			}
		}
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
}

public class LocalisationEditWindow : EditorWindow
{
	[MenuItem("Window/Localisation/Edit Window")]
	public static void Open()
	{
		var window = EditorWindow.GetWindow<LocalisationEditWindow>();
		window.titleContent = new GUIContent("Localisation Fields Editor", (Texture)Resources.Load("magnify"), "Edit window for easy access to localisation fields");
		window.Show();
	}

	public string searchValue;
	public Vector2 scroll;
	public Dictionary<string, string> dictionary;

	LocalisationSystem.Language selectedLanguage = LocalisationSystem.CurrentLanguage;

	private void OnEnable()
	{
		dictionary = LocalisationSystem.GetDictionaryForEditor();
	}

	public void OnGUI()
	{
		EditorGUILayout.BeginHorizontal("Box");
		EditorGUILayout.LabelField("Search: ", EditorStyles.boldLabel);
		searchValue = EditorGUILayout.TextField(searchValue);
		LocalisationSystem.SetLanguage(selectedLanguage = (LocalisationSystem.Language)EditorGUILayout.EnumPopup(selectedLanguage));
		dictionary = LocalisationSystem.GetDictionaryForEditor();
		EditorGUILayout.EndHorizontal();

		GetSearchResults();

		if (GUILayout.Button(new GUIContent("Add new field")))
		{
			LocalisationSystem.Edit("", "");
			AssetDatabase.Refresh();
			LocalisationSystem.Init();
			dictionary = LocalisationSystem.GetDictionaryForEditor();
		}
	}

	private void GetSearchResults()
	{
		if (searchValue == null) { return; }

		EditorGUILayout.BeginVertical();
		scroll = EditorGUILayout.BeginScrollView(scroll);

		foreach (KeyValuePair<string, string> element in dictionary)
		{
			if (element.Key.ToLower().Contains(searchValue.ToLower()) || element.Value.ToLower().Contains(searchValue.ToLower()))
			{
				EditorGUILayout.BeginHorizontal("Box");
				Texture closeIcon = (Texture)Resources.Load("close");

				GUIContent content = new GUIContent(closeIcon);

				if (GUILayout.Button(content, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
				{
					if (EditorUtility.DisplayDialog("Remove Key " + element.Key + "?", "This will remove the element from localisation, are you sure?", "Do it", "Nevermind"))
					{
						LocalisationSystem.Remove(element.Key, selectedLanguage);
						AssetDatabase.Refresh();
						LocalisationSystem.Init();
						dictionary = LocalisationSystem.GetDictionaryForEditor();
					}
				}

				EditorGUILayout.TextField(element.Key);
				EditorGUILayout.TextField(element.Value);
				EditorGUILayout.EndHorizontal();
			}
		}
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
}