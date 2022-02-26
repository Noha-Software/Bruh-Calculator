using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RemarkableIdentities : MonoBehaviour
{
    public string aInputField;
    public string bINputField;

    public void GetSliceInputs()
    {
        string aInputName = GameObject.Find("Tab 3 - Remarkable Identities/Tabs").GetComponent<TabGroup>().currentPageOpen.name + "/aInput";
        string bInputName = GameObject.Find("Tab 3 - Remarkable Identities/Tabs").GetComponent<TabGroup>().currentPageOpen.name + "/bInput";
        aInputField = GameObject.Find(aInputName).GetComponent<TMP_InputField>().text;
        bINputField = GameObject.Find(bInputName).GetComponent<TMP_InputField>().text;
        Debug.Log(aInputField + bINputField);

    }

    public void ClosePage()
    {
        GameObject currentPageOpen = GameObject.Find("Tab 3 - Remarkable Identities/Tabs").GetComponent<TabGroup>().currentPageOpen;
        currentPageOpen.SetActive(false);
    }
}
