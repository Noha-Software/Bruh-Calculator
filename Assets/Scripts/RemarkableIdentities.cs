﻿using System.Collections;
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

    List<string> aNumbers;
    List<string> aVariables;
    List<string> bNumbers;
    List<string> bVariables;
    List<string> aNumbersOutput;
    List<List<string>> indexedListA;
    List<string> aIndexes;
    List<List<string>> indexedListB;
    List<int> bIndexes;
    
    private void Start()
    {
        aNumbers = new List<string>();
        aVariables = new List<string>();
        bNumbers = new List<string>();
        bVariables = new List<string>();
        aNumbersOutput = new List<string>();
        indexedListA = new List<List<string>>();
        aIndexes = new List<string>();
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
        SortData(aInputField, aNumbers, aVariables);
        SortData(bInputField, bNumbers, bVariables);
        //Calculate(aNumbers, aNumbersOutput);
        listIndexIdx = 0;
    }
    public void SortData(string input, List<string> numberList, List<string> variableList)
    {      
        numberList.Clear();
        variableList.Clear();

        idxTracker = 0;
                       
        foreach (char c in input)
        {
            if ((int)c <= 57 && (int)c >= 48 && idxTracker < 2)
            {
               
                if (isVariable == false)
                {
                    isNumber = true;
                    continue;
                }
                else if (isVariable && idxTracker > 0)
                {
                    continue;
                }
                else
                {
                    SendToLists(variableList);
                }
            }
            else if (((int)c <= 90 && (int)c >= 65) || ((int)c <= 122 && (int)c >= 97) && idxTracker < 2 && (int)c != 94)
            {
                if (isNumber)
                {
                    SendToLists(numberList);
                }
                isVariable = true;
            }
            else if ((int)c == 94)
            {
                isIndex = true;
                ++idxTracker;
            }
            if (idxTracker > 1)
            {
                if (isIndex) SendToLists(aIndexes);
                else if (isNumber) SendToLists(numberList);
                else if (isVariable) SendToLists(variableList);                
            }
            currentComponent += c;

            Debug.Log(currentComponent);
        }       
        if (currentComponent != "") numberList.Add(currentComponent);
        if (currentComponent != "") variableList.Add(currentComponent);

    }
    public void Calculate(List<string> numbers, List<string> output)
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
