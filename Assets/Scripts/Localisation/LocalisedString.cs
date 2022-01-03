using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LocalisedString
{
	public string key;
    public LocalisedString(string key)
	{
		this.key = key;
	}

	public string value
	{
		get
		{
			return LocalisationSystem.GetLocalisedValue(key, LocalisationSystem.CurrentLanguage);
		}
#if UNITY_EDITOR
		set
		{
			LocalisationSystem.Edit(key, value, LocalisationSystem.CurrentLanguage);
		}
#endif
	}

	public static implicit operator LocalisedString(string key)
	{
		return new LocalisedString(key);
	}
}
