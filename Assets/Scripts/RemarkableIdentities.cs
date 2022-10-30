using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RemarkableIdentities : MonoBehaviour
{
	public GameObject currentTextSelected;

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

	void Start()
	{
		aList = new List<string>();
		bList = new List<string>();
		output = new StringBuilder(50);
		currentComponent = new StringBuilder();
		calculations = new StringBuilder();
		pascal = new List<int>();
	}
	/*
	void GetInputs()
	{
		xInput = transform.GetChild(2).GetComponent<TMP_InputField>();        
		aInput = transform.GetChild(0).GetComponent<TMP_InputField>();
		bInput = transform.GetChild(1).GetComponent<TMP_InputField>();
		outputText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
	}
	*/
	public void Output()
	{
		//GetInputs();
		if (aInput.text == "" || bInput.text == "" || xInput.text == "")
		{
			outputText.text = "ERROR: All three fields must have data in them to proccess them";
			return;
		}
		if (int.TryParse(xInput.text, out int res))
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
		if (two != "")
		{
			outputText.text = two;
			ResetAll();
			return;
		}

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
			Calculate(((x - 1) - i), i, pascal[i]);
		}
		outputText.text = SimplifyExpression(output.ToString());
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
				if (isIndex) return "ERROR: Use of variable as an exponent is forbidden"; //Hibaüzenet, ha változó kerül egy kitevõbe               
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
		return ""; //Ne adjon hibaüzenetet helyes szintaktika használatakor
	}
	void FuckMe(int idx, List<string> list) //???
	{
		if (idx != 0) foreach (string s in list)
			{
				if (long.TryParse(s, out long res))
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
			//Debug.Log(calculations.ToString());
			//Debug.Log(number + (calculations.ToString().PadLeft(calculations.Length + number.ToString().Length)).Trim());
			//Debug.Log(Int64.TryParse(calculations.ToString(), out long l));
			if (!(calculations.ToString() == "")) calculations.Replace(calculations.ToString(), number + (calculations.ToString().PadLeft(calculations.Length + number.ToString().Length)).Trim());
			else
			{
				//Debug.Log("GHECI");
				calculations.Append((number + (calculations.ToString().PadLeft(calculations.Length + number.ToString().Length)).Trim()));
			}
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
		TMP_InputField field = currentTextSelected.GetComponent<TMP_InputField>();
		int caret = field.caretPosition;
		field.text = field.text.Substring(0, caret) + "^" + field.text.Substring(caret);
		field.Select();
		field.caretPosition = caret + 1;
	}
	public void AddDoubleUpperIndex()
	{
		TMP_InputField field = currentTextSelected.GetComponent<TMP_InputField>();
		int caret = field.caretPosition;
		field.text = field.text.Substring(0, caret) + "^^" + field.text.Substring(caret);
		field.Select();
		field.caretPosition = caret + 1;
	}
	public void SetThisSelectedTab(GameObject input)
	{
		currentTextSelected = input;
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

	string SimplifyExpression(string expression)
	{
		expression = expression.ToLower();
		Regex pattern = new Regex(@"(\-)|(\+)");
		string[] members = pattern.Split(expression);

		List<Member> outputMembers = new List<Member>();

		bool negative = false;
		for (int i = 0; i < members.Length; i++)
		{
			if (members[i] == "+" || members[i] == "-")
			{
				if (members[i] == "-")
				{
					negative = true;
				}
				else
				{
					negative = false;
				}
				continue;   // ignore if operator
			}

			Regex exponentPattern = new Regex(@"<.{3,4}>");
			members[i] = exponentPattern.Replace(members[i], "");   // delete superscript (<sup></sup>)

			Member member = new Member(members[i]);

			if (negative)
			{
				member.coefficient *= -1;
			}

			outputMembers.Add(member);
			Debug.Log(member.OutputText());
		}

		List<string> outputs = new List<string>();
		for (int i = 0; i < outputMembers.Count; i++)
		{
			if (outputMembers[i].coefficient > 0 && i != 0)
			{
				outputs.Add("+");
			}
			outputs.Add(outputMembers[i].OutputText());
		}

		return string.Join("", outputs);
	}

	internal struct Member
	{
		public int coefficient;
		public char[] variables;

		public override string ToString()
		{
			return coefficient + new string(variables.ToArray());
		}

		public Member(int coefficient, char[] variables)
		{
			this.coefficient = coefficient;
			this.variables = variables;
		}
		public Member(string formattedExpression)
		{
			coefficient = 0;
			List<char> variableList = new List<char>();
			int lastLetter = -1;
			for (int j = 0; j < formattedExpression.Length; j++)
			{
				if (char.IsDigit(formattedExpression[j]))
				{
					if (coefficient != 0)
					{
						if (formattedExpression.Length > j + 1)
						{
							if (char.IsDigit(formattedExpression[j + 1]))
							{
								continue;
							}
							else
							{
								int exponent = int.Parse(formattedExpression.Substring(lastLetter + 1, j - lastLetter));
								for (int k = 0; k < exponent - 1; k++)
								{
									variableList.Add(formattedExpression[lastLetter]);
								}
							}
						}
						else
						{
							int exponent = int.Parse(formattedExpression.Substring(lastLetter + 1, j - lastLetter));
							for (int k = 0; k < exponent - 1; k++)
							{
								variableList.Add(formattedExpression[lastLetter]);
							}
						}
					}
				}
				else
				{
					if (j == 0)
					{
						coefficient = 1;    // coefficient is 1 if not specified
					}
					else if (coefficient == 0)
					{
						coefficient = int.Parse(formattedExpression.Substring(0, j));
					}
					variableList.Add(formattedExpression[j]);
					lastLetter = j;
				}
			}
			variableList.Sort();
			variables = variableList.ToArray();
		}

		public string OutputText()
		{
			List<string> output = new List<string>();
			output.Add(coefficient.ToString());

			char[] uniqueVariables = variables.Distinct().ToArray();
			foreach (var variable in uniqueVariables)
			{
				output.Add(variable.ToString());

				int exponent = Array.FindAll(variables, x => x == variable).Length;
				if (exponent > 1)
				{
					output.Add("<sup>" + exponent + "</sup>");
				}
			}

			return string.Join("", output);
		}

		public static Member operator +(Member a, Member b)
		{
			if (a.variables != b.variables)
			{
				Debug.LogError("Can't simplify members that don't share variables");
				return new Member();
			}
			else
			{
				return new Member(a.coefficient + b.coefficient, a.variables);
			}
		}
		public static Member operator -(Member a, Member b)
		{
			if (a.variables != b.variables)
			{
				Debug.LogError("Can't simplify members that don't share variables");
				return new Member();
			}
			else
			{
				return new Member(a.coefficient - b.coefficient, a.variables);
			}
		}
	}
}