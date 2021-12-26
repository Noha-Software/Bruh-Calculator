using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemConverter
{
	public static string SystemConvert(int inSys, int outSys, string inString)
	{
		if ((outSys < 1 || inSys < 1) || (outSys > 36 || inSys > 36)) //check if systems are out of range
		{
			Debug.LogWarning("Conversion parameter out of range");
			return "NaN";
		}

		List<int> numbers = new List<int>();

		/*
		char[] inChars = inString.ToCharArray();
		int currentCharacter;
		for (int i = 0; i < inChars.Length; i++)
		{
			currentCharacter = inChars[i] - '0';
			if (currentCharacter > 39 && (currentCharacter - 39) < inSys) //if letter
			{
				numbers.Add(currentCharacter - 39);
			}
			else if (currentCharacter < 39 && currentCharacter >= 0 && (currentCharacter - 39) < inSys) //if number
			{
				numbers.Add(currentCharacter);
			}
			else
			{
				numbers.Add(0);
				Debug.Log("Error adding " + inChars[i] + " at #" + i + " to the number");
			}
		}
		*/

		int[] asd = ConvertFromChars(inString);
		numbers.AddRange(asd);

		/*
		if (inSys != 1)
		{
			foreach (var number in numbers.ToArray())
			{
				if (CheckCharacter(inSys, number))
				{
					numbers.Remove(number);
				}
			}
		}
		*/
		
		if (inSys == outSys)													// if systems are the same
		{
			return ArrayToString(numbers.ToArray());							// pass input
		}
		else if (outSys == 1)													// if output is 1
		{
			int num10 = ArrayToInt(ConvertTo10(inSys, numbers.ToArray()));		// convert to 10
			List<int> digits = new List<int>();
			for (int i = 0; i < num10; i++)
			{
				digits.Add(1);													// add 1s to the number according to num10
			}
			return ArrayToInt(digits.ToArray()).ToString();
		}
		else if (inSys == 1)													// if input is 1
		{
			int num10 = numbers.ToArray().Length;
			return ConvertFrom10(outSys, DigitsIn(num10));						// return the number of 1s in the output system
		}
		else if (outSys == 10)													// if output is 10
		{
			return ArrayToString(ConvertTo10(inSys, numbers.ToArray()));		// convert to 10
		}
		else if (inSys == 10)													// if input is 10
		{
			return ConvertFrom10(outSys, numbers.ToArray());					// convert from 10
		}
		else
		{
			int[] num10 = ConvertTo10(inSys, numbers.ToArray());				// convert to 10
			return ConvertFrom10(outSys, num10);								// convert from 10
		}
	}

	public static int[] ConvertTo10(int inSys, int[] digits)
	{
		Debug.Log("Converting " + ArrayToInt(digits) + " from " + inSys + " to 10");

		int newNum = 0;
		for (int i = 0; i < digits.Length; i++)
		{
			int newDigit = digits[digits.Length - 1 - i] * (int)Mathf.Pow(inSys, i);
			newNum += newDigit;
			Debug.Log("no" + i + " newDigit: " + newDigit);
		}

		//Debug.Log("newNum:" + newNum);
		return DigitsIn(newNum);
	}

	public static string ConvertFrom10(int outSys, int[] numbers)
	{
		Debug.Log("Converting " + ArrayToInt(numbers) + " from 10 to " + outSys);
		int newNum = ArrayToInt(numbers);
		List<int> digits = new List<int>();

		for (int i = newNum; i > 0; i = Mathf.FloorToInt(i / outSys))
		{
			int oldNum = newNum;
			newNum = Mathf.FloorToInt(oldNum / outSys);
			int rest = oldNum - (newNum * outSys);
			digits.Add(rest);
			Debug.Log(i + " | " + rest);
		}

		int[] digitsArray = digits.ToArray();
		System.Array.Reverse(digitsArray);
		return ConvertToChars(digitsArray);
	}

	public static bool CheckCharacter(int sys, int number)
	{
		Debug.Log("Checking " + number + " if compatible with " + sys);
		if (number > 39 && (number - 39) < sys) //if letter
		{
			if (number >= sys)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		else if (number < 39 && number >= 0 && (number - 39) < sys) //if number
		{
			if (number >= sys || number < 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return true;
		}
	}

	public static int[] DigitsIn(int value)
	{
		var numbers = new Stack<int>();

		for (; value > 0; value /= 10)
			numbers.Push(value % 10);

		return numbers.ToArray();
	}

	public static int ArrayToInt(int[] array)
	{
		int finalScore = 0;
		for (int i = 0; i < array.Length; i++)
		{
			finalScore += array[i] * (int)Mathf.Pow(10, array.Length - i - 1);
		}
		return finalScore;
	}

	public static string ArrayToString(int[] array)
	{
		string finalScore = "";
		for (int i = 0; i < array.Length; i++)
		{
			finalScore += array[i];
		}
		return finalScore;
	}

	public static string ConvertToChars(int[] array)
	{
		string[] digits = new string[array.Length];
		int currentDigit;
		char currentChar;
		for (int i = 0; i < array.Length; i++)
		{
			currentDigit = array[i];
			if (currentDigit >= 0 && currentDigit <= 9) //if number
			{
				digits[i] = currentDigit.ToString();
				//Debug.Log("no" + i + " digit: " + currentDigit);
			}
			else if (currentDigit > 9)
			{
				currentDigit += 87;
				//Debug.Log("no" + i + " digit: " + currentDigit);
				currentChar = (char)currentDigit;
				//Debug.Log("no" + i + " char: " + currentChar);
				digits[i] = currentChar.ToString();
			}
			else
			{
				digits[i] = "?";
			}
		}

		return string.Join("", digits);
	}

	public static int[] ConvertFromChars(string s)
	{
		char[] charArray = s.ToCharArray();
		List<int> newDigits = new List<int>();

		int currentCharacter;

		for (int i = 0; i < charArray.Length; i++)
		{
			currentCharacter = charArray[i] - '0';
			if (currentCharacter >= 49) //if letter
			{
				newDigits.Add(currentCharacter - 39);
			}
			else if (currentCharacter <= 9 && currentCharacter >= 0) //if number
			{
				newDigits.Add(currentCharacter);
			}
			else
			{
				Debug.LogWarning("Error adding " + charArray[i] + " at #" + i + " to the array");
			}
		}

		return newDigits.ToArray();
	}
}