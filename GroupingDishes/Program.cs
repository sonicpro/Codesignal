using System;
using System.Linq;
using System.Collections.Generic;
using SortedList = System.Collections.SortedList;

namespace GroupingDishes
{
	class Program
	{
		// Input is jagged array where each element is an array representing a dish.
		// The first element of the array element is a dish name, then ingridients.
		// The task is to write a function returning the jagged array of ingridients that are the component of at least two dishes.
		// The first element of an array element is the ingridient name, then dish names.
		// The array should be sorted lexicographically by ingridients, dish names inside the each element should be sorted lexicographically as well.
		static void Main(string[] args)
		{
			var dishes = new[]
			{
				new[] { "Pasta", "Tomato Sauce", "Onions", "Garlic" },
				new[] { "Chicken Curry", "Chicken", "Curry Sauce" },
				new[] { "Fried Rice", "Rice", "Onions", "Nuts" },
				new[] { "Salad", "Spinach", "Nuts" },
				new[] { "Sandwich", "Cheese", "Bread" },
				new[] { "Quesadilla", "Chicken", "Cheese" },
			};
			var ingridients = groupingDishes(dishes);
			foreach (var i in ingridients)
			{
				Console.WriteLine($"{i[0]} is an ingridient of:");
				Console.WriteLine(i.Where((s, index) => index != 0).Aggregate((prev, curr) => prev + ", " + curr));
				Console.WriteLine();
			}
		}

		private static string[][] groupingDishes(string[][] dishes)
		{
			Dictionary<string, SortedList> dishesByIngridient = new Dictionary<string, SortedList>();
			foreach (string[] dish in dishes)
			{
				for (int i = 1; i != dish.Length; i++)
				{
					SortedList dishNames = new SortedList();
					dishNames.Add(dish[0], null);
					// Dictionary<Tkey, TValue>.TryAdd is only available in .NET Core / .NET Standard.
					if (!dishesByIngridient.TryAdd(dish[i], dishNames))
					{
						dishesByIngridient[dish[i]].Add(dish[0], null);
					}
				}
			}
			string[] ingridientNames = new string[dishesByIngridient.Count];
			dishesByIngridient.Keys.CopyTo(ingridientNames, 0);
			foreach (string ingridient in ingridientNames)
			{
				if (dishesByIngridient[ingridient].Count < 2)
				{
					dishesByIngridient.Remove(ingridient);
				}
			}
			string[] sortedIngridients = new string[dishesByIngridient.Keys.Count];
			dishesByIngridient.Keys.CopyTo(sortedIngridients, 0);
			Array.Sort(sortedIngridients);
			string[][] groupedByIngridient = new string[sortedIngridients.Length][];
			int outerIndex = 0;
			foreach (string ingridient in sortedIngridients)
			{
				string[] ingridientFolowedByTheDishes = new string[dishesByIngridient[ingridient].Count + 1];
				ingridientFolowedByTheDishes[0] = ingridient;
				int index = 1;
				foreach (string dish in dishesByIngridient[ingridient].Keys)
				{
					ingridientFolowedByTheDishes[index] = dish;
					index++;
				}
				groupedByIngridient[outerIndex] = ingridientFolowedByTheDishes;
				outerIndex++;
			}
			return groupedByIngridient;
		}
	}
}
