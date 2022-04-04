using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Text;

public class RemarkableIdentities : MonoBehaviour
{
    public GameObject currentTextSelcted;

    string aInputName;
    string bInputName;
    int idxTracker = 0;

    bool isNumber;
    bool isVariable;
    bool isIndex;

    StringBuilder output;
    StringBuilder currentComponent;

    public string aInputField;
    public string bInputField;

    [SerializeField] List<string> aList;
    [SerializeField] List<string> bList;

    private void Start()
    {
        aList = new List<string>();
        bList = new List<string>();
        output = new StringBuilder(50);
        currentComponent = new StringBuilder();
    }
    public void GetInputs()
    {
        //Get the inputs as strings
        aInputName = GameObject.Find("Tab 3 - Remarkable Identities/Tabs/").GetComponent<TabGroup>().currentPageOpen.name + "/aInput";
        bInputName = GameObject.Find("Tab 3 - Remarkable Identities/Tabs/").GetComponent<TabGroup>().currentPageOpen.name + "/bInput";
        aInputField = GameObject.Find(aInputName).GetComponent<TMP_InputField>().text;
        bInputField = GameObject.Find(bInputName).GetComponent<TMP_InputField>().text;
    }
    public void Output()
    {
        GetInputs();
        SortData(aInputField, aList);
        SortData(bInputField, bList);
        Calculate(aList);        
        Calculate(bList);
        output.Clear();
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
                    currentComponent.Append(c);
                }
                else if (isVariable == false)
                {
                    isNumber = true;
                    currentComponent.Append(c);
                }
                else
                {
                    SendToList(list);
                    currentComponent.Append(c);
                    isNumber = true;
                }
            }
            else if (((int)c <= 90 && (int)c >= 65) || ((int)c <= 122 && (int)c >= 97) && idxTracker < 2 && (int)c != 94)
            {
                SendToList(list);
                isVariable = true;
                currentComponent.Append(c);
            }
            else if ((int)c == 94)
            {
                isIndex = true;
                idxTracker++;
            }
            if (idxTracker > 1)
            {
                isIndex = false;
                SendToList(list);
                idxTracker = 0;
            }

            //Debug.Log(currentComponent);
        }
        SendToList(list);
    }
    public void Calculate(List<string> list)
    {
        foreach(string s in list)
        {
            if (Int64.TryParse(s, out long result))
            {
                output.Append(result *= result);
            }
            else
            {
                if(s.Length == 1)
                {
                    output.Append(s + "<sup>2</sup>");
                }
                else
                {
                    long v = Int64.Parse(s.Trim(s[0]));                   
                    output.Append(s[0] + "<sup>" + (v*2).ToString() + "</sup>");                    
                }
            }
        }
        Debug.Log(output);
    }
    public void SendToList(List<string> list)
    {
        if (currentComponent.ToString() != "") list.Add(currentComponent.ToString());
        isNumber = false;
        isVariable = false;
        currentComponent.Clear();
    }
    public void ClosePage()
    {
        GameObject currentPageOpen = GameObject.Find("Tab 3 - Remarkable Identities/Tabs").GetComponent<TabGroup>().currentPageOpen;
        currentPageOpen.SetActive(false);
    }
    public void AddUpperIndex()
    {
        TMP_InputField field = currentTextSelcted.GetComponent<TMP_InputField>();
        field.text += "^";
        //Debug.Log(field.caretPosition);
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
