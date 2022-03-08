using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RemarkableIdentities : MonoBehaviour
{
    public GameObject currentTextSelcted;

    string aInputName;
    string bInputName;

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
        aInputName = GameObject.Find("Tab 3 - Remarkable Identities/Tabs/").GetComponent<TabGroup>().currentPageOpen.name + "/aInput";
        bInputName = GameObject.Find("Tab 3 - Remarkable Identities/Tabs/").GetComponent<TabGroup>().currentPageOpen.name + "/bInput";
        aInputField = GameObject.Find(aInputName).GetComponent<TMP_InputField>().text;
        bInputField = GameObject.Find(bInputName).GetComponent<TMP_InputField>().text;        
    }   
    public void Calculate()
    {
        GetInputs();
        SortData(aInputField, aNumbers, aVariables);
        SortData(bInputField, bNumbers, bVariables);        
    }
    public void SortData(string input, List<string> numberList, List<string> variableList)
    {      
        numberList.Clear();
        variableList.Clear();

        //Send the inputs to their respectable lists
        int idxTracker = 0;
        string currentNumber = "";
        string currentLetter = "";
        /*string aOrBOrC = "";
        if (input == aInputField) aOrBOrC = "a";
        else if (input == bInputField) aOrBOrC = "b";*/

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
                    isVariable = false;
                }
            }
            else if (((int)c <= 90 && (int)c >= 65) || ((int)c <= 122 && (int)c >= 97) && idxTracker < 2 && (int)c != 94)
            {
                if (isNumber)
                {
                    if (currentNumber != "") numberList.Add(currentNumber);
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
        /*foreach (string s in numberList)
        {
            Debug.Log("Index " + numberList.IndexOf(s) + "of" + aOrBOrC + "numbers: " + s);
        }
        foreach (string s in variableList)
        {
            Debug.Log("Index " + variableList.IndexOf(s) + "of" + aOrBOrC + "variables: " + s);
        }*/

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
