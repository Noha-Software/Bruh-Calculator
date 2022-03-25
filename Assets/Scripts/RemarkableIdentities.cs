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
    List<int> aIndexes;
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
        aIndexes = new List<int>();
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
        Calculate(aNumbers, aNumbersOutput);
        listIndexIdx = 0;
    }
    public void SortData(string input, List<string> numberList, List<string> variableList)
    {      
        numberList.Clear();
        variableList.Clear();

        int idxTracker = 0;
        string currentNumber = "";
        string currentLetter = "";
        

        foreach (char c in input)
        {
            if ((int)c <= 57 && (int)c >= 48 && idxTracker < 2)
            {
                if (isVariable == false)
                {
                    isNumber = true;
                    currentNumber += c;
                }
                else if (isVariable && idxTracker > 0)
                {
                    currentLetter += c;
                }
                else
                {
                    currentNumber += c;
                    if (currentLetter != "") variableList.Add(currentLetter);
                    currentLetter = "";
                    isIndex = false;
                    isVariable = false;
                    Debug.Log(listIndexIdx);
                    listIndexIdx++;
                }
            }
            else if (((int)c <= 90 && (int)c >= 65) || ((int)c <= 122 && (int)c >= 97) && idxTracker < 2 && (int)c != 94)
            {
                if (isNumber)
                {
                    if (currentNumber != "") numberList.Add(currentNumber);
                    currentNumber = "";
                    idxTracker = 0;
                    isIndex = false;
                    isNumber = false;
                    Debug.Log(listIndexIdx);
                    listIndexIdx++;
                }
                isVariable = true;
                currentLetter += c;

            }
            else if ((int)c == 94)
            {
                isIndex = true;
                if (isNumber == true)
                {
                    currentNumber += c;
                }
                else
                {
                    currentLetter += c;
                }
                ++idxTracker;
            }
            if (idxTracker > 1)
            {
                if (currentNumber != "") numberList.Add(currentNumber);
                if (currentLetter != "") variableList.Add(currentLetter);

                isNumber = false;
                isVariable = false;

                currentNumber = "";
                currentLetter = "";
                idxTracker = 0;
                Debug.Log(listIndexIdx);
                listIndexIdx++;
            }
        }
        if (currentNumber != "") numberList.Add(currentNumber);
        if (currentLetter != "") variableList.Add(currentLetter);

    }
    public void Calculate(List<string> numbers, List<string> output)
    {
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
    public void SendToLists(string currentComponent, List<string> list, bool boolean, int idxTracker)
    {
        if (currentComponent != "") list.Add(currentComponent);
        boolean = false;
        idxTracker = 0;
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
