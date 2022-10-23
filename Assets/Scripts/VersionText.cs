using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[ExecuteInEditMode]
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
		GetComponent<Text>().text = PlayerSettings.bundleVersion;
	}
#endif
}