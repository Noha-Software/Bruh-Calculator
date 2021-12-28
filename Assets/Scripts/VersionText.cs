using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[ExecuteInEditMode]
public class VersionText : MonoBehaviour
{
    private void Start()
    {
		UpdateVersionText();
    }

	private void Awake()
	{
		UpdateVersionText();
	}

	private void Reset()
	{
		UpdateVersionText();
	}

	void UpdateVersionText()
	{
		GetComponent<Text>().text = PlayerSettings.bundleVersion;
	}
}
