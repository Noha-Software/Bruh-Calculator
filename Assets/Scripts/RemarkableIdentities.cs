using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RemarkableIdentities : MonoBehaviour
{
    bool isNumberOfA;
    bool isVariableOfA;
    bool isNumberOfB;
    bool isVariableOfB;

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
    }
    public void GetInputs()
    {
        //Get the inputs as strings
        string aInputName = GameObject.Find("Tab 3 - Remarkable Identities/Tabs").GetComponent<TabGroup>().currentPageOpen.name + "/aInput";
        string bInputName = GameObject.Find("Tab 3 - Remarkable Identities/Tabs").GetComponent<TabGroup>().currentPageOpen.name + "/bInput";
        aInputField = GameObject.Find(aInputName).GetComponent<TMP_InputField>().text;
        bInputField = GameObject.Find(bInputName).GetComponent<TMP_InputField>().text;
        //Debug.Log(aInputField + bINputField);
    }
    public void SortData()
    {
        GetInputs();
        //Send the inputs to their respectable lists
        int idxTracker = 0;
        string currentNumbers = "";
        string currentLetters = "";

        foreach(char c in aInputField)
        {
            if((int)c <= 57 && (int)c >= 48 && idxTracker < 2)
            {
                if (isVariableOfB == false)
                {
                    isNumberOfA = true;
                    currentNumbers += c;
                }
                else currentLetters += c;
                //Debug.Log(idxTracker);
            }
            else if((int)c <= 90 && (int)c >= 65 && (int)c <= 122 && (int)c >= 97 && idxTracker < 2 && (int)c != 94)
            {
                isVariableOfB = true;
                currentLetters += c;
            }
            else if ((int)c == 94)
            {
                if (isNumberOfA == true)
                    currentNumbers += c;
                else currentLetters += c;
                ++idxTracker;
                //Debug.Log(idxTracker);
            }
            if (idxTracker > 1)
            {
                aNumbers.Add(currentNumbers);
                aVariables.Add(currentLetters);
                //bNumbers.Add(currentLetters);                
                foreach (string s in aNumbers)
                {
                    Debug.Log("Index " + aNumbers.IndexOf(s) + "of aNumbers : " + s);
                }
                foreach(string s in aVariables)
                {
                    Debug.Log("Index " + aVariables.IndexOf(s) + "of aVariables : " + s);
                }
                isNumberOfA = false;
                isVariableOfA = false;
                isNumberOfB = false;
                isVariableOfB = false;
                currentNumbers = "";
                currentLetters = "";
                idxTracker = 0;
            }                                                     
        }
    }

    public void ClosePage()
    {
        GameObject currentPageOpen = GameObject.Find("Tab 3 - Remarkable Identities/Tabs").GetComponent<TabGroup>().currentPageOpen;
        currentPageOpen.SetActive(false);
    }
}
