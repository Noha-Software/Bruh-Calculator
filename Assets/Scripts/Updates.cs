using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;
using UnityEngine.UI;

public class Updates : MonoBehaviour
{
	string latestVersionString;
	public Version latestVersion;
	public Version currentVersion;

	[SerializeField] GameObject updateCheckButton;
	[SerializeField] GameObject updateImage;
	[SerializeField] GameObject updateButton;
	[SerializeField] Sprite magnify;
	[SerializeField] Sprite checkmark;

	public bool IsUpToDate()
	{
		ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
		return currentVersion >= latestVersion;
	}

	public struct userAttributes { }
	public struct appAttributes { }
	private void Awake()
	{
		ConfigManager.FetchCompleted += SetVersion;
		ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
	}
	private void OnEnable()
	{
		ConfigManager.FetchCompleted += SetVersion;
	}
	private void OnDestroy()
	{
		ConfigManager.FetchCompleted -= SetVersion;
	}
	private void OnDisable()
	{
		ConfigManager.FetchCompleted -= SetVersion;
	}

	/// <summary>
	/// Get version from Remote Config
	/// </summary>
	void SetVersion(ConfigResponse response)
	{
		latestVersionString = ConfigManager.appConfig.GetString("latest_version");
		latestVersion = new Version(latestVersionString);
		currentVersion = new Version(PlayerPrefs.GetString("version"));
	}

	public void CheckForUpdates()
	{
		updateImage.GetComponentInChildren<Image>().sprite = magnify;
		if (!IsUpToDate())
		{
			updateImage.SetActive(false);
			updateCheckButton.SetActive(false);
			updateButton.SetActive(true);
		}
		else
		{
			updateImage.GetComponentInChildren<Image>().sprite = checkmark;
		}
	}

	public void UpdateURL()
	{
		Application.OpenURL("https://drive.google.com/drive/folders/14PiUiJB9eCW9DsxvSI7k2xbMOLiDGsKF?usp=share_link");
	}
}

public class Version
{
	public int major, minor, patch;
	public string label;

	public Version(string versionText)
	{
		versionText.Trim();
		string[] elements = versionText.Split('.', '-');
		if (elements.Length >= 4)
		{
			major = int.Parse(elements[0].Trim('v'));
			minor = int.Parse(elements[1]);
			patch = int.Parse(elements[2]);
			label = elements[3];
		}
	}

	public override string ToString()
	{
		string output = "v" + string.Join(".", major, minor, patch);
		if (!string.IsNullOrWhiteSpace(label))
		{
			output = output + '-' + label;
		}
		return output;
	}
	public static bool operator >(Version a, Version b)
	{
		bool isBigger = false;
		if (a.major > b.major)
		{
			isBigger = true;
		}
		else if (a.major == b.major)
		{
			if (a.minor > b.minor)
			{
				isBigger = true;
			}
			else if (a.minor == b.minor)
			{
				if (a.patch > b.patch)
				{
					isBigger = true;
				}
			}
		}
		return isBigger;
	}
	public static bool operator >=(Version a, Version b)
	{
		return a > b || a == b;
	}
	public static bool operator <(Version a, Version b)
	{
		bool isSmaller = false;
		if (a.major < b.major)
		{
			isSmaller = true;
		}
		else if (a.major == b.major)
		{
			if (a.minor < b.minor)
			{
				isSmaller = true;
			}
			else if (a.minor == b.minor)
			{
				if (a.patch < b.patch)
				{
					isSmaller = true;
				}
			}
		}
		return isSmaller;
	}
	public static bool operator <=(Version a, Version b)
	{
		return a < b || a == b;
	}
	public static bool operator ==(Version a, Version b)
	{
		if (a.major == b.major && a.minor == b.minor && a.patch == b.patch && a.label == b.label)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public override bool Equals(object obj)
	{
		Version a = this;
		Version b = (Version)obj;

		if (a.major == b.major && a.minor == b.minor && a.patch == b.patch && a.label == b.label)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public static bool operator !=(Version a, Version b)
	{
		if (a.major != b.major || a.minor == b.minor || a.patch == b.patch || a.label == b.label)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}