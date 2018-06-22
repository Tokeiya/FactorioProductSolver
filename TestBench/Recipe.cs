using System;
using System.Collections.Generic;
using FPS.CoreLib;
using FPS.CoreLib.Entity;

namespace TestBench
{
	public class Recipe
	{
		public static Recipe Create(TableElement source)
		{

#warning Create_Is_NotImpl
			throw new NotImplementedException("Create is not implemented");
		}


		public Recipe(string name, double energyRequired = 0.5
			, RecipeTypes recipeType = RecipeTypes.Normal,
			PhysicalProperties physicalProperty = PhysicalProperties.Item)
		{
			if (energyRequired <= 0) throw new ArgumentOutOfRangeException(nameof(energyRequired));
			if (!recipeType.Verify()) throw new ArgumentException($"{nameof(recipeType)} is unexpected types.");

			if (!physicalProperty.Verify())
				throw new ArgumentException($"{nameof(physicalProperty)} is unexpected types");

			Name = name ?? throw new ArgumentNullException(nameof(name));
			EnergyRequired = energyRequired;
			RecipeType = recipeType;
			PhysicalProperty = physicalProperty;

		}

		public string Name { get; }
		public double EnergyRequired { get; }
		public RecipeTypes RecipeType { get; }
		public PhysicalProperties PhysicalProperty { get; }

		private readonly List<(Recipe recipe, double amount)> _ingredients = new List<(Recipe recipe, double amount)>();
		public IReadOnlyList<(Recipe recipe, double amount)> Ingredients => _ingredients;

		public void AddIngredients(Recipe recipe, double amount) =>
			_ingredients.Add((recipe ?? throw new ArgumentNullException(nameof(recipe)),
				amount <= 0 ? throw new ArgumentOutOfRangeException(nameof(amount)) : amount));

		private readonly List<(Recipe recipe, double amount)> _results = new List<(Recipe recipe, double amount)>();

		public IReadOnlyList<(Recipe recipe, double amount)> Results => _results;

		public void AddResults(Recipe recipe, double amount) =>
			_results.Add((recipe ?? throw new ArgumentNullException(nameof(recipe)),
				amount <= 0 ? throw new ArgumentOutOfRangeException(nameof(amount)) : amount));
	}
}