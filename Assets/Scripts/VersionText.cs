using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[ExecuteInEditMode]
public class VersionText : MonoBehaviour
{
    void Start()
    {
        GetComponent<Text>().text = PlayerSettings.bundleVersion;
    }
}
