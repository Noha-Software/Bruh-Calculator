using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class RemarkableIdentities : MonoBehaviour
{
    public GameObject currentTextSelcted;

    string aInputName;
    string bInputName;
    string output;
    int idxTracker = 0;
    string currentComponent = "";

    bool isNumber;
    bool isVariable;
    bool isIndex;

    public string aInputField;
    public string bInputField;

    [SerializeField] List<string> aList;
    [SerializeField] List<string> bList;

    private void Start()
    {
        aList = new List<string>();
        bList = new List<string>();
    }
    public void GetInputs()
    {
        //Get the inputs as strings
        aInputName = GameObject.Find("Tab 3 - Remarkable Identities/Tabs/").GetComponent<TabGroup>().currentPageOpen.name + "/aInput";
        bInputName = GameObject.Find("Tab 3 - Remarkable Identities/Tabs/").GetComponent<TabGroup>().currentPageOpen.name + "/bInput";
        aInputField = GameObject.Find(aInputName).GetComponent<TMP_InputField>().text;
        bInputField = GameObject.Find(bInputName).GetComponent<TMP_InputField>().text;        
    }
    public void RIOutput()
    {
        GetInputs();
        SortData(aInputField, aList);
        SortData(bInputField, bList);
    }
    public void SortData(string input, List<string> list)
    {
        list.Clear();
        foreach (char c in input)
        {
            if ((int)c <= 57 && (int)c >= 48 && idxTracker < 2)
            {
                if (isIndex)
                {
                    currentComponent += c;
                }
                else if (isVariable == false)
                {
                    isNumber = true;
                    currentComponent += c;
                }
                else
                {
                    SendToList(list);
                    currentComponent += c;
                    isNumber = true;
                }
            }
            else if (((int)c <= 90 && (int)c >= 65) || ((int)c <= 122 && (int)c >= 97) && idxTracker < 2 && (int)c != 94)
            {
                SendToList(list);
                isVariable = true;
                currentComponent += c;
            }
            else if ((int)c == 94)
            {
                if (!isIndex && !isVariable)
                {
                    isIndex = true;
                    SendToList(list);                    
                }
                ++idxTracker;
            }
            if (idxTracker > 1)
            {
                isIndex = false;
                SendToList(list);
                idxTracker = 0;
            }

            Debug.Log(currentComponent);
        }
        SendToList(list);
    }
    public void SendToList(List<string> list)
    {
        if (currentComponent != "") list.Add(currentComponent);        
        isNumber = false;
        isVariable = false;               
        currentComponent = "";
    }
    public void ClosePage()
    {
        GameObject currentPageOpen = GameObject.Find("Tab 3 - Remarkable Identities/Tabs").GetComponent<TabGroup>().currentPageOpen;
        currentPageOpen.SetActive(false);
    }   
    public void AddUpperIndex()
    {        
        currentTextSelcted.GetComponent<TMP_InputField>().text += "^";
    }
    public void AddDoubleUpperIndex()
    {        
        currentTextSelcted.GetComponent<TMP_InputField>().text += "^^";
    }
    public void SetThisSelectedTab(GameObject input)
    {
        currentTextSelcted = input;
    }
}
