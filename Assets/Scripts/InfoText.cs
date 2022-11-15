using UnityEngine;
using UnityEditor;
using TMPro;

[ExecuteInEditMode]
public class InfoText : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI infoText;
#if UNITY_EDITOR
	private void Reset()
	{
		infoText.text = GetInformation();
	}
	private void OnEnable()
	{
		infoText.text = GetInformation();
	}
	private void Start()
	{
		infoText.text = GetInformation();
	}

	string GetInformation()
	{
		int year = System.DateTime.Now.Year;
		string version = PlayerSettings.bundleVersion;
		PlayerPrefs.SetString("version", version);
		string company = PlayerSettings.companyName;
		return "© " + year + ' ' + company + '\n' + version;
	}
#endif

	public void GitHub()
	{
		Application.OpenURL("https://www.github.com/Noha-Software");
	}
	public void Discord()
	{
		Application.OpenURL("https://discord.gg/fbaB8nFzRS");
	}
	public void Email()
	{
		Application.OpenURL("mailto://noha.software@gmail.com");
	}
}
