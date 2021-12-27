using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class PrimeFactorizator
{
	public static string Factorize(int n)
	{
		List<string> result = new List<string>();

		for (int i = 2; i < n; i++)
		{
			if ((n % i) == 0)
			{
				int count = 0;
				while ((n % i) == 0)
				{
					n = n / i;
					count++;
				}

				if (count > 1)
				{
					result.Add(string.Join("", i, "<sup>", count, "</sup>"));
				}
				else
				{
					result.Add(i.ToString());
				}
			}
		}

		if (n != 1)
		{
			result.Add(n.ToString());
		}

		return string.Join(" x ", result);
	}

	public static int[][] Visualize(int n)
	{
		List<int> factors = new List<int>();
		List<int> divisors = new List<int>();
		// List<string> result = new List<string>();

		factors.Add(n);
		for (int i = 2; i < n; i++)
		{
			if ((n % i) == 0)
			{
				// int count = 0;
				while ((n % i) == 0)
				{
					n = n / i;
					factors.Add(n);
					divisors.Add(i);
					//count++;
				}

				/*
				if (count > 1)
				{
					result.Add(string.Join("", i, "<sup>", count, "</sup>"));
				}
				else
				{
					result.Add(i.ToString());
				}
				*/
			}
		}

		if (n != 1)
		{
			factors.Add(1);
			divisors.Add(n);
			// result.Add(n.ToString());
		}

		int[][] result = new int[2][];
		result[0] = factors.ToArray();
		result[1] = divisors.ToArray();

		return result;
	}

	/*
	private static string[] FindDivisors(int n)
	{
		List<string> divisors = new List<string>();

		for (int i = 2; i < n; i++)
		{
			if ((n % i) == 0)
			{
				int count = 0;
				while ((n % i) == 0)
				{
					n = n / i;
					count++;
				}

				if (count > 1)
				{
					divisors.Add(string.Join("^", i, count));
				}
				else
				{
					divisors.Add(string.Join("^", i, 1));
				}
			}
		}

		if (n != 1)
		{
			divisors.Add(string.Join("^", n, 1));
		}

		return divisors.ToArray();
	}
	*/

	public static int FindGCD(params int[] numbers)
	{
		/*
		string[][] divisors = new string[numbers.Length][];
		List<int> appearingFactors = new List<int>();
		int result = 0;

		for (int i = 0; i < numbers.Length; i++)
		{
			divisors[i] = FindDivisors(numbers[i]);
		}

		int currentFactor;
		foreach (string[] array in divisors)
		{
			for (int i = 0; i < array.Length; i++)
			{
				int.TryParse(array[i].Split("^".ToCharArray())[0], out currentFactor);
				if (!appearingFactors.Contains(currentFactor))
				{
					appearingFactors.Add(currentFactor);
				}
			}
		} */

		if (numbers.Length <= 0)
		{
			Debug.LogWarning("Parameters out of range");
			return 0;
		}
		else if (numbers.Length == 1)
		{
			return numbers[0];
		}
		else if (numbers.Length == 2)
		{
			return (int)BigInteger.GreatestCommonDivisor(numbers[0], numbers[1]);
		}
		else
		{
			BigInteger prevGCD;
			foreach (int n in numbers)
			{
				if (prevGCD == null)
				{
					prevGCD = n;
				}
				else
				{
					prevGCD = BigInteger.GreatestCommonDivisor(prevGCD, n);
				}
			}

			return (int)prevGCD;
		}		
	}

	public static int FindLCM(params int[] numbers)
	{
		int gcd = FindGCD(numbers);
		int lcm;

		if (numbers.Length <= 0)
		{
			Debug.LogWarning("Parameters out of range");
			return 0;
		}
		else if (numbers.Length == 1)
		{
			return numbers[0];
		}
		else if (numbers.Length == 2)
		{
			lcm = Mathf.Abs(numbers[0] * numbers[1]) / gcd;
		}
		else
		{
			int multiple = 1;
			foreach (int n in numbers)
			{
				multiple *= n;
			}

			lcm = Mathf.Abs(multiple) / gcd;
		}

		return lcm;
	}
}