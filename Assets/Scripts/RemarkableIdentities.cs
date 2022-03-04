using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RemarkableIdentities : MonoBehaviour
{
    bool isNumber;
    bool isVariable;
    
    public string aInputField;
    public string bInputField;

    List<string> aNumbers;
    List<string> aVariables;
    List<string> bNumbers;
    List<string> bVariables;

    private void Start()
    {
        aNumbers = new List<string>();
        aVariables = new List<string>();
        bNumbers = new List<string>();
        bVariables = new List<string>();
    }
    public void GetInputs()
    {
        //Get the inputs as strings
        string aInputName = GameObject.Find("Tab 3 - Remarkable Identities/Tabs").GetComponent<TabGroup>().currentPageOpen.name + "/aInput";
        string bInputName = GameObject.Find("Tab 3 - Remarkable Identities/Tabs").GetComponent<TabGroup>().currentPageOpen.name + "/bInput";
        aInputField = GameObject.Find(aInputName).GetComponent<TMP_InputField>().text;
        bInputField = GameObject.Find(bInputName).GetComponent<TMP_InputField>().text;        
    }   
    public void Calculate()
    {
        SortData(aInputField, aNumbers, aVariables);
        SortData(bInputField, bNumbers, bVariables);        
    }
    public void SortData(string input, List<string> numberList, List<string> variableList)
    {
        GetInputs();
        numberList.Clear();
        variableList.Clear();

        //Send the inputs to their respectable lists
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
                    variableList.Add(currentLetter);
                    currentLetter = "";
                    isVariable = false;
                }
            }
            else if (((int)c <= 90 && (int)c >= 65) || ((int)c <= 122 && (int)c >= 97) && idxTracker < 2 && (int)c != 94)
            {
                if (isNumber)
                {
                    numberList.Add(currentNumber);
                    currentNumber = "";
                    idxTracker = 0;
                    isNumber = false;
                }
                isVariable = true;
                currentLetter += c;

            }
            else if ((int)c == 94)
            {
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
            }
        }
        if (currentNumber != "") numberList.Add(currentNumber);
        if (currentLetter != "") variableList.Add(currentLetter);
        foreach (string s in numberList)
        {
            Debug.Log("Index " + aNumbers.IndexOf(s) + "of aNumbers : " + s);
        }
        foreach (string s in variableList)
        {
            Debug.Log("Index " + aVariables.IndexOf(s) + "of aVariables : " + s);
        }

    }
    public void ClosePage()
    {
        GameObject currentPageOpen = GameObject.Find("Tab 3 - Remarkable Identities/Tabs").GetComponent<TabGroup>().currentPageOpen;
        currentPageOpen.SetActive(false);
    }
}
