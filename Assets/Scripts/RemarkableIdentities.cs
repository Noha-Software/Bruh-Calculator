using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Text;
using UnityEngine.UI;

public class RemarkableIdentities : MonoBehaviour
{
    public GameObject currentTextSelcted;
    public GameObject tabs;

    int idxTracker = 0;

    bool isVariable;
    bool isIndex;
    bool deduction;
    bool hasNumber;

    int x;
    long number;
    char opSym;

    StringBuilder output;
    StringBuilder currentComponent;
    StringBuilder calculations;

    public TMP_InputField aInput;
    public TMP_InputField bInput;
    public TMP_InputField xInput;
    public TextMeshProUGUI outputText;

    List<string> aList;
    List<string> bList;
    List<int> pascal;
    GameObject currentPageSelected;

    void Start()
    {
        aList = new List<string>();
        bList = new List<string>();
        output = new StringBuilder(50);
        currentComponent = new StringBuilder();
        calculations = new StringBuilder();
        pascal = new List<int>();
    }
    void GetInputs()
    {
        currentPageSelected = tabs.GetComponent<TabGroup>().currentPageOpen;
        xInput = GameObject.Find(currentPageSelected.name + "indexInput").GetComponent<TMP_InputField>();        
        aInput = GameObject.Find(currentPageSelected.name + "/aInput").GetComponent<TMP_InputField>();
        bInput = GameObject.Find(currentPageSelected.name + "/bInput").GetComponent<TMP_InputField>();
        outputText = GameObject.Find(currentPageSelected.name + "/output/outputText").GetComponent<TextMeshProUGUI>();
    }
    public void Output()
    {
        GetInputs();
        if (aInput.text == "" || bInput.text == "" || xInput.text == "")
        {
            outputText.text = "ERROR: All three fields must have data in them to proccess them";
            return;
        }
        if(Int32.TryParse((GameObject.Find(currentPageSelected.name + "indexInput").GetComponent<TMP_InputField>().text), out int res))
        {
            x = res + 1;
        }
        else
        {
            outputText.text = "ERROR: Only intergers can be typed as the exponent";
            return;
        }
        //x = Int32.Parse(GameObject.Find(currentPageSelected.name + "indexInput").GetComponent<TMP_InputField>().text) + 1;
        string one = SortData(aInput.text, aList);
        string two = SortData(bInput.text, bList);
        if (one != "")
        {
            outputText.text = one;
            ResetAll();
            return;
        }
        if( two != "")
        {
            outputText.text = two;
            ResetAll();
            return;
        }

        //Pascal h�romsz�g
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
            Calculate(((x-1) - i), i, pascal[i]);            
        }
        outputText.text = output.ToString();
        pascal.Clear();
        output.Clear();       
    }
    string SortData(string input, List<string> list)
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
                if (isIndex) return "ERROR: Use of variable as an exponent is forbidden"; //Hiba�zenet, ha v�ltoz� ker�l egy kitev�be               
                SendToList(list);
                isVariable = true;
                currentComponent.Append(c);
            }
            else if ((int)c == 94)
            {
                isIndex = true;
                idxTracker++;
            }
            else
            {
                return "ERROR: An invalid character was used. Valid characters include:\nCapital and non-capital letters of the english alphabet\nNumerics from 0 to 9\n'^' to mark exponents";
            }
            if (idxTracker > 1)
            {
                isIndex = false;
                SendToList(list);
                idxTracker = 0;
            }
        }
        if (isIndex) return "ERROR: The final exponent has been not sealed properly (only one '^' used instead of two)";
        SendToList(list);
        return ""; //Ne adjon hiba�zenetet helyes szintaktika haszn�latakor
    }
    void FuckMe(int idx, List<string> list)
    {
        if (idx != 0) foreach (string s in list)
        {

            if (Int64.TryParse(s, out long res))
            {
                hasNumber = true;
                int n = (int)res;
                for (int i = 0; i < (idx - 1); ++i) if (idx != 1)
                {
                    res *= n;
                }
                number *= res;
            }
            else
            {
                if (s.Length == 1)
                {
                    calculations.Append(s);
                    if (idx != 1) calculations.Append("<sup>" + idx + "</sup>");
                }
                else
                {
                    long v = Int64.Parse(s.Trim(s[0]));
                    calculations.Append(s[0] + "<sup>" + (v * idx) + "</sup>");
                }
            }
        }
    }
    void Calculate(int a, int b, int pascalNum)
    {
        hasNumber = false;
        number = 1;
        calculations.Clear();
        opSym = '+';

        FuckMe(a, aList);
        FuckMe(b, bList);
        if (hasNumber)
        {
            number *= pascalNum;
            calculations.Replace(calculations.ToString(), number + (calculations.ToString().PadLeft(calculations.Length + number.ToString().Length)).Trim());
        }
        else
        {
            if (pascalNum != 1) calculations.Replace(calculations.ToString(), pascalNum + (calculations.ToString().PadLeft(calculations.Length + pascalNum.ToString().Length)).Trim());
        }
        output.Append(calculations.ToString());
        if (deduction && b % 2 == 0) opSym = '-';
        if (a != 0) output.Append(opSym);
    }
    void SendToList(List<string> list)
    {
        if (currentComponent.ToString() != "") list.Add(currentComponent.ToString());
        isVariable = false;
        currentComponent.Clear();
    }
    public void ResetAll()
    {
        idxTracker = 0;
        isVariable = false;
        isIndex = false;
        aList.Clear();
        bList.Clear();
    }
    public void ClosePage()
    {
        GameObject currentPageOpen = GameObject.Find("Tab 3 - Remarkable Identities/Tabs").GetComponent<TabGroup>().currentPageOpen;
        currentPageOpen.SetActive(false);
    }
    public void AddUpperIndex()
    {
        TMP_InputField field = currentTextSelcted.GetComponent<TMP_InputField>();
        int caret = field.caretPosition;
        field.text = field.text.Substring(0, caret) + "^" + field.text.Substring(caret);
    }
    public void AddDoubleUpperIndex()
    {
        TMP_InputField field = currentTextSelcted.GetComponent<TMP_InputField>();
        int caret = field.caretPosition;
        field.text = field.text.Substring(0, caret) + "^^" + field.text.Substring(caret);
    }
    public void SetThisSelectedTab(GameObject input)
    {
        currentTextSelcted = input;
    }
    public void OperatingSymbol(GameObject text)
    {
        if (deduction)
        {
            deduction = false;
            text.GetComponent<Text>().text = "+";
        }
        else 
        { 
            deduction = true;
            text.GetComponent<Text>().text = "_";
        }
    }
}
