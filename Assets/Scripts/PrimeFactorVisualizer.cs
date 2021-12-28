using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrimeFactorVisualizer : MonoBehaviour
{
	string factorsResult;
	string divisorsResult;

	public Button closeButton;
	public TMP_Text factorsText;
	public TMP_Text divisorsText;

    public void Enable()
	{
		gameObject.SetActive(true);
	}

	public void Disable()
	{
		gameObject.SetActive(false);
		ResetVisualizer();
	}

	public void Visualize(int[][] factorization)
	{
		ResetVisualizer();

		factorsResult = string.Join("\n", factorization[0]);
		divisorsResult = string.Join("\n", factorization[1]);

		factorsText.text = factorsResult;
		divisorsText.text = divisorsResult;

		Enable();
	}

	public void ResetVisualizer()
	{
		factorsText.text = "";
		divisorsText.text = "";
	}
}
