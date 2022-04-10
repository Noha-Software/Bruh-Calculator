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
    string indexInputName;
    string outputName;
    int idxTracker = 0;

    bool isNumber;
    bool isVariable;
    bool isIndex;

    int x;

    StringBuilder output;
    StringBuilder currentComponent;

    public string aInputField;
    public string bInputField;
    public string outputField;

    [SerializeField] List<string> aList;
    [SerializeField] List<string> bList;
    List<int> pascal;

    private void Start()
    {
        aList = new List<string>();
        bList = new List<string>();
        output = new StringBuilder(50);
        currentComponent = new StringBuilder();
        pascal = new List<int>();
    }
    public void GetInputs()
    {
        string pagename = GameObject.Find("Tab 3 - Remarkable Identities/Tabs/").GetComponent<TabGroup>().currentPageOpen.name;
        indexInputName = pagename + "/indexInput";
        aInputName = pagename + "/aInput";
        bInputName = pagename + "/bInput";
        outputName = pagename + "/outputText";
        x = Int32.Parse(GameObject.Find(indexInputName).GetComponent<TMP_InputField>().text) + 1;
        aInputField = GameObject.Find(aInputName).GetComponent<TMP_InputField>().text;
        bInputField = GameObject.Find(bInputName).GetComponent<TMP_InputField>().text;
        outputField = GameObject.Find(outputName).GetComponent<TextMeshProUGUI>().text;
    }
    public void Output()
    {
        GetInputs();
        SortData(aInputField, aList);
        SortData(bInputField, bList);

        int val = 1, blank, j;
        for (int i = 0; i < x; i++)
        {
            for (blank = 1; blank <= x - i; blank++)
                for (j = 0; j <= i; j++)
                {
                    if (j == 0 || i == 0)
                        val = 1;
                    else val = val * (i - j + 1) / j;
                    if (x - 1 == i) pascal.Add(val);
                }
        }

        for (int i = 0; i < x; ++i)
        {            
            Calculate(((x-1) - i), i);            
            //Debug.Log((x-1) - i);
        }
        Debug.Log(output);
        outputField = output.ToString();
        //outputField = "bruh";
        pascal.Clear();
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
    public void Calculate(int a, int b)
    {
        void FuckMe(int idx, List<string> list)
        {
            if (idx != 0) foreach (string s in list)
            {
                if (Int64.TryParse(s, out long res))
                {
                    int n = (int)res;
                    for (int i = 0; i < (idx-1); ++i) if(idx != 1)
                    {

                        res *= n;
                        //Debug.Log(res + " " + idx + " " + i);
                    }
                    res *= pascal[b];
                    output.Append(res);
                    //Debug.Log(res);
                }
                else
                {
                    if (s.Length == 1)
                    {
                        output.Append(s + "<sup>" + idx + "</sup>");
                    }
                    else
                    {
                        long v = Int64.Parse(s.Trim(s[0]));
                        output.Append(s[0] + "<sup>" + (v * idx) + "</sup>");
                    }
                }
            }
        }
        FuckMe(a, aList);
        FuckMe(b, bList);
        output.Append(" + ");
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
