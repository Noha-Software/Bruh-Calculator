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
    string aOutput;
    string bOutput;
    int listIndexIdx = 0;
    int idxTracker = 0;
    string currentComponent = "";

    bool isNumber;
    bool isVariable;
    bool isIndex;

    public string aInputField;
    public string bInputField;

    [SerializeField] List<List<string>> aList;
    [SerializeField] List<List<string>> bList;        
    private void Start()
    {
        aList = new List<List<string>>() { new List<string>(), new List<string>(), new List<string>()};
        bList = new List<List<string>>() { new List<string>(), new List<string>(), new List<string>()};

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
        /*for(int i = 0; i < 6; ++i)
        {
            for(int idx = 0; idx < aList[i].Count; ++idx) Debug.Log("alist[" + i +"]["+idx+"]: "+aList[i][idx]+"");
        }
        for (int i = 0; i < 6; ++i)
        {
            for (int idx = 0; idx < bList[i].Count; ++idx) Debug.Log("blist[" + i + "][" + idx + "]: " + bList[i][idx] + "");
        }*/
        listIndexIdx = 0;
    }
    public void SortData(string input, List<List<string>> list)
    {      
        foreach(List<string> s in list)
        {
            s.Clear();
        }

        idxTracker = 0;

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
                    SendToLists(list[1]);
                    currentComponent += c;
                    isNumber = true;
                }
            }
            else if (((int)c <= 90 && (int)c >= 65) || ((int)c <= 122 && (int)c >= 97) && idxTracker < 2 && (int)c != 94)
            {
                //Debug.Log(currentComponent + isNumber);
                if (isNumber)
                {
                    SendToLists(list[0]);
                }
                SendToLists(list[1]);
                isVariable = true;
                currentComponent += c;
            }
            else if ((int)c == 94)
            {
                if (!isIndex)
                {
                    if (isNumber) SendToLists(list[0]);
                    else SendToLists(list[1]);
                    isIndex = true;
                }
                ++idxTracker;
            }
            if (idxTracker > 1)
            { 
                SendToLists(list[3]);                
            }

            //Debug.Log(currentComponent);
        }
        if(isNumber) SendToLists(list[0]);
        if(isVariable) SendToLists(list[1]);
    }
    public void Calculate(List<string> numbers, List<string> output, List<string> indexList)
    {
        //Ezt majd írd át
        output.Clear();
        int i;
        int idx = 0;
        foreach (string s in numbers)
        {            
            i = Int32.Parse(s);
            output.Add((i*i).ToString());
            Debug.Log(output[idx]);
            ++idx;
        }
    }
    public void SendToLists(List<string> list)
    {
        if (currentComponent != "") list.Add(currentComponent);
        isIndex = false;
        isNumber = false;
        isVariable = false;       
        idxTracker = 0;
        currentComponent = "";        
        listIndexIdx++;
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
