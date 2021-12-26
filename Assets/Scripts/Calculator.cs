using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Calculator : MonoBehaviour
{
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

	[Header("System Converter Attributes")]
	public bool systemConvertEnabled = false;
	[Range(1,36)]
	public int systemConvertOriginSystem;
	[Range(1,36)]
	public int systemConvertTargetSystem;
	public string systemConvertInputNumber;

	public Button systemConvertButton;
	public TMP_Text systemConvertOutputText;
	public TMP_InputField systemConvertOriginSystemField;
	public TMP_InputField systemConvertInputNumberField;
	public TMP_InputField systemConvertTargetSystemField;


	[Header("Prime Factorizator Attributes")]
	public bool primeFactorEnabled = false;
	public bool primeFactorVisualizerEnabled = false;
	[Min(1)]
	public int factorInputNumber;

	public PrimeFactorVisualizer primeFactorVisualizer;

	public Button factorizeButton;
	public TMP_Text factorOutputText;
	public TMP_InputField factorInputNumberField;

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
			try
			{
				int.TryParse(factorInputNumberField.text, out factorInputNumber);
			}
			catch (System.Exception)
			{
				Debug.LogWarning("Error parsing data from primeFactor input fields");
				throw;
			}
		}

		primeFactorVisualizerEnabled = primeFactorVisualizer.gameObject.activeSelf;
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
}