using UnityEngine;
using UnityEditor;
using TMPro;

#if UNITY_EDITOR

[ExecuteInEditMode]
public class InfoText : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI infoText;

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
		string company = PlayerSettings.companyName;
		return "© " + year + ' ' + company + '\n' + version;
	}
#endif

	public void GitHubLink()
	{
		Debug.Log("Opening link...");
		Application.OpenURL("https://github.com/Noha-Software");
	}
	public void EmailLink()
	{
		Debug.Log("Opening link...");
		Application.OpenURL("mailto:noha.software@gmail.com");
	}
}
