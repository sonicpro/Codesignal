using System;
using System.Linq;
using System.Collections.Generic;

namespace FindLongestSubarrayBySum
{
	//You have an unsorted array arr of non-negative integers and a number s.
	//Find a longest contiguous subarray in arr that has a sum equal to s.
	//Return two integers that represent its inclusive bounds.
	//If there are several possible answers, return the one with the smallest left bound.
	//If there are no answers, return [-1].

	//Your answer should be 1-based, meaning that the first position of the array is 1 instead of 0.

	class Program
	{
		static void Main(string[] args)
		{
			//var arr = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }; var s = 15;
			var arr = new[] { 1, 2, 3, 7, 5 }; var s = 12;
			//var arr = new[] { 1, 2, 3, 4, 5, 0, 0, 0, 6, 7, 8, 9, 10 }; var s = 15;

			var result = findLongestSubarrayBySum(s, arr);
			if (result.Length == 1)
				Console.WriteLine("The subarray is not found.");
			else
				Console.WriteLine($"Lower bound (1-based): {result[0]}\nHigher bound (1-based): {result[1]}");
		}

		private static int[] findLongestSubarrayBySum(int s, int[] arr)
		{
			if (arr.Sum() == s)
			{
				return new[] { 1, arr.Length };
			}

			List<int[]> matchedSubarrays = new List<int[]>();
			var trimmedArray = new int[arr.Length];
			arr.CopyTo(trimmedArray, 0);

			int[] subarrayBounds = subarrayFinder(s, trimmedArray);
			while (subarrayBounds.Length != 1)
			{
				// Reconsile the indexes to the original array if the search was performed after it had been trimmed.
				var trimmedLength = arr.Length - trimmedArray.Length;
				matchedSubarrays.Add(new[] { subarrayBounds[0] + trimmedLength, subarrayBounds[1] + trimmedLength });

				// Remove the elements up to the lower bound of the found subarray and repeat the search
				// to find out all the matched and then sort them to find the longest one.
				trimmedArray = trimmedArray.Where((e, i) => i >= (subarrayBounds[0] + 1)).ToArray();
				subarrayBounds = subarrayFinder(s, trimmedArray);
			}
			if (matchedSubarrays.Count > 0)
			{
				// Sort by subarray length first, then by lower bound descending.
				matchedSubarrays.Sort((x, y) =>
				{
					int comp = (x[1] - x[0]).CompareTo(y[1] - y[0]);
					if (comp != 0)
					{
						return comp;
					}
					else
					{
						return -x[0].CompareTo(y[0]);
					}
				});
				var zeroBasedBounds = matchedSubarrays.Last();
				return new[] { zeroBasedBounds[0] + 1, zeroBasedBounds[1] + 1 };
			}
			return new[] { -1 };
		}

		private static int[] subarrayFinder(int s, int[] arr)
		{
			var success = false;
			var bounds = new int[2];
			bounds[0] = bounds[1] = 0;
			while (bounds[0] != arr.Length && bounds[1] != arr.Length)
			{
				var sum = arr.Where((e, i) => i >= bounds[0] && i <= bounds[1]).Sum();
				if (sum == s)
				{
					success = true;
					// Find the longest arrays eagerly, so catch all the trailing zeroes.
					while (bounds[1] != arr.Length - 1 && arr[bounds[1] + 1] == 0)
					{
						bounds[1]++;
					}
					break;
				}
				else if (sum > s)
				{
					if (bounds[0] < bounds[1])
					{
						bounds[0]++;
					}
					else
					{
						bounds[0]++;
						bounds[1]++;
					}
				}
				else if (sum < s)
				{
					bounds[1]++;
				}
			}

			if (success)
			{
				return bounds;
			}
			else
			{
				return new[] { -1 };
			}
		}
	}
}
