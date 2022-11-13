using UnityEngine;
using UnityEditor;
using TMPro;

[ExecuteInEditMode]
[RequireComponent(typeof(TextMeshProUGUI))]
public class VersionText : MonoBehaviour
{
#if UNITY_EDITOR
	private void Start()
	{
		UpdateVersionText();
	}

	private void Reset()
	{
		UpdateVersionText();
	}

	private void OnEnable()
	{
		UpdateVersionText();
	}

	void UpdateVersionText()
	{
		GetComponent<TextMeshProUGUI>().text = PlayerSettings.bundleVersion;
	}
#endif
}