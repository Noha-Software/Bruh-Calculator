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
        //Get the numbers of 'a'
        int idxTracker = 0;
        string currentString = "";

        foreach(char c in aInputField)
        {
            if((int)c <= 57 && (int)c >= 48 && idxTracker < 2)
            {
                isNumberOfA = true;
                currentString += c;
                Debug.Log(idxTracker);
            }
            else if ((int)c == 94)
            {
                //debug needed
                currentString += c;
                ++idxTracker;
                Debug.Log(idxTracker);
            }
            if (idxTracker > 1)
            {
                aNumbers.Add(currentString);                
                Debug.Log("Is number of a? : " + isNumberOfA);
                Debug.Log("Is varuable of a? : " + isVariableOfA);
                Debug.Log("is number of b? : " + isNumberOfB);
                Debug.Log("Is variable of b? : " + isVariableOfB);
                foreach (string s in aNumbers)
                {
                    Debug.Log("Index" + aNumbers.IndexOf(s) + "of aNumbers : " + s);
                }
                currentString = "";
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
