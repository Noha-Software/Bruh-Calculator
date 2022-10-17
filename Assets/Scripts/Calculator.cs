using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Calculator : MonoBehaviour
{
	#region Field get-set
	public bool SystemConvertEnabled
	{
		get { return systemConvertEnabled; }
		set { systemConvertEnabled = value; }
	}
	public bool PrimeFactorEnabled
	{
		get { return primeFactorEnabled; }
		set { primeFactorEnabled = value; }
	}
	public bool PlainEnabled
	{
		get { return plainEnabled; }
		set { plainEnabled = value; }
	}
	public bool GCDEnabled
	{
		get { return gcdEnabled; }
		set { gcdEnabled = value; }
	}
	public bool LCMEnabled
	{
		get { return lcmEnabled; }
		set { lcmEnabled = value; }
	}
	#endregion

	#region Fields
	[Header("System Converter Attributes")]
	public bool systemConvertEnabled = false;
	[Range(1,36)]
	public int systemConvertOriginSystem;
	[Range(1,36)]
	public int systemConvertTargetSystem;
	public string systemConvertInputNumber;

	[Space]
	public Button systemConvertButton;
	public TMP_Text systemConvertOutputText;
	public TMP_InputField systemConvertOriginSystemField;
	public TMP_InputField systemConvertInputNumberField;
	public TMP_InputField systemConvertTargetSystemField;

	[Space]
	[Header("Prime Factorizator Attributes")]
	public bool primeFactorEnabled = false;
	public bool primeFactorVisualizerEnabled = false;
	public bool plainEnabled;
	public bool gcdEnabled;
	public bool lcmEnabled;
	
	[Space]
	[Min(1)] public int factorInputNumber;
	public int[] gcdInputNumbers;
	public int[] lcmInputNumbers;

	[Space]
	public PrimeFactorVisualizer primeFactorVisualizer;
	public Button factorizeButton;
	public TMP_Text factorOutputText;
	public TMP_InputField factorInputNumberField;

	[Space]
	public Button gcdButton;
	public TMP_Text gcdOutputText;
	public TMP_InputField[] gcdInputNumberFields;

	[Space]
	public Button lcmButton;
	public TMP_Text lcmOutputText;
	public TMP_InputField[] lcmInputNumberFields;

	[Space]
	[Header("Thermal expansion attributes")]
	public ThermalExpansion thermalExpansion;
	public TabGroup expansionTabgGroup;
	#endregion

	private void Start()
	{
		gcdInputNumbers = new int[gcdInputNumberFields.Length];
		lcmInputNumbers = new int[lcmInputNumberFields.Length];
		ExpansionConverter.Define();
	}

	private void Update()
	{
		if (systemConvertEnabled)
		{
			try
			{
				int.TryParse(systemConvertOriginSystemField.text, out systemConvertOriginSystem);
				systemConvertInputNumber = systemConvertInputNumberField.text;
				int.TryParse(systemConvertTargetSystemField.text, out systemConvertTargetSystem);
			}
			catch (System.Exception)
			{
				Debug.LogWarning("Error parsing data from systemConvert input fields");
				throw;
			}
		}

		if (primeFactorEnabled)
		{
			if (plainEnabled)
			{
				try
				{
					int.TryParse(factorInputNumberField.text, out factorInputNumber);
				}
				catch (System.Exception)
				{
					Debug.LogWarning("Error parsing data from primeFactor input fields");
					throw;
				}
				primeFactorVisualizerEnabled = primeFactorVisualizer.gameObject.activeSelf;
			}
			else if (gcdEnabled)
			{
				try
				{
					for (int i = 0; i < gcdInputNumberFields.Length; i++)
					{
						int.TryParse(gcdInputNumberFields[i].text, out gcdInputNumbers[i]);
					}
				}
				catch (System.Exception)
				{
					Debug.LogWarning("Error parsing data from GCD input fields");
					throw;
				}
			}
			else if (lcmEnabled)
			{
				try
				{
					for (int i = 0; i < lcmInputNumberFields.Length; i++)
					{
						int.TryParse(lcmInputNumberFields[i].text, out lcmInputNumbers[i]);
					}
				}
				catch (System.Exception)
				{
					Debug.LogWarning("Error parsing data from LCM input fields");
					throw;
				}
			}
		}
	}


	#region Prime Factorizator
	public void PrimeFactor()
	{
		if (primeFactorEnabled)
		{
			factorOutputText.text = PrimeFactorizator.Factorize(factorInputNumber);
		}
	}

	public void VisualizePrimeFactor()
	{
		if (primeFactorEnabled)
		{
			int[][] factorVisualizeResult = PrimeFactorizator.Visualize(factorInputNumber);
			if (factorVisualizeResult[0][0] != 0 && factorVisualizeResult[1][0] != 0)
			{
				if (primeFactorVisualizerEnabled)
				{
					primeFactorVisualizer.Disable();
					primeFactorVisualizer.Visualize(factorVisualizeResult);
				}
				primeFactorVisualizer.Visualize(factorVisualizeResult);
			}
		}
	}

	public void GreatestCommonDivisor()
	{
		if (gcdEnabled)
		{
			gcdOutputText.text = PrimeFactorizator.FindGCD(gcdInputNumbers).ToString();
		}
	}

	public void LeastCommonMultiple()
	{
		if (lcmEnabled)
		{
			lcmOutputText.text = PrimeFactorizator.FindLCM(lcmInputNumbers).ToString();
		}
	}
	#endregion

	#region System Converter
	public void ConvertToSystem()
	{
		if (systemConvertEnabled)
		{
			Debug.Log(systemConvertOriginSystem);
			Debug.Log(systemConvertTargetSystem);
			Debug.Log(systemConvertInputNumber);
			systemConvertOutputText.text = SystemConverter.SystemConvert(systemConvertOriginSystem, systemConvertTargetSystem, systemConvertInputNumber);
		}
	}

	public void OnInputValueChanged()
	{
		if (systemConvertEnabled)
		{
			CheckValue(systemConvertInputNumberField.text);
		}
	}

	void CheckValue(string text)
	{
		if(systemConvertOriginSystemField.text == "" || systemConvertOriginSystemField.text == null)
		{
			return;
		}

		int.TryParse(systemConvertOriginSystemField.text, out int inSys);
		int[] chars = SystemConverter.ConvertFromChars(text);
		List<int> newChars = new List<int>();
		bool wrong;

		for (int i = 0; i < chars.Length; i++)
		{
			if (SystemConverter.CheckCharacter(inSys, chars[i]))
			{
				wrong = true;
			}
			else
			{
				wrong = false;
			}

			if (!wrong)
			{
				newChars.Add(chars[i]);
			}
		}

		if (newChars.ToArray() == chars)
		{
			return;
		}
		else
		{
			systemConvertInputNumberField.text = SystemConverter.ConvertToChars(newChars.ToArray());
		}
	}
	#endregion

	#region Thermal Expansion

	TMP_InputField input;
	bool isDecimal;
	bool isScientificallyNotated;
	bool stopSoros;
	int decimalPos;
	int savedLength;

	public void ParsebleCheck(ExpansionConversionData data)
    {
		input = data.input;

		Debug.Log(input.text);

		if(isDecimal && (input.text.Contains(",") || input.text.Contains(",")))
        {			
			if (savedLength != input.text.Length && savedLength - 1 != input.text.Length && savedLength + 1 != input.text.Length)
            {
				if (Array.FindAll<char>(input.text.ToCharArray(), x => (x == ',' || x == '.')).Length > 1)
				{
					if (decimalPos > input.text.Length - 1) decimalPos -= savedLength - input.text.Length;
					else if (input.text[decimalPos] != ',' || input.text[decimalPos] != '.') decimalPos -= savedLength - input.text.Length;
				}
				else isDecimal = false;
            }
			if (decimalPos == 0)
			{
				if (savedLength - 1 == input.text.Length && input.text[decimalPos] != ',') isDecimal = false;
				else if (savedLength + 1 == input.text.Length && input.text[decimalPos] != ',') decimalPos++;
			}
			/*else if (decimalPos + 1 == input.text.Length)
            {
				if (savedLength - 1 == input.text.Length && input.text[decimalPos - 1] != ',') isDecimal = false;
				else if (savedLength + 1 == input.text.Length && input.text[decimalPos] != ',') decimalPos++;
			}*/
			//{ if (savedLength + 1 == input.text.Length && input.text[decimalPos] != ',') isDecimal = false; }
			else if (savedLength - 1 == input.text.Length && (input.text[decimalPos - 1] == ',' || input.text[decimalPos - 1] == '.')) decimalPos -= 1;
			else if (savedLength + 1 == input.text.Length && (input.text[decimalPos + 1] == ',' || input.text[decimalPos + 1] == '.')) decimalPos += 1;
		}		 
		if (!stopSoros && input.text != "")
		{
			Debug.Log("decimalPos at the start of function: " + decimalPos);
			for (int i1 = 0; i1 < input.text.Length; i1++)
			{
				switch (input.text[i1])
				{
					case ',':
						if (!isDecimal || decimalPos == i1)
						{
							decimalPos = i1;
							break;
						}
						stopSoros = true;
						Remove(decimalPos);
						if (i1 > decimalPos) decimalPos = i1 - 1;
						else decimalPos = i1;
						Debug.Log(decimalPos);
						break;
					case '.':
						input.text = input.text.Substring(0, i1) + "," + input.text.Substring(i1 + 1);
						break;
					/*case 'x':
						if (!isScientificallyNotated) expansionCheckInput.text += "10<sup>";
						else
						{
							stopSoros = true;
							expansionCheckInput.text = expansionCheckInput.text.Substring(0, expansionCheckInput.caretPosition - 1) + expansionCheckInput.text.Substring(expansionCheckInput.caretPosition);
						}
						break;*/ //IDE KELL A LEZÁRÁS
					case '0':
						break;
					case '1':
						break;
					case '2':
						break;
					case '3':
						break;
					case '4':
						break;
					case '5':
						break;
					case '6':
						break;
					case '7':
						break;
					case '8':
						break;
					case '9':
						break;
					case '-':
						if (data.measurementFamily == 2 && i1 == 0) break;
						else
						{
							Remove(i1);
						}
						break;
					default:
						Remove(i1);
						break;
				}
			}
			/*if (input.text.Contains("x")) isScientificallyNotated = true;
			else isScientificallyNotated = false*/ //IDE IS
			if (input.text.Contains(",")) isDecimal = true;
			else
			{
				isDecimal = false;
				decimalPos = 1;
			}
			savedLength = input.text.Length;

			Debug.Log("decimalPos at the end of function: " + decimalPos);
		}
		else stopSoros = false;
	}
    void Remove(int i)
    {
		if (i > 0 && i < input.text.Length - 1) input.text = input.text.Substring(0, i) + input.text.Substring(i + 1);
		else if (i <= 0) input.text = input.text.Substring(1);
		else input.text = input.text.Substring(0, input.text.Length - 1);
	}
    #endregion
}