using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}