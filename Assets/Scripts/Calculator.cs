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

	TMP_InputField expansionCheckInput;
	bool isDecimal;
	bool isScientificallyNotated;
	bool stopSoros;

	public void ParsebleCheck(GameObject thisObject)
    {
		expansionCheckInput = thisObject.GetComponent<TMP_InputField>();

		if (!stopSoros)
		{
			switch (expansionCheckInput.text[expansionCheckInput.caretPosition - 1])
			{
				case ',':
					if (isDecimal)
					{
						stopSoros = true;
						expansionCheckInput.text = expansionCheckInput.text.Substring(0, expansionCheckInput.caretPosition - 1) + expansionCheckInput.text.Substring(expansionCheckInput.caretPosition);
					}
					break;
				case '.':
					expansionCheckInput.text = expansionCheckInput.text.Substring(0, expansionCheckInput.caretPosition - 1) + "," + expansionCheckInput.text.Substring(expansionCheckInput.caretPosition);
					break;
				/*case 'x':
					if (!isScientificallyNotated) expansionCheckInput.text += "10<sup>";
					else
					{
						stopSoros = true;
						expansionCheckInput.text = expansionCheckInput.text.Substring(0, expansionCheckInput.caretPosition - 1) + expansionCheckInput.text.Substring(expansionCheckInput.caretPosition);
					}
					break;*/
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
				default:
					expansionCheckInput.text = expansionCheckInput.text.Remove(expansionCheckInput.text.Length - 1);
					break;
			}

			if (expansionCheckInput.text.Contains("x")) isScientificallyNotated = true;
			else isScientificallyNotated = false;


			if (expansionCheckInput.text.Contains(",")) isDecimal = true;
			else isDecimal = false;

			Debug.Log(expansionCheckInput.text[expansionCheckInput.caretPosition - 1]);
			Debug.Log(isDecimal);
			Debug.Log(isScientificallyNotated);
		}
		else
        {
			stopSoros = false;
        }
	}
    #endregion
}