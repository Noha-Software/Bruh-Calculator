using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Text;

public class RemarkableIdentities : MonoBehaviour
{
    public GameObject currentTextSelcted;
    public GameObject tabs;

    int idxTracker = 0;

    bool isVariable;
    bool isIndex;

    int x;

    StringBuilder output;
    StringBuilder currentComponent;

    public TMP_InputField aInput;
    public TMP_InputField bInput;
    public TextMeshProUGUI outputText;

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
        GameObject currentPageSelected = tabs.GetComponent<TabGroup>().currentPageOpen;
        x = Int32.Parse(GameObject.Find(currentPageSelected.name + "indexInput").GetComponent<TMP_InputField>().text) + 1;
        aInput = GameObject.Find(currentPageSelected.name + "/aInput").GetComponent<TMP_InputField>();
        bInput = GameObject.Find(currentPageSelected.name + "/bInput").GetComponent<TMP_InputField>();
        outputText = GameObject.Find(currentPageSelected.name + "/outputText").GetComponent<TextMeshProUGUI>();
    }
    public void Output()
    {
        GetInputs();
        SortData(aInput.text, aList);
        SortData(bInput.text, bList);

        //Pascal háromszög
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
        outputText.text = output.ToString();
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
                    currentComponent.Append(c);
                }
                else
                {
                    SendToList(list);
                    currentComponent.Append(c);
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
                bool hasNumber;
                if (Int64.TryParse(s, out long res))
                {
                    int n = (int)res;
                    int amog = 0;
                    for (int i = 0; i < (idx-1); ++i) if(idx != 1)
                    {

                        res *= n;
                        //Debug.Log(res + " " + idx + " " + i);
                    }
                    res *= pascal[b];
                    output.Append(amog);
                    //Debug.Log(res);
                }
                else
                {
                    if (s.Length == 1)
                    {
                        output.Append(s);
                        if (idx != 1) output.Append("<sup>" + idx + "</sup>");
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
    if (a != 0) output.Append(" + ");
    }
    public void SendToList(List<string> list)
    {
        if (currentComponent.ToString() != "") list.Add(currentComponent.ToString());
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
