﻿
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DyeEasy
{
	public class DyeEasy : Mod
	{
		//Hamstar's Mod Helpers integration
		public static string GithubUserName { get { return "goldenapple3"; } }
		public static string GithubProjectName { get { return "DyeEasy"; } }
	}

	public class DyeEasyRecipes : ModSystem
	{
		Dictionary<int, int> changedRecipes = new Dictionary<int, int>();

		public override void AddRecipes()
		{
			for (int i = 0; i < Main.recipe.Length; i++)
			{
				var recipe = Main.recipe[i];

				if (recipe.createItem.dye != 0)
				{
					int dyeIngredients = 0;
					var foundDyeTypes = new List<int>();
					bool foundBottledWater = false;

					foreach (var item in recipe.requiredItem)
					{
						if (item != null)
						{
							if (item.dye != 0)
							{
								dyeIngredients += item.stack;
								foundDyeTypes.Add(item.type);
							}
							else if (item.type == ItemID.BottledWater)
								foundBottledWater = true;
						}
					}
					int originalStack = recipe.createItem.stack;

					//Lunar dyes (ignore)
					if (foundBottledWater)
						continue;
					//Basic dyes
					if (dyeIngredients == 0)
						recipe.createItem.stack = 3;
					//Compound dyes
					else if (foundDyeTypes.Count > 1)
						recipe.createItem.stack = dyeIngredients;
					changedRecipes.Add(i, originalStack);
				}
			}
		}
		public override void Unload()
		{
			foreach (var kvp in changedRecipes)
				if (kvp.Key < Main.recipe.Length)
					Main.recipe[kvp.Key].createItem.stack = kvp.Value;
		}
	}
}
